using Microsoft.EntityFrameworkCore;
using StaffingPortalBackend.Models;

namespace StaffingPortalBackend.Services
{
    public class HierarchyService : IManagementHierarchyService
    {
        private readonly AppDbContext _context;

        public HierarchyService(AppDbContext context)
        {
            _context = context;
        }

        // Ensure this method is asynchronous and returns a Task
        public async Task AddSubordinate(int managerId, int subordinateId)
        {
            var managerHierarchy = await _context.ManagementHierarchies
                .Include(mh => mh.Subordinates)
                .FirstOrDefaultAsync(mh => mh.ManagerId == managerId);

            if (managerHierarchy == null)
            {
                // Handle the error: manager not found
                throw new Exception("Manager not found.");
            }

            var subordinateHierarchy = new ManagementHierarchy
            {
                ManagerId = subordinateId
                // Set other properties as needed
            };

            managerHierarchy.Subordinates.Add(subordinateHierarchy);
            await _context.SaveChangesAsync();
        }

        // Ensure this method is asynchronous and returns a Task
        public async Task RemoveSubordinate(int managerId, int subordinateId)
        {
            var managerHierarchy = await _context.ManagementHierarchies
                .Include(mh => mh.Subordinates)
                .FirstOrDefaultAsync(mh => mh.ManagerId == managerId);

            if (managerHierarchy == null)
            {
                // Handle the error: manager not found
                throw new Exception("Manager not found.");
            }

            var subordinateHierarchy = managerHierarchy.Subordinates
                .FirstOrDefault(mh => mh.ManagerId == subordinateId);
            if (subordinateHierarchy != null)
            {
                managerHierarchy.Subordinates.Remove(subordinateHierarchy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ManagementHierarchy?> GetHierarchyByManagerId(int managerId)
        {
            return await _context.ManagementHierarchies
                .Include(mh => mh.Subordinates)
                .FirstOrDefaultAsync(mh => mh.ManagerId == managerId);
        }

        public async Task<IEnumerable<ManagementHierarchy>> GetAllHierarchies()
        {
            return await _context.ManagementHierarchies
                .Include(mh => mh.Subordinates)
                .ToListAsync();
        }

        public async Task UpdateHierarchy(ManagementHierarchy updatedHierarchy)
        {
            var hierarchy = await _context.ManagementHierarchies
                .FirstOrDefaultAsync(mh => mh.Id == updatedHierarchy.Id);

            if (hierarchy == null)
            {
                throw new Exception("Hierarchy not found.");
            }

            hierarchy.Level = updatedHierarchy.Level;

            _context.ManagementHierarchies.Update(hierarchy);
            await _context.SaveChangesAsync();
        }
    }
}
