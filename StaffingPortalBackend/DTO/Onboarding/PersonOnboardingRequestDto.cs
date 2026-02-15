using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StaffingPortalBackend.DTO;

namespace StaffingPortalBackend.DTO.Onboarding
{
    public class PersonOnboardingRequestDto
    {
        [Required]
        public PersonCreateDto Person { get; set; } = null!;

        public InitialUserSetupDto? User { get; set; }

        public InitialSalaryReviewSetupDto? SalaryReview { get; set; }

        public ManagerAssignmentsDto? ManagerAssignments { get; set; }
    }

    public class InitialUserSetupDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Stream { get; set; } = string.Empty;

        public int? ManagerPersonId { get; set; }

        [MaxLength(100)]
        public string? Division { get; set; }

        [MaxLength(100)]
        public string? ManagedBy { get; set; }
    }

    public class InitialSalaryReviewSetupDto
    {
        [Required]
        public DateTime ReviewDate { get; set; }

        [MaxLength(500)]
        public string? Justification { get; set; }

        [Required]
        public decimal OldSalary { get; set; }

        [Required]
        public decimal ProposedSalary { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string ContractType { get; set; } = string.Empty;

        public DateTime? NewSalaryStartDate { get; set; }

        public DateTime? SalaryReceivedDate { get; set; }

        [MaxLength(500)]
        public string? DivisionManagerComments { get; set; }

        [MaxLength(500)]
        public string? TalentManagerComments { get; set; }

        [MaxLength(100)]
        public string ReviewResult { get; set; } = "Approved";

        public bool IsApproved { get; set; } = true;

        public bool ApprovedByDivisionManager { get; set; } = true;
    }

    public class ManagerAssignmentsDto
    {
        public List<int> TalentManagerPersonIds { get; set; } = new();

        public List<int> DivisionManagerPersonIds { get; set; } = new();
    }
}
