using System.Collections.Generic;

namespace StaffingPortalBackend.DTO
{
    public class PersonUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string DivisionManager { get; set; }
        public string ResourceManager { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string Comments { get; set; }
        public ICollection<int> ProjectCandidateIds { get; set; } // Если вы хотите обновлять связанные сущности
    }
}
