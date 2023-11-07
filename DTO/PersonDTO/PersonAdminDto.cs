namespace StaffingPortalBackend.DTO
{
    public class PersonAdminDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string DivisionManager { get; set; }
        public string TalentManager { get; set; }
        public string ProjectAssignment { get; set; }
    }
}