using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public interface IManagementHierarchyService
    {
        Task<ManagementHierarchy> GetHierarchyByManagerId(int managerId);
        Task<IEnumerable<ManagementHierarchy>> GetAllHierarchies();
        Task AddSubordinate(int managerId, int subordinateId);
        Task RemoveSubordinate(int managerId, int subordinateId);
        Task UpdateHierarchy(ManagementHierarchy hierarchy);
    }
}