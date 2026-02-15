using System.Collections.Generic;
using StaffingPortalBackend.DTO;

namespace StaffingPortalBackend.DTO.Onboarding
{
    public class PersonOnboardingResponseDto
    {
        public PersonReadDto Person { get; set; } = null!;

        public UserReadDto? User { get; set; }

        public SalaryReviewReadDto? SalaryReview { get; set; }

        public IDictionary<string, IReadOnlyCollection<int>> UpdatedAssignments { get; set; }
            = new Dictionary<string, IReadOnlyCollection<int>>();
    }
}
