using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class UserCreateDto : UserBaseDto
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character")]
        public string Password { get; set; } // necessary to create a user
    }
}