using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffingPortalBackend.Services
{
    public interface ISalaryReviewHistoryService
    {
        Task<IEnumerable<SalaryReviewHistoryReadDto>> GetAllHistoryAsync();
        Task<IEnumerable<SalaryReviewHistoryReadDto>> GetHistoryByPersonIdAsync(int personId);
        Task<SalaryReviewHistoryReadDto> GetHistoryByIdAsync(int id);
        Task CreateHistoryAsync(SalaryReviewHistoryCreateDto historyDto);
        Task UpdateHistoryAsync(SalaryReviewHistoryUpdateDto historyDto);
        Task DeleteHistoryAsync(int id);
        Task ArchiveSalaryReview(int salaryReviewId);
        Task<bool> ArchivePreviousSalaryReview(int personId);
        Task<List<MonthlySalaryAggregationDto>> GetAggregatedMonthlySalariesByYear(int year, User currentUser);
        Task<IEnumerable<AggregatedSalaryDto>> GetAggregatedSalariesForAllTime(User currentUser);
        Task<IEnumerable<AggregatedSalaryDto>> GetAggregatedSalariesByYear(int year);
    }
}
