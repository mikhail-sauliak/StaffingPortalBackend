using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class SalaryReviewBaseDto
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        [MaxLength(500)]
        public string Justification { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReviewResult { get; set; }

        [Required]
        public decimal OldSalary { get; set; } 

        [Required]
        public decimal ProposedSalary { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(10)]
        public string ContractType { get; set; }

        public DateTime? NewSalaryStartDate { get; set; }

        public DateTime? SalaryReceivedDate { get; set; }

        [MaxLength(500)]
        public string DivisionManagerComments { get; set; }

        [MaxLength(500)]
        public string TalentManagerComments { get; set; }
    }
}
