using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsers();
        Task<UserReadDto> GetUserById(int id);
        Task<UserReadDto> CreateUser(User user);
        Task UpdateUser(int id, UserUpdateDto userDto);
        Task DeleteUser(int id);
        Task<User> Authenticate(string username, string password);
        byte[] GenerateSalt();
        string HashPassword(string password, byte[] salt);
        bool VerifyPassword(string enteredPassword, string storedHash, byte[] storedSalt);
        Task ChangePassword(int userId, string oldPassword, string newPassword);
    }
}