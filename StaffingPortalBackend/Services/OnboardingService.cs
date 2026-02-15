using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.DTO.Onboarding;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public class OnboardingService : IOnboardingService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IManagementHierarchyService _hierarchyService;

        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int HashIterations = 10000;

        public OnboardingService(
            AppDbContext context,
            IMapper mapper,
            IManagementHierarchyService hierarchyService)
        {
            _context = context;
            _mapper = mapper;
            _hierarchyService = hierarchyService;
        }

        public async Task<PersonOnboardingResponseDto> OnboardPersonAsync(PersonOnboardingRequestDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var person = await CreatePersonAsync(request.Person);

                var response = new PersonOnboardingResponseDto
                {
                    Person = _mapper.Map<PersonReadDto>(person)
                };

                if (request.User != null)
                {
                    response.User = await CreateUserAsync(person, request.User);
                }

                if (request.ManagerAssignments != null)
                {
                    var assignmentResults = await ApplyManagerAssignmentsAsync(person, request.ManagerAssignments);
                    response.UpdatedAssignments = assignmentResults;
                }

                if (request.SalaryReview != null)
                {
                    response.SalaryReview = await CreateInitialSalaryReviewAsync(person.Id, request.SalaryReview);
                }

                await transaction.CommitAsync();

                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<Person> CreatePersonAsync(PersonCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var existingPerson = await _context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.FirstName == dto.FirstName && p.LastName == dto.LastName);

            if (existingPerson != null)
            {
                throw new InvalidOperationException($"Person with name {dto.FirstName} {dto.LastName} already exists.");
            }

            var personEntity = _mapper.Map<Person>(dto);

            await _context.People.AddAsync(personEntity);
            await _context.SaveChangesAsync();

            return personEntity;
        }

        private async Task<UserReadDto> CreateUserAsync(Person person, InitialUserSetupDto userSetup)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            if (userSetup == null)
            {
                throw new ArgumentNullException(nameof(userSetup));
            }

            var normalizedUsername = userSetup.Username.Trim();

            var existingUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == normalizedUsername);

            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with username {normalizedUsername} already exists.");
            }

            var salt = GenerateSalt();
            var passwordHash = HashPassword(userSetup.Password, salt);

            var user = new User
            {
                Username = normalizedUsername,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                Role = userSetup.Role,
                Stream = userSetup.Stream,
                PersonId = person.Id,
                ManagedUsers = new List<User>()
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            person.UserId = user.Id;
            _context.People.Update(person);
            await _context.SaveChangesAsync();

            if (userSetup.ManagerPersonId.HasValue && userSetup.ManagerPersonId.Value > 0)
            {
                var managerExists = await _context.People.AnyAsync(p => p.Id == userSetup.ManagerPersonId.Value);
                if (!managerExists)
                {
                    throw new InvalidOperationException("Specified manager was not found.");
                }

                await _hierarchyService.AddSubordinateAsync(userSetup.ManagerPersonId.Value, person.Id);
            }

            var createdUser = await _context.Users
                .Include(u => u.Person)
                    .ThenInclude(p => p.DivisionManager)
                .Include(u => u.Person)
                    .ThenInclude(p => p.TalentManager)
                .FirstAsync(u => u.Id == user.Id);

            return _mapper.Map<UserReadDto>(createdUser);
        }

        private async Task<IDictionary<string, IReadOnlyCollection<int>>> ApplyManagerAssignmentsAsync(Person newManager, ManagerAssignmentsDto assignments)
        {
            var results = new Dictionary<string, IReadOnlyCollection<int>>(StringComparer.OrdinalIgnoreCase);

            var talentManagerUpdates = await UpdateManagerAssignmentsAsync(newManager.Id, assignments.TalentManagerPersonIds, isTalentManager: true);
            if (talentManagerUpdates.Count > 0)
            {
                results["TalentManager"] = talentManagerUpdates;
            }

            var divisionManagerUpdates = await UpdateManagerAssignmentsAsync(newManager.Id, assignments.DivisionManagerPersonIds, isTalentManager: false);
            if (divisionManagerUpdates.Count > 0)
            {
                results["DivisionManager"] = divisionManagerUpdates;
            }

            return results;
        }

        private async Task<IReadOnlyCollection<int>> UpdateManagerAssignmentsAsync(int managerId, IEnumerable<int> subordinateIds, bool isTalentManager)
        {
            if (subordinateIds == null)
            {
                return Array.Empty<int>();
            }

            var uniqueIds = subordinateIds
                .Where(id => id > 0 && id != managerId)
                .Distinct()
                .ToArray();

            if (uniqueIds.Length == 0)
            {
                return Array.Empty<int>();
            }

            var subordinates = await _context.People
                .Where(p => uniqueIds.Contains(p.Id))
                .ToListAsync();

            foreach (var subordinate in subordinates)
            {
                if (isTalentManager)
                {
                    subordinate.TalentManagerId = managerId;
                }
                else
                {
                    subordinate.DivisionManagerId = managerId;
                }
            }

            if (subordinates.Count > 0)
            {
                await _context.SaveChangesAsync();
            }

            return subordinates.Select(p => p.Id).ToArray();
        }

        private async Task<SalaryReviewReadDto> CreateInitialSalaryReviewAsync(int personId, InitialSalaryReviewSetupDto salaryReview)
        {
            var salaryReviewDto = new SalaryReviewCreateDto
            {
                PersonId = personId,
                ReviewDate = salaryReview.ReviewDate,
                Justification = salaryReview.Justification,
                OldSalary = salaryReview.OldSalary,
                ProposedSalary = salaryReview.ProposedSalary,
                Currency = salaryReview.Currency,
                ContractType = salaryReview.ContractType,
                NewSalaryStartDate = salaryReview.NewSalaryStartDate,
                SalaryReceivedDate = salaryReview.SalaryReceivedDate,
                DivisionManagerComments = salaryReview.DivisionManagerComments,
                TalentManagerComments = salaryReview.TalentManagerComments,
                ReviewResult = salaryReview.ReviewResult,
                IsApproved = salaryReview.IsApproved
            };

            var salaryReviewEntity = _mapper.Map<SalaryReview>(salaryReviewDto);
            salaryReviewEntity.ApprovedByDM = salaryReview.ApprovedByDivisionManager;

            await _context.SalaryReviews.AddAsync(salaryReviewEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<SalaryReviewReadDto>(salaryReviewEntity);
        }

        private static byte[] GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);
            return salt;
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, salt, HashIterations, HashAlgorithmName.SHA256);
            return algorithm.GetBytes(KeySize);
        }
    }
}
