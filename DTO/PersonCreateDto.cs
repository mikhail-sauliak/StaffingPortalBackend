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
        public string Comments { get; set; }
    }
}