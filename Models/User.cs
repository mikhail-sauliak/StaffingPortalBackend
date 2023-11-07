using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffingPortalBackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } // Head, DeputyHead, DivisionManager, TalentManager
        public int? PersonId { get; set; } // Link to the Person if the user is a TalentManager or DivisionManager
        public Person Person { get; set; }
        public string Stream { get; set; }
        public ICollection<User> ManagedUsers { get; set; }
    }
}
