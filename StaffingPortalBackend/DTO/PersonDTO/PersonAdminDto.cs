namespace StaffingPortalBackend.DTO
{
    public class PersonAdminDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Stream { get; set; }
        public DivisionManagerDto DivisionManager { get; set; }
        public TalentManagerDto TalentManager { get; set; }
        public int? DivisionManagerId { get; set; }
        public int? TalentManagerId { get; set; }
        public int? ManagerPersonId { get; set; }
        public string ManagerFullName { get; set; }
        public string PlannedAssignment { get; set; }
        public string TechStack { get; set; }
        public string Level { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string TMAware { get; set; }
        public bool IsDismissed { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AssignmentExistsInGCP { get; set; }
        public int? UserId { get; set; }
        public int? HeadId { get; set; }
        public string HeadFullName { get; set; }
        public int? DeputyHeadId { get; set; }
        public string DeputyHeadFullName { get; set; }
    }
}