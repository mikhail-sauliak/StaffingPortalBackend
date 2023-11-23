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

        public async Task<bool> ArchivePreviousSalaryReview(int personId)
        {
            try
            {
                var salaryReviews = await _context.SalaryReviews
                    .Where(sr => sr.PersonId == personId)
                    .ToListAsync();

                var previousSalaryReview = salaryReviews
                    .OrderByDescending(sr => sr.ReviewDate.Date)
                    .FirstOrDefault();

                if (previousSalaryReview != null)
                {
                    var historyRecord = _mapper.Map<SalaryReviewHistory>(previousSalaryReview);
                    if (historyRecord == null)
                    {
                        Console.WriteLine("Mapping to SalaryReviewHistory resulted in a null object.");
                        return false;
                    }

                    _context.SalaryReviewHistories.Add(historyRecord);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Archived Salary Review with HistoryId: {historyRecord.HistoryId}");

                    _context.SalaryReviews.Remove(previousSalaryReview);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine("No existing salary review found for archiving.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ArchivePreviousSalaryReview: {ex.Message}");
                return false;
            }

            return true;
        }


    }
}
