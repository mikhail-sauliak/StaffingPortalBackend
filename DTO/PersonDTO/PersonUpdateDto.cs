using System.Collections.Generic;

namespace StaffingPortalBackend.DTO
{
    public class PersonUpdateDto : PersonBaseDto
    {
        public ICollection<int>? ProjectCandidateIds { get; set; }
        public int? UserId { get; set; }    
    }
}
