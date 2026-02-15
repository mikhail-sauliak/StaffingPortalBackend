using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffingPortalBackend.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string TechStack { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comments { get; set; }
        public ICollection<ProjectCandidate> ProjectCandidates { get; set; }
        public string Search { get; set; } = "Draft";
        public bool Signed { get; set; } = false;
        public DateTime? PositionSigned { get; set; }
        public DateTime? PositionClosed { get; set; }
        public string Location { get; set; }
        public string Stream { get; set; }
        public string Level { get; set; }
        public string Priority { get; set; }
        public string? Attachment { get; set; } // path to file or URL
    }
}