namespace StaffingPortalBackend.DTO
{
    // Добавьте другие свойства, которые вы хотите включить
    public class PersonReadDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string DivisionManager { get; set; }
        public string ResourceManager { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string Comments { get; set; }
        // Добавьте другие свойства, которые вы хотите включить
    }
        
}
