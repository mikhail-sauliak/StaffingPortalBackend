using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class UserBaseDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }

        [MaxLength(100)]
        public string Division { get; set; }

        [MaxLength(100)]
        public string ManagedBy { get; set; }

        public int? PersonId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Stream { get; set; }

        [Required]
        public int ManagerId { get; set; }
    }
}