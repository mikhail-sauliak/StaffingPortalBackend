using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public class SalaryReviewService : ISalaryReviewService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SalaryReviewService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SalaryReviewReadDto>> GetAllSalaryReviews(bool isTalentManager)
        {
            var reviews = await _context.SalaryReviews.ToListAsync();
            if (isTalentManager)
            {
                reviews = reviews.Select(r =>
                {
                    r.OldSalary = 0;
                    r.ProposedSalary = 0;
                    return r;
                }).ToList();
            }
            return _mapper.Map<IEnumerable<SalaryReviewReadDto>>(reviews);
        }

        public async Task<SalaryReviewReadDto> GetSalaryReviewById(int id)
        {
            var review = await _context.SalaryReviews.FindAsync(id);
            return _mapper.Map<SalaryReviewReadDto>(review);
        }

        public async Task<SalaryReviewReadDto> CreateSalaryReview(SalaryReviewCreateDto reviewDto)
        {
            var review = _mapper.Map<SalaryReview>(reviewDto);
            _context.SalaryReviews.Add(review);
            await _context.SaveChangesAsync();
            return _mapper.Map<SalaryReviewReadDto>(review);
        }

        public async Task UpdateSalaryReview(int id, SalaryReviewUpdateDto reviewDto)
        {
            var review = await _context.SalaryReviews.FindAsync(id);
            _mapper.Map(reviewDto, review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSalaryReview(int id)
        {
            var review = await _context.SalaryReviews.FindAsync(id);
            _context.SalaryReviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}