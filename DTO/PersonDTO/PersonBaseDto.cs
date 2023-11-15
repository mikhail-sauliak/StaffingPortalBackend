using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class PersonBaseDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        [Required]
        public int? DivisionManagerId { get; set; }

        [Required]
        public int? TalentManagerId { get; set; }

        [Required]
        public DateTimeOffset AvailableFrom { get; set; }

        [Required]
        [MaxLength(100)]
        public string TechStack { get; set; }

        [MaxLength(1000)]
        public string? Comments { get; set; }

        [Required]
        [MaxLength(4)]
        public string Stream { get; set; }

        [Required]
        [MaxLength(10)]
        public string TMAware { get; set; }

        [Required]
        [MaxLength(20)]
        public string Level { get; set; }
        public bool IsDismissed { get; set; }
        [Required]
        public bool AssignmentExistsInGCP { get; set; }
        public int? UserId { get; set; }
        public int? HeadId { get; set; }
        public string? HeadName { get; set; }
        public int? DeputyHeadId { get; set; }
        public string? DeputyHeadName { get; set; }
    }
}
