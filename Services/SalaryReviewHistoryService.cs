using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;
using StaffingPortalBackend.DTO;
using AutoMapper;

namespace StaffingPortalBackend.Services
{
    public class SalaryReviewHistoryService : ISalaryReviewHistoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SalaryReviewHistoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SalaryReviewHistoryReadDto>> GetSalaryReviewHistoryByPersonId(int personId)
        {
            var historyRecords = await _context.SalaryReviewHistories
                .Where(srh => srh.PersonId == personId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SalaryReviewHistoryReadDto>>(historyRecords);
        }

        public async Task ArchiveSalaryReview(int salaryReviewId)
        {
            var salaryReview = await _context.SalaryReviews.FindAsync(salaryReviewId);
            if (salaryReview != null)
            {
                var historyRecord = _mapper.Map<SalaryReviewHistory>(salaryReview);
                _context.SalaryReviewHistories.Add(historyRecord);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ArchivePreviousSalaryReview(int personId)
        {
            var salaryReviews = await _context.SalaryReviews
                .Where(sr => sr.PersonId == personId)
                .ToListAsync();  // асинхронное получение результатов запроса

            var previousSalaryReview = salaryReviews
                .OrderByDescending(sr => sr.ReviewDate.DateTime)
                .FirstOrDefault();  // синхронная сортировка и выборка на стороне клиента

            if (previousSalaryReview != null)
            {
                await ArchiveSalaryReview(previousSalaryReview.Id);
            }
        }
    }
}
