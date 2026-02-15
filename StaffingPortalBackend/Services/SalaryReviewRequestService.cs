using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace StaffingPortalBackend.Services
{
    public class SalaryReviewRequestService : ISalaryReviewRequestService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISalaryReviewHistoryService _salaryReviewHistoryService;
        private readonly IManagementHierarchyService _hierarchyService;

        public SalaryReviewRequestService(AppDbContext context, IMapper mapper, ISalaryReviewHistoryService salaryReviewHistoryService, IManagementHierarchyService hierarchyService)
        {
            _context = context;
            _mapper = mapper;
            _salaryReviewHistoryService = salaryReviewHistoryService;
            _hierarchyService = hierarchyService;
        }

        public async Task<SalaryReviewRequest> CreateReviewRequest(SalaryReviewRequestDto requestDto)
        {
            var request = new SalaryReviewRequest
            {
                PersonId = requestDto.PersonId,
                ReviewDate = DateTime.Now,
                Justification = requestDto.Justification,
                NewSalaryStartDate = requestDto.NewSalaryStartDate,
                SalaryReceivedDate = requestDto.SalaryReceivedDate,
                TalentManagerComments = requestDto.TalentManagerComments,
                ContractType = requestDto.ContractType,
                Currency = requestDto.Currency,
                OldSalary = requestDto.OldSalary,
                NewSalary = requestDto.NewSalary,
                Status = "Pending"
            };

            _context.SalaryReviewRequests.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<IEnumerable<SalaryReviewRequest>> GetPendingRequests(int userId)
        {
            try
            {
                Console.WriteLine($"Searching for user with ID: {userId}");
                var user = await _context.Users
                    .Include(u => u.Person)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    Console.WriteLine("User not found");
                    throw new InvalidOperationException("User not found");
                }

                Console.WriteLine($"Found user: {user.Username}, Role: {user.Role}");

                var managerPersonId = await ResolveManagerPersonIdAsync(user);

                var pendingQuery = _context.SalaryReviewRequests
                    .Include(r => r.Person)
                    .Where(r => r.Status == "Pending");

                bool applyFilter = true;
                List<int> filterPersonIds = new();

                switch (user.Role)
                {
                    case "Head":
                        applyFilter = false;
                        if (managerPersonId.HasValue)
                        {
                            var permitted = await _hierarchyService.GetSubordinatePersonIdsAsync(managerPersonId.Value, includeManager: true);
                            if (permitted.Count > 0)
                            {
                                applyFilter = true;
                                filterPersonIds = permitted.ToList();
                            }
                        }
                        break;
                    case "DeputyHead":
                    case "DivisionManager":
                    case "TalentManager":
                        if (!managerPersonId.HasValue)
                        {
                            throw new InvalidOperationException("User is not linked to a person");
                        }
                        var subordinateIds = await _hierarchyService.GetSubordinatePersonIdsAsync(managerPersonId.Value, includeManager: true);
                        filterPersonIds = subordinateIds.ToList();
                        break;
                    default:
                        throw new UnauthorizedAccessException("Access denied");
                }

                if (applyFilter)
                {
                    if (filterPersonIds.Count == 0)
                    {
                        Console.WriteLine("No permitted subordinates found for user");
                        return Enumerable.Empty<SalaryReviewRequest>();
                    }

                    pendingQuery = pendingQuery.Where(r => r.Person != null && filterPersonIds.Contains(r.PersonId));
                }

                var finalResult = await pendingQuery.ToListAsync();
                Console.WriteLine($"Total filtered requests: {finalResult.Count}");
                return finalResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPendingRequests: {ex}");
                throw;
            }
        }

        private async Task<int?> ResolveManagerPersonIdAsync(User user)
        {
            if (user.PersonId.HasValue)
            {
                return user.PersonId.Value;
            }

            var personId = await _context.People
                .Where(p => p.UserId == user.Id)
                .Select(p => (int?)p.Id)
                .FirstOrDefaultAsync();

            return personId;
        }

        public async Task<SalaryReviewRequestDto> ApproveRequest(int requestId, SalaryReviewRequestDto requestDto)
        {
            Console.WriteLine($"Service: Starting to approve request with ID: {requestId}");

            try
            {
                Console.WriteLine($"Before fetching request: requestId = {requestId}");
                var request = await _context.SalaryReviewRequests
                    .Include(r => r.Person)
                    .FirstOrDefaultAsync(r => r.Id == requestId);

                if (request == null)
                {
                    throw new KeyNotFoundException($"Request with ID {requestId} not found.");
                }

                await _salaryReviewHistoryService.ArchivePreviousSalaryReview(request.PersonId);

                var newSalaryReview = _mapper.Map<SalaryReview>(requestDto);
                newSalaryReview.PersonId = request.PersonId;
                newSalaryReview.ReviewDate = DateTime.Now;
                newSalaryReview.IsApproved = true;

                _context.SalaryReviews.Add(newSalaryReview);

                request.Status = "Approved";

                await _context.SaveChangesAsync();

                return _mapper.Map<SalaryReviewRequestDto>(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ApproveRequest: {ex.Message}");
                throw;
            }
        }

        public async Task<SalaryReviewRequest> DeclineRequest(int requestId, string managerComments)
        {
            var request = await _context.SalaryReviewRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = "Declined";
                request.DivisionManagerComments = managerComments;
                await _context.SaveChangesAsync();
            }

            return request;
        }

        public async Task<SalaryReviewRequest> GetReviewRequest(int requestId)
        {
            return await _context.SalaryReviewRequests.FindAsync(requestId);
        }

        public async Task<IEnumerable<SalaryReviewRequestDto>> GetRequestsWithReviews()
        {
            var pendingRequestsWithReviews = await _context.SalaryReviewRequests
                .Where(srr => srr.Status == "Pending")
                .Include(srr => srr.Person)
                    .ThenInclude(p => p.SalaryReviews)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SalaryReviewRequestDto>>(pendingRequestsWithReviews);
        }

        public async Task<SalaryReviewRequest> UpdateFields(int id, UpdateFieldsDto updateFieldsDto)
        {
            var salaryReviewRequest = await _context.SalaryReviewRequests.FindAsync(id);
            if (salaryReviewRequest == null)
            {
                throw new InvalidOperationException("Salary Review Request not found");
            }

            _mapper.Map(updateFieldsDto, salaryReviewRequest);

            await _context.SaveChangesAsync();

            return salaryReviewRequest;
        }

        public async Task<SalaryReview> ApproveByDM(int id)
        {
            var salaryReview = await _context.SalaryReviews.FindAsync(id);

            if (salaryReview == null)
            {
                throw new InvalidOperationException("Salary Review not found");
            }

            salaryReview.ApprovedByDM = true;

            await _context.SaveChangesAsync();

            return salaryReview;
        }
    }
}