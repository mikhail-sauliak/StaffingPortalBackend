using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

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
                Justification = requestDto.Justification,
                NewSalaryStartDate = requestDto.NewSalaryStartDate,
                SalaryReceivedDate = requestDto.SalaryReceivedDate,
                TalentManagerComments = requestDto.TalentManagerComments,
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
                case "DivisionManager" :
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

        public async Task<SalaryReviewRequest> ApproveRequest(int requestId, string managerComments)
        {
            var request = await _context.SalaryReviewRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = "Approved";
                request.DivisionManagerComments = managerComments;
                await _salaryReviewHistoryService.ArchivePreviousSalaryReview(request.PersonId);
                await _context.SaveChangesAsync();
            }

            return request;
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
            // Находим запрос на пересмотр зарплаты по id
            var salaryReview = await _context.SalaryReviews.FindAsync(id);

            if (salaryReview == null)
            {
                throw new InvalidOperationException("Salary Review not found");
            }

            // Обновляем поля запроса на пересмотр зарплаты
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

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();

            return salaryReview;
        }
        public async Task<SalaryReview> ApproveByDM(int id)
        {
            // Находим запрос на пересмотр зарплаты по id
            var salaryReview = await _context.SalaryReviews.FindAsync(id);

            if (salaryReview == null)
            {
                throw new InvalidOperationException("Salary Review not found");
            }

            // Обновляем поле ApprovedByDM запроса на пересмотр зарплаты
            salaryReview.ApprovedByDM = true;

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();

            return salaryReview;
        }
    }    
}