using StaffingPortalBackend.DTO.UserDTO;

namespace StaffingPortalBackend.Services
{
    public interface IManagementHierarchyService
    {
        Task<HierarchyNodeDto?> GetHierarchyByManagerIdAsync(int managerPersonId);
        Task<IReadOnlyCollection<HierarchyNodeDto>> GetAllHierarchiesAsync();
        Task<IReadOnlyCollection<int>> GetSubordinatePersonIdsAsync(int managerPersonId, bool includeManager = false, bool includeIndirect = true);
        Task AddSubordinateAsync(int managerPersonId, int subordinatePersonId);
        Task RemoveSubordinateAsync(int managerPersonId, int subordinatePersonId);
        Task UpdateHierarchyLevelAsync(int personId, string newLevel);
    }
}