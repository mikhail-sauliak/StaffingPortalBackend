namespace StaffingPortalBackend.DTO
{
    public class PersonAdminDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DivisionManagerDto DivisionManager { get; set; }
        public TalentManagerDto TalentManager { get; set; }
        public string ProjectAssignment { get; set; }
        public string TechStack { get; set; }
        public string Level { get; set; }
        public string Location { get; set; }
        public DateTime AvailableFrom { get; set; }
        public bool TMAware { get; set; }
    }
}