using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string DivisionManager { get; set; }
        public string ResourceManager { get; set; }
        public string AvailableFrom { get; set; }
        public string Comments { get; set; }
    }
}