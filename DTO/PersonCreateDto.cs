namespace StaffingPortalBackend.DTO
{
    public class PersonCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string DivisionManager { get; set; }
        public string TalentManager { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string TechStack { get; set; }
        public string Comments { get; set; } 
        public string Stream { get; set; }
        public string TMAware { get; set; }
        public string Level { get; set; }
        public bool AssignmentExistsInGCP { get; set; }
    }
}