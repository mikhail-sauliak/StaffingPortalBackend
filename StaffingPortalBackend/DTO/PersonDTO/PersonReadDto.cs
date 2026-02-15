using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class PersonReadDto : PersonBaseDto
    {
        public int Id { get; set; }
        [MaxLength(500)]
        public string PlannedAssignment { get; set; }
        public DateTime? EndDate { get; set; }
    }    
}
