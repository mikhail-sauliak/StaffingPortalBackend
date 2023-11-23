using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System.Globalization;

namespace StaffingPortalBackend.Services
{
    public class SalaryReviewRequestService : ISalaryReviewRequestService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISalaryReviewHistoryService _salaryReviewHistoryService;

        public SalaryReviewRequestService(AppDbContext context, IMapper mapper, ISalaryReviewHistoryService salaryReviewHistoryService)
        {
            _context = context;
            _mapper = mapper;
            _salaryReviewHistoryService = salaryReviewHistoryService;
        }

        public async Task<SalaryReviewRequest> CreateReviewRequest(SalaryReviewRequestDto requestDto)
        {
            var request = new SalaryReviewRequest
            {
                PersonId = requestDto.PersonId,
                ReviewDate = DateTime.Now,
                Justification = requestDto.Justification,
                NewSalaryStartDate = requestDto.NewSalaryStartDate,
                SalaryReceivedDate = requestDto.SalaryReceivedDate,
                TalentManagerComments = requestDto.TalentManagerComments,
                ContractType = requestDto.ContractType,
                Currency = requestDto.Currency,
                OldSalary = requestDto.OldSalary,
                NewSalary = requestDto.NewSalary,
                Status = "Pending"
            };

            _context.SalaryReviewRequests.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<IEnumerable<SalaryReviewRequest>> GetPendingRequests(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new InvalidOperationException("User not found");

            IQueryable<SalaryReviewRequest> query = _context.SalaryReviewRequests.Where(r => r.Status == "Pending");

            switch (user.Role)
            {
                case "DivisionManager":
                    query = query.Where(r => r.Person.DivisionManagerId == userId);
                    break;
                case "DeputyHead":
                    query = query.Where(r => r.Person.DivisionManagerId == userId);
                    break;
                case "Head":
                    break;
                default:
                    throw new UnauthorizedAccessException("Access denied");
            }

            return await query.ToListAsync();
        }

        public async Task<SalaryReviewRequestDto> ApproveRequest(int requestId, SalaryReviewRequestDto requestDto)
        {
            Console.WriteLine($"Service: Starting to approve request with ID: {requestId}");
            try
            {
                var request = await _context.SalaryReviewRequests
                    .Include(r => r.Person)
                    .ThenInclude(p => p.SalaryReviews)
                    .FirstOrDefaultAsync(r => r.Id == requestId);

                if (request == null)
                {
                    throw new KeyNotFoundException("Review request not found.");
                }

                request.Status = "Approved";
                request.DivisionManagerComments = requestDto.DivisionManagerComments;                

                var isArchived = await _salaryReviewHistoryService.ArchivePreviousSalaryReview(request.PersonId);
                if (!isArchived)
                {
                    throw new InvalidOperationException("Failed to archive previous salary review.");
                }               

                    var newSalaryReview = new SalaryReview
                    {
                        PersonId = request.PersonId,
                        ReviewDate = requestDto.ReviewDate,
                        Justification = requestDto.Justification,
                        OldSalary = requestDto.OldSalary,
                        ProposedSalary = requestDto.NewSalary,
                        Currency = requestDto.Currency,
                        ContractType = requestDto.ContractType,
                        NewSalaryStartDate = requestDto.NewSalaryStartDate,
                        SalaryReceivedDate = requestDto.SalaryReceivedDate,
                        DivisionManagerComments = requestDto.DivisionManagerComments,
                        TalentManagerComments = requestDto.TalentManagerComments,
                        IsApproved = true,
                        ApprovedByDM = true,
                        ReviewResult = "Approved"
                    };

                _context.SalaryReviews.Add(newSalaryReview);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Salary Review Request Approved: {requestId}");

                return _mapper.Map<SalaryReviewRequestDto>(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ApproveRequest: {ex.Message}");
                throw;
            }
        }


        public async Task<SalaryReviewRequest> DeclineRequest(int requestId, string managerComments)
        {
            var request = await _context.SalaryReviewRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = "Declined";
                request.DivisionManagerComments = managerComments;
                await _context.SaveChangesAsync();
            }

            return request;
        }

        public async Task<SalaryReviewRequest> GetReviewRequest(int requestId)
        {
            return await _context.SalaryReviewRequests.FindAsync(requestId);
        }

        public async Task<IEnumerable<SalaryReviewRequestDto>> GetRequestsWithReviews()
        {
            var pendingRequestsWithReviews = await _context.SalaryReviewRequests
                .Where(srr => srr.Status == "Pending")
                .Include(srr => srr.Person)
                    .ThenInclude(p => p.SalaryReviews)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SalaryReviewRequestDto>>(pendingRequestsWithReviews);
        }

        public async Task<SalaryReview> UpdateFields(int id, UpdateFieldsDto updateFieldsDto)
        {
            var salaryReview = await _context.SalaryReviews.FindAsync(id);

            if (salaryReview == null)
            {
                throw new InvalidOperationException("Salary Review not found");
            }

            salaryReview.ProposedSalary = updateFieldsDto.ProposedSalary;
            salaryReview.Currency = updateFieldsDto.Currency;
            salaryReview.ContractType = updateFieldsDto.ContractType;
            salaryReview.NewSalaryStartDate = updateFieldsDto.NewSalaryStartDate;
            salaryReview.SalaryReceivedDate = updateFieldsDto.SalaryReceivedDate;
            salaryReview.DivisionManagerComments = updateFieldsDto.DivisionManagerComments;

            if (updateFieldsDto.IsApproved.HasValue)
            {
                salaryReview.IsApproved = updateFieldsDto.IsApproved.Value;
            }

            await _context.SaveChangesAsync();

            return salaryReview;
        }
        public async Task<SalaryReview> ApproveByDM(int id)
        {
            var salaryReview = await _context.SalaryReviews.FindAsync(id);

            if (salaryReview == null)
            {
                throw new InvalidOperationException("Salary Review not found");
            }

            salaryReview.ApprovedByDM = true;

            await _context.SaveChangesAsync();

            return salaryReview;
        }
    }
}