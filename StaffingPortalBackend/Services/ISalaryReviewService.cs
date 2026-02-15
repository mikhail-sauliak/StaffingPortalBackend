using StaffingPortalBackend.DTO;

namespace StaffingPortalBackend.Services
{
    public interface ISalaryReviewService
    {
        Task<IEnumerable<SalaryReviewReadDto>> GetAllSalaryReviews(bool isTalentManager);
        Task<SalaryReviewReadDto> GetSalaryReviewById(int id);
        Task<SalaryReviewReadDto> CreateSalaryReview(SalaryReviewCreateDto reviewDto);
        Task UpdateSalaryReview(int id, SalaryReviewUpdateDto reviewDto);
        Task DeleteSalaryReview(int id);
        Task<SalaryReviewReadDto> GetCurrentSalaryByPersonId(int personId);
        Task UpdatePartialSalaryReview(int id, SalaryReviewPatchDto salaryReviewPatchDto);
        Task<int?> GetPersonIdBySalaryReviewId(int salaryReviewId);
    }
}