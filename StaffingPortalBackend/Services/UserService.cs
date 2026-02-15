using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System;
using System.Security.Cryptography;

namespace StaffingPortalBackend.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IManagementHierarchyService _hierarchyService;

        public UserService(AppDbContext context, IMapper mapper, IManagementHierarchyService hierarchyService)
        {
            _context = context;
            _mapper = mapper;
            _hierarchyService = hierarchyService;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.Person)
                    .ThenInclude(p => p.DivisionManager)
                .Include(u => u.Person)
                    .ThenInclude(p => p.TalentManager)
                .ToListAsync();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<UserReadDto> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Person)
                    .ThenInclude(p => p.DivisionManager)
                .Include(u => u.Person)
                    .ThenInclude(p => p.TalentManager)
                .FirstOrDefaultAsync(u => u.Id == id);
            return _mapper.Map<UserReadDto>(user);
        }

        // public async Task<ServiceResponse<UserReadDto>> CreateUser(UserCreateDto userDto)
        // {
        //     var response = new ServiceResponse<UserReadDto>();

        //     var existingUser = _context.Users.FirstOrDefault(u => u.PersonId == userDto.PersonId);
        //     if (existingUser != null)
        //     {
        //         response.Success = false;
        //         response.Message = "A user already exists for the provided PersonId.";
        //         return response;
        //     }

        //     using var hmac = new System.Security.Cryptography.HMACSHA512();
        //     var user = new User
        //     {
        //         Username = userDto.Username,
        //         PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userDto.Password)),
        //         PasswordSalt = hmac.Key,
        //         Role = userDto.Role,
        //         Stream = userDto.Stream,
        //         PersonId = userDto.PersonId,                
        //     };

        //     _context.Users.Add(user);
        //     await _context.SaveChangesAsync();
        //     if (user.PersonId.HasValue)
        //     {
        //         var person = await _context.People.FindAsync(user.PersonId.Value);
        //         if (person != null)
        //         {
        //             person.UserId = user.Id;
        //             _context.People.Update(person);
        //             await _context.SaveChangesAsync();
        //         }
        //     }

        //     response.Data = _mapper.Map<UserReadDto>(user);
        //     return response;
        // }

        public async Task<ServiceResponse<UserReadDto>> CreateUser(UserCreateDto userDto)
        {
            var response = new ServiceResponse<UserReadDto>();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingPerson = await _context.People
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == userDto.PersonId);

                if (existingPerson == null)
                {
                    response.Success = false;
                    response.Message = "Person not found for the provided PersonId.";
                    await transaction.RollbackAsync();
                    return response;
                }

                if (existingPerson.User != null)
                {
                    response.Success = false;
                    response.Message = "This person already has an associated User.";
                    await transaction.RollbackAsync();
                    return response;
                }

                if (userDto.ManagerId == existingPerson.Id)
                {
                    response.Success = false;
                    response.Message = "ManagerId cannot be the same as the subordinate.";
                    await transaction.RollbackAsync();
                    return response;
                }

                var salt = GenerateSalt();
                var hashedPassword = HashPassword(userDto.Password, salt);

                var user = new User
                {
                    Username = userDto.Username,
                    PasswordHash = Convert.FromBase64String(hashedPassword),
                    PasswordSalt = salt,
                    Role = userDto.Role,
                    Stream = userDto.Stream,
                    PersonId = userDto.PersonId
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                existingPerson.UserId = user.Id;
                _context.People.Update(existingPerson);
                await _context.SaveChangesAsync();

                if (userDto.ManagerId > 0)
                {
                    await _hierarchyService.AddSubordinateAsync(userDto.ManagerId, existingPerson.Id);
                }

                await transaction.CommitAsync();

                var createdUser = await _context.Users
                    .Include(u => u.Person)
                        .ThenInclude(p => p.DivisionManager)
                    .Include(u => u.Person)
                        .ThenInclude(p => p.TalentManager)
                    .FirstAsync(u => u.Id == user.Id);

                response.Data = _mapper.Map<UserReadDto>(createdUser);
                response.Success = true;
                response.Message = "User created and associated with the person successfully.";
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.Success = false;
                response.Message = $"Failed to create user: {ex.Message}";
                return response;
            }
        }

        public async Task UpdateUser(int id, UserUpdateDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            _mapper.Map(userDto, user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;

        public string HashPassword(string password, byte[] salt)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(algorithm.GetBytes(KeySize));
        }

        public byte[] GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);
            return salt;
        }

        public bool VerifyPassword(string enteredPassword, string storedHash, byte[] storedSalt)
        {
            var newHash = HashPassword(enteredPassword, storedSalt);
            return newHash == storedHash;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            try
            {
                Console.WriteLine("Username from controller is:" + username);
                Console.WriteLine("Password from controller is:" + password);

                var normalizedUsername = username.Trim().ToLower();
                var userFromDb = await _context.Users
                    .Include(u => u.Person)
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == normalizedUsername);

                if (userFromDb == null)
                {
                    Console.WriteLine($"User with username {username} not found in database.");
                    return null;
                }

                Console.WriteLine($"Found user with username: {userFromDb.Username}");

                if (!VerifyPassword(password, Convert.ToBase64String(userFromDb.PasswordHash), userFromDb.PasswordSalt))
                    return null;

                return userFromDb;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                throw;
            }
        }

        public async Task ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            if (!VerifyPassword(oldPassword, Convert.ToBase64String(user.PasswordHash), user.PasswordSalt))
                throw new Exception("Old password is incorrect");

            var newSalt = GenerateSalt();
            var newHash = HashPassword(newPassword, newSalt);
            user.PasswordSalt = newSalt;
            user.PasswordHash = Convert.FromBase64String(newHash);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Person>> GetTalentManagersByDivisionManager(int divisionManagerId)
        {
            var talentManagers = await _context.People
                .Include(p => p.User)
                .Where(p => p.TalentManagerId == divisionManagerId && p.User != null && p.User.Role == "TalentManager")
                .ToListAsync();

            return talentManagers;
        }
    }
}
