
namespace StaffingPortalBackend.DTO;
public class SalaryReviewHistoryReadDto : SalaryReviewBaseDto
    {
        public int HistoryId { get; set; }
        public DateTimeOffset ArchivedDate { get; set; }
    }