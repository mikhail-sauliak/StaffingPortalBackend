namespace StaffingPortalBackend.DTO;

public class ProjectUpdateDto
{
    public string Name { get; set; }
    public string TechStack { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Comments { get; set; }
    public List<int> CandidateIds { get; set; }
}
