using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string TechStack { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string Comments { get; set; }
        public ICollection<ProjectCandidate> ProjectCandidates { get; set; }
        public string Search { get; set; } = "Draft";
        public bool Signed { get; set; } = false;
        public DateTimeOffset? PositionSigned { get; set; }
        public DateTimeOffset? PositionClosed { get; set; }
        public string Location { get; set; }
        public string Stream { get; set; }
        public string Level { get; set; }
        public string Priority { get; set; }
        public string? Attachment { get; set; } // path to file or URL
    }
}