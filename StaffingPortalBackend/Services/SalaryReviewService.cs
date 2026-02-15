using Microsoft.EntityFrameworkCore;
using AutoMapper;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System;

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
            IQueryable<SalaryReview> query = _context.SalaryReviews;

            if (isTalentManager)
            {
                query = query.Select(r => new SalaryReview 
                {
                    Id = r.Id,
                    PersonId = r.PersonId,
                    ReviewDate = r.ReviewDate,
                    Justification = r.Justification,
                    OldSalary = 0,  
                    ProposedSalary = 0,  
                    IsApproved = r.IsApproved,
                    Currency = r.Currency,
                    ContractType = r.ContractType,
                    NewSalaryStartDate = r.NewSalaryStartDate,
                    SalaryReceivedDate = r.SalaryReceivedDate,
                    DivisionManagerComments = r.DivisionManagerComments,
                    TalentManagerComments = r.TalentManagerComments,
                });
            }

            var reviewsDto = await _mapper.ProjectTo<SalaryReviewReadDto>(query).ToListAsync();

            return reviewsDto;
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

        public async Task UpdatePartialSalaryReview(int id, SalaryReviewPatchDto salaryReviewPatchDto)
        {
            var salaryReview = await _context.SalaryReviews.FindAsync(id);
            if (salaryReview == null)
            {
                throw new KeyNotFoundException("SalaryReview not found.");
            }
            _mapper.Map(salaryReviewPatchDto, salaryReview);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSalaryReview(int id)
        {
            var review = await _context.SalaryReviews.FindAsync(id);
            _context.SalaryReviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<SalaryReviewReadDto> GetCurrentSalaryByPersonId(int personId)
        {
            var currentSalary = await _context.SalaryReviews
                .Where(sr => sr.PersonId == personId && sr.IsApproved)
                .OrderByDescending(sr => sr.ReviewDate)
                .FirstOrDefaultAsync();

            return _mapper.Map<SalaryReviewReadDto>(currentSalary);
        }

        public async Task<int?> GetPersonIdBySalaryReviewId(int salaryReviewId)
        {
            var salaryReview = await _context.SalaryReviews.FindAsync(salaryReviewId);
            return salaryReview?.PersonId;
        }
    }
}