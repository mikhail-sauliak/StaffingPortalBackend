using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class UpdateLevelDto
    {
        [Required]
        public string NewLevel { get; set; }
        [Required]
        public string Justification { get; set; }
    }
}
