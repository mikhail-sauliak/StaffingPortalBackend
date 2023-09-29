using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class ProjectCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "TechStack is required.")]
        [StringLength(500, ErrorMessage = "TechStack length can't be more than 500.")]
        public string TechStack { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }

        // Comments can be null, so no [Required] attribute here
        [StringLength(1000, ErrorMessage = "Comments length can't be more than 1000.")]
        public string? Comments { get; set; }

        // CandidateIds can be null, so it's a nullable type
        public List<int>? CandidateIds { get; set; }

        [Required(ErrorMessage = "Search is required.")]
        [StringLength(50, ErrorMessage = "Search length can't be more than 50.")]
        public string Search { get; set; } = "Draft";

        [Required(ErrorMessage = "Signed is required.")]
        public bool Signed { get; set; } = false;

        // PositionSigned can be null, so it's a nullable type
        public DateTime? PositionSigned { get; set; }

        // PositionClosed can be null, so it's a nullable type
        public DateTime? PositionClosed { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location length can't be more than 100.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Stream is required.")]
        [StringLength(50, ErrorMessage = "Stream length can't be more than 50.")]
        public string Stream { get; set; }

        [Required(ErrorMessage = "Level is required.")]
        [StringLength(50, ErrorMessage = "Level length can't be more than 50.")]
        public string Level { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [StringLength(50, ErrorMessage = "Priority length can't be more than 50.")]
        public string Priority { get; set; }

        // Attachment can be null, so no [Required] attribute here
        // Placeholder for future implementation
        public string? Attachment { get; set; }
    }
}
