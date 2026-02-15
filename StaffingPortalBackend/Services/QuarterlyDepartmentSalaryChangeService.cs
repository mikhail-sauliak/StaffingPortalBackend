using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.DTO;
using StaffingPortalBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffingPortalBackend.Services
{
    public class QuarterlyDepartmentSalaryChangeService : IQuarterlyDepartmentSalaryChangeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public QuarterlyDepartmentSalaryChangeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuarterlyDepartmentSalaryChangeDto>> GetAllQuarterlyChanges()
        {
            var changes = await _context.QuarterlyDepartmentSalaryChanges.ToListAsync();
            return _mapper.Map<IEnumerable<QuarterlyDepartmentSalaryChangeDto>>(changes);
        }

        public async Task<QuarterlyDepartmentSalaryChangeDto> GetQuarterlyChangeById(int id)
        {
            var change = await _context.QuarterlyDepartmentSalaryChanges.FindAsync(id);
            return _mapper.Map<QuarterlyDepartmentSalaryChangeDto>(change);
        }

        public async Task<QuarterlyDepartmentSalaryChangeDto> CreateQuarterlyChange(QuarterlyDepartmentSalaryChangeDto dto)
        {
            var existingChange = await _context.QuarterlyDepartmentSalaryChanges
                .FirstOrDefaultAsync(x => x.Year == dto.Year);

            if (existingChange != null)            {
                
                throw new InvalidOperationException($"A quarterly change record for the year {dto.Year} already exists.");
            }

            var change = _mapper.Map<QuarterlyDepartmentSalaryChange>(dto);
            _context.QuarterlyDepartmentSalaryChanges.Add(change);
            await _context.SaveChangesAsync();
            return _mapper.Map<QuarterlyDepartmentSalaryChangeDto>(change);
        }

        public async Task UpdateQuarterlyChange(int id, QuarterlyDepartmentSalaryChangeDto dto)
        {
            var change = await _context.QuarterlyDepartmentSalaryChanges.FindAsync(id);
            if (change == null)
            {
                throw new KeyNotFoundException("Quarterly change not found.");
            }
            _mapper.Map(dto, change);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuarterlyChange(int id)
        {
            var change = await _context.QuarterlyDepartmentSalaryChanges.FindAsync(id);
            if (change != null)
            {
                _context.QuarterlyDepartmentSalaryChanges.Remove(change);
                await _context.SaveChangesAsync();
            }
        }
    }
}
