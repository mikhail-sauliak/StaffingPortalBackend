
namespace StaffingPortalBackend.Models
{
    public class ManagementHierarchy
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int? ParentId { get; set; }
        public ManagementHierarchy Parent { get; set; }

        public string Level { get; set; }

        public List<ManagementHierarchy> Subordinates { get; set; } = new();
    }
}