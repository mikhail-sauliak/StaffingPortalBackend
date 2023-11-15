
namespace StaffingPortalBackend.Models
{
    public class ManagementHierarchy
    {
        public int Id { get; set; }
        public int ManagerId { get; set; } 
        public string Level { get; set; }
        public List<ManagementHierarchy> Subordinates { get; set; }
    }
}