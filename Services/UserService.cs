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

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<UserReadDto> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserReadDto>(user);
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

                var allUsers = await _context.Users.ToListAsync();
                var userFromDb = allUsers.FirstOrDefault(u => u.Username.Trim().Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));

                if (userFromDb == null)
                {
                    Console.WriteLine($"User with username {username} not found in database.");
                    return null;
                }

                Console.WriteLine($"Found user with username: {userFromDb.Username}");

                if (!VerifyPassword(password, Convert.ToBase64String(userFromDb.PasswordHash), userFromDb.PasswordSalt))
                    return null;

                // if (userFromDb.ManagerId == null)
                // {
                //     userFromDb.ManagerId = 0; // или любое другое значение по умолчанию
                // }

                return userFromDb;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка:" + ex.Message);
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
    }
}
