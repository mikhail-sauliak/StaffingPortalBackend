using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using StaffingPortalBackend.DTO.UserDTO;
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

        public async Task AddSubordinateAsync(int managerPersonId, int subordinatePersonId)
        {
            if (managerPersonId == subordinatePersonId)
            {
                throw new InvalidOperationException("Manager and subordinate cannot be the same person.");
            }

            var manager = await _context.People.FindAsync(managerPersonId) ?? throw new InvalidOperationException("Manager not found.");
            var subordinate = await _context.People.FindAsync(subordinatePersonId) ?? throw new InvalidOperationException("Subordinate not found.");

            var managerNode = await EnsureNodeAsync(manager.Id);
            var subordinateNode = await EnsureNodeAsync(subordinate.Id);

            if (subordinateNode.ParentId == managerNode.Id)
            {
                return;
            }

            if (await IsAncestorAsync(subordinateNode.Id, managerNode.Id))
            {
                throw new InvalidOperationException("Cannot create a cyclic relationship in the hierarchy.");
            }

            subordinateNode.ParentId = managerNode.Id;
            subordinateNode.Level = subordinate.Level;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveSubordinateAsync(int managerPersonId, int subordinatePersonId)
        {
            var managerNode = await _context.ManagementHierarchies.FirstOrDefaultAsync(mh => mh.PersonId == managerPersonId);
            if (managerNode == null)
            {
                throw new InvalidOperationException("Manager hierarchy node not found.");
            }

            var subordinateNode = await _context.ManagementHierarchies.FirstOrDefaultAsync(mh => mh.PersonId == subordinatePersonId);
            if (subordinateNode == null || subordinateNode.ParentId != managerNode.Id)
            {
                throw new InvalidOperationException("Subordinate relationship not found.");
            }

            subordinateNode.ParentId = null;
            await _context.SaveChangesAsync();
        }

        public async Task<HierarchyNodeDto?> GetHierarchyByManagerIdAsync(int managerPersonId)
        {
            var (nodes, lookupByPersonId, childrenLookup) = await LoadHierarchyAsync();

            if (!lookupByPersonId.TryGetValue(managerPersonId, out var rootNode))
            {
                return null;
            }

            return BuildHierarchyNodeDto(rootNode, childrenLookup);
        }

        public async Task<IReadOnlyCollection<HierarchyNodeDto>> GetAllHierarchiesAsync()
        {
            var (nodes, _, childrenLookup) = await LoadHierarchyAsync();
            var roots = nodes.Where(n => n.ParentId == null).ToList();

            return roots
                .Select(root => BuildHierarchyNodeDto(root, childrenLookup))
                .ToList();
        }

        public async Task<IReadOnlyCollection<int>> GetSubordinatePersonIdsAsync(int managerPersonId, bool includeManager = false, bool includeIndirect = true)
        {
            var (_, lookupByPersonId, childrenLookup) = await LoadHierarchyAsync();

            if (!lookupByPersonId.TryGetValue(managerPersonId, out var root))
            {
                return Array.Empty<int>();
            }

            if (!childrenLookup.TryGetValue(root.Id, out var directSubordinates))
            {
                return includeManager ? new[] { root.PersonId } : Array.Empty<int>();
            }

            var result = new HashSet<int>();

            if (includeManager)
            {
                result.Add(root.PersonId);
            }

            if (!includeIndirect)
            {
                foreach (var child in directSubordinates)
                {
                    result.Add(child.PersonId);
                }

                return result.ToList();
            }

            var stack = new Stack<ManagementHierarchy>(directSubordinates);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (result.Add(node.PersonId) && childrenLookup.TryGetValue(node.Id, out var grandchildren))
                {
                    foreach (var grandChild in grandchildren)
                    {
                        stack.Push(grandChild);
                    }
                }
            }

            return result.ToList();
        }

        public async Task UpdateHierarchyLevelAsync(int personId, string newLevel)
        {
            var node = await _context.ManagementHierarchies.FirstOrDefaultAsync(mh => mh.PersonId == personId)
                ?? throw new InvalidOperationException("Hierarchy node not found.");

            node.Level = newLevel;
            await _context.SaveChangesAsync();
        }

        private async Task<ManagementHierarchy> EnsureNodeAsync(int personId)
        {
            var existingNode = await _context.ManagementHierarchies.FirstOrDefaultAsync(mh => mh.PersonId == personId);
            if (existingNode != null)
            {
                return existingNode;
            }

            var person = await _context.People.FindAsync(personId) ?? throw new InvalidOperationException("Person not found.");

            var node = new ManagementHierarchy
            {
                PersonId = person.Id,
                Level = person.Level
            };

            _context.ManagementHierarchies.Add(node);
            await _context.SaveChangesAsync();

            return node;
        }

        private async Task<bool> IsAncestorAsync(int potentialAncestorNodeId, int nodeId)
        {
            var hierarchy = await _context.ManagementHierarchies
                .Select(mh => new { mh.Id, mh.ParentId })
                .ToListAsync();

            var parentLookup = hierarchy.ToDictionary(h => h.Id, h => h.ParentId);

            var current = nodeId;
            while (parentLookup.TryGetValue(current, out var parentId) && parentId.HasValue)
            {
                if (parentId.Value == potentialAncestorNodeId)
                {
                    return true;
                }

                current = parentId.Value;
            }

            return false;
        }

        private async Task<(List<ManagementHierarchy> nodes, Dictionary<int, ManagementHierarchy> byPersonId, Dictionary<int, List<ManagementHierarchy>> childrenLookup)> LoadHierarchyAsync()
        {
            var nodes = await _context.ManagementHierarchies
                .Include(mh => mh.Person)
                    .ThenInclude(p => p.User)
                .AsNoTracking()
                .ToListAsync();

            var byPersonId = nodes.ToDictionary(node => node.PersonId, node => node);
            var childrenLookup = nodes
                .Where(node => node.ParentId.HasValue)
                .GroupBy(node => node.ParentId!.Value)
                .ToDictionary(group => group.Key, group => group.ToList());

            return (nodes, byPersonId, childrenLookup);
        }

        private static HierarchyNodeDto BuildHierarchyNodeDto(ManagementHierarchy root, Dictionary<int, List<ManagementHierarchy>> childrenLookup)
        {
            var dto = new HierarchyNodeDto
            {
                PersonId = root.PersonId,
                FullName = root.Person == null ? string.Empty : $"{root.Person.FirstName} {root.Person.LastName}",
                Level = root.Level,
                Role = root.Person?.User?.Role,
                Stream = root.Person?.Stream,
                Subordinates = Array.Empty<HierarchyNodeDto>()
            };

            if (!childrenLookup.TryGetValue(root.Id, out var children) || children.Count == 0)
            {
                return dto;
            }

            var subordinateDtos = new List<HierarchyNodeDto>(children.Count);
            foreach (var child in children)
            {
                subordinateDtos.Add(BuildHierarchyNodeDto(child, childrenLookup));
            }

            dto.Subordinates = subordinateDtos;
            return dto;
        }
    }
}
