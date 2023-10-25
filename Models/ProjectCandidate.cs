namespace StaffingPortalBackend.Models;

public class ProjectCandidate
{
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    
    public int PersonId { get; set; }
    public Person Person { get; set; }
}