using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
