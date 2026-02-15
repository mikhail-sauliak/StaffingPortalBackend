using System;

namespace StaffingPortalBackend.DTO
{
    public class TitleHistoryDto
    {
        public int Id { get; set; } // The ID of the title change entry
        public int PersonId { get; set; } // The ID of the person associated with this title change
        public string OldTitle { get; set; } // The old title before the change
        public string NewTitle { get; set; } // The new title after the change
        public DateTime DateChanged { get; set; } // The date when the title change occurred
        public string Justification { get; set; } // The rationale behind the title change
    }
}