using StaffingPortalBackend.DTO;

namespace StaffingPortalBackend.Services
{
    public interface ISalaryReviewHistoryService
    {
        Task<IEnumerable<SalaryReviewHistoryReadDto>> GetSalaryReviewHistoryByPersonId(int personId);
        Task ArchiveSalaryReview(int salaryReviewId);
        Task<bool> ArchivePreviousSalaryReview(int personId);
    }
}
