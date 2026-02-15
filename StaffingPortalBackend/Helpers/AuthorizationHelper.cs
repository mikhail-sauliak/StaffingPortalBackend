using StaffingPortalBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using StaffingPortalBackend.Services;

public class AuthorizationHelper
{
    private readonly AppDbContext _context;
    private readonly IManagementHierarchyService _hierarchyService;

    public AuthorizationHelper(AppDbContext context, IManagementHierarchyService hierarchyService)
    {
        _context = context;
        _hierarchyService = hierarchyService;
    }

    public async Task<bool> CanUserAccessDataAsync(User currentUser, int targetPersonId)
    {
        if (targetPersonId <= 0)
        {
            return false;
        }

        if (currentUser.PersonId.HasValue && currentUser.PersonId.Value == targetPersonId)
        {
            return true;
        }

        if (currentUser.Role == "Head")
        {
            return true;
        }

        if ((currentUser.Role == "DeputyHead" || currentUser.Role == "DivisionManager" || currentUser.Role == "TalentManager")
            && currentUser.PersonId.HasValue)
        {
            var subordinateIds = await _hierarchyService.GetSubordinatePersonIdsAsync(currentUser.PersonId.Value, includeManager: false);
            return subordinateIds.Contains(targetPersonId);
        }

        return false;
    }

    public async Task<HashSet<int>> GetAccessiblePersonIdsAsync(User currentUser)
    {
        var accessiblePersonIds = new HashSet<int>();

        if (currentUser == null)
        {
            return accessiblePersonIds;
        }

        if (currentUser.PersonId.HasValue)
        {
            accessiblePersonIds.Add(currentUser.PersonId.Value);

            if ((currentUser.Role == "DeputyHead" || currentUser.Role == "DivisionManager" || currentUser.Role == "TalentManager")
                && currentUser.PersonId.HasValue)
            {
                var subordinateIds = await _hierarchyService.GetSubordinatePersonIdsAsync(currentUser.PersonId.Value, includeManager: false);
                foreach (var subordinateId in subordinateIds)
                {
                    accessiblePersonIds.Add(subordinateId);
                }
            }
        }

        return accessiblePersonIds;
    }

    public bool CanUserAccessSalaryReviewPage(User currentUser)
    {
        return currentUser.Role == "Head" || currentUser.Role == "DeputyHead" || currentUser.Role == "DivisionManager";
    }

    public bool IsAuthorizedForAdminOperations(ClaimsPrincipal user)
    {
        var userRoles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);

        return userRoles.Any(role => role == "Head" || role == "DeputyHead" || role == "DivisionManager");
    }
}
