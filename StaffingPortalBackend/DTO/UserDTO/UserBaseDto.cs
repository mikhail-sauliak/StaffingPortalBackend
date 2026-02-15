using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class UserBaseDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [MaxLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; }

        [MaxLength(100, ErrorMessage = "Division cannot exceed 100 characters.")]
        public string Division { get; set; }

        [MaxLength(100, ErrorMessage = "ManagedBy cannot exceed 100 characters.")]
        public string ManagedBy { get; set; }

        public int? PersonId { get; set; }

        [Required(ErrorMessage = "Stream is required.")]
        [MaxLength(100, ErrorMessage = "Stream cannot exceed 100 characters.")]
        public string Stream { get; set; }

        [Required(ErrorMessage = "ManagerId is required.")]
        public int ManagerId { get; set; }
    }
}