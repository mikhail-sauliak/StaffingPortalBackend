
namespace StaffingPortalBackend.DTO;
public class SalaryReviewHistoryReadDto : SalaryReviewBaseDto
    {
        public int HistoryId { get; set; }
        public DateTime ArchivedDate { get; set; }
    }