using System.Collections.Generic;

namespace StaffingPortalBackend.DTO.UserDTO
{
    public class HierarchyNodeDto
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string Level { get; set; }
        public string Role { get; set; }
        public string Stream { get; set; }
        public IReadOnlyCollection<HierarchyNodeDto> Subordinates { get; set; } = new List<HierarchyNodeDto>();
    }
}
