using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public bool IsDismissed { get; set; } = false;
        public DateTimeOffset AvailableFrom { get; set; }
        public string? Comments { get; set; }
        public string TechStack { get; set; }
        public ICollection<ProjectCandidate>? ProjectCandidates { get; set; }
        public string Stream { get; set; } // QA, SDET
        public string TMAware { get; set; } // Not, Notified, Approves
        public string? PlannedAssignment { get; set; } // Project name and Start Date
        public string Level { get; set; } // Intern, Junior, Middle, Senior, Lead, Principal
        public bool AssignmentExistsInGCP { get; set; } // Yes, No
        public int? UserId { get; set; }
        public User User { get; set; }
        public ICollection<SalaryReview> SalaryReviews { get; set; }
        public int? DivisionManagerId { get; set; }
        public Person DivisionManager { get; set; }
        public int? TalentManagerId { get; set; }
        public Person TalentManager { get; set; }
        // For DivisionManager
        public List<Person> ManagedPersonsAsDivisionManager { get; set; }
        // For TalentManager
        public List<Person> ManagedPersonsAsTalentManager { get; set; }
    }
}