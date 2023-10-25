using StaffingPortalBackend.Models;
using System.Linq;

public class AuthorizationHelper
{
    private readonly AppDbContext _context;

    public AuthorizationHelper(AppDbContext context)
    {
        _context = context;
    }

    public bool CanUserAccessData(User currentUser, int targetUserId)
    {
        Console.WriteLine($"Checking access for User {currentUser.Id} to data of User {targetUserId}");
        if (currentUser.Role == "Head" || currentUser.Role == "DeputyHead")
            return true;

        if (currentUser.Role == "DivisionManager" && IsUserInDivisionManagerPool(currentUser, targetUserId))
            return true;

        if (currentUser.Role == "TalentManager" && IsUserInTalentManagerPool(currentUser, targetUserId))
            return true;

        return false;
    }

    private bool IsUserInDivisionManagerPool(User divisionManager, int targetUserId)
    {
       var targetUser = _context.People.FirstOrDefault(p => p.Id == targetUserId);
        var access = targetUser != null && targetUser.DivisionManagerId == divisionManager.Id;
        Console.WriteLine($"Division Manager {divisionManager.Id} access to User {targetUserId}: {access}");
        return access;
    }

    private bool IsUserInTalentManagerPool(User talentManager, int targetUserId)
    {
        var targetUser = _context.People.FirstOrDefault(p => p.Id == targetUserId);
        var access = targetUser != null && targetUser.TalentManagerId == talentManager.Id;
        Console.WriteLine($"Talent Manager {talentManager.Id} access to User {targetUserId}: {access}");
        return access;
    }

    public bool CanUserAccessSalaryReviewPage(User currentUser)
    {
        return currentUser.Role == "Head" || currentUser.Role == "DeputyHead" || currentUser.Role == "DivisionManager";
    }
}
