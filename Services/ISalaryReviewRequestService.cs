using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public interface ISalaryReviewRequestService
    {
        Task<SalaryReviewRequest> CreateReviewRequest(SalaryReviewRequestDto requestDto);
        Task<IEnumerable<SalaryReviewRequest>> GetPendingRequests(int userId);
        Task<SalaryReviewRequest> ApproveRequest(int requestId, string managerComments);
        Task<SalaryReviewRequest> DeclineRequest(int requestId, string managerComments);
        Task<SalaryReviewRequest> GetReviewRequest(int requestId);
        Task<IEnumerable<SalaryReviewRequestDto>> GetRequestsWithReviews();
        Task<SalaryReview> UpdateFields(int id, UpdateFieldsDto updateFieldsDto);
        Task<SalaryReview> ApproveByDM(int id);
    }
}
