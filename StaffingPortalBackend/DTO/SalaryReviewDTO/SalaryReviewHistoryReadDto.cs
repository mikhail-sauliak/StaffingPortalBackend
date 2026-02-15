
namespace StaffingPortalBackend.DTO;
public class SalaryReviewHistoryReadDto : SalaryReviewHistoryCreateDto
{
    public int HistoryId { get; set; }
    public DateTime ArchivedDate { get; set; }
}