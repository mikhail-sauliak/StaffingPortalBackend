using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.Models
{
    public class Project
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string TechStack { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Comments { get; set; }
    public ICollection<ProjectCandidate> ProjectCandidates { get; set; }
}
}