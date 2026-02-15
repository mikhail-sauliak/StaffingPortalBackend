using StaffingPortalBackend.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffingPortalBackend.Services
{
    public interface IQuarterlyDepartmentSalaryChangeService
    {
        Task<IEnumerable<QuarterlyDepartmentSalaryChangeDto>> GetAllQuarterlyChanges();
        Task<QuarterlyDepartmentSalaryChangeDto> GetQuarterlyChangeById(int id);
        Task<QuarterlyDepartmentSalaryChangeDto> CreateQuarterlyChange(QuarterlyDepartmentSalaryChangeDto dto);
        Task UpdateQuarterlyChange(int id, QuarterlyDepartmentSalaryChangeDto dto);
        Task DeleteQuarterlyChange(int id);
    }
}
