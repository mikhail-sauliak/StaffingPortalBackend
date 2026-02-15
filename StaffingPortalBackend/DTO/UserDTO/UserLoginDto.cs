using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username must be at most 30 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, ErrorMessage = "Password must be at most 20 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
