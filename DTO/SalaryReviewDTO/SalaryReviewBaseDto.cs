using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class SalaryReviewBaseDto
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public DateTimeOffset ReviewDate { get; set; }

        [MaxLength(500)]
        public string Justification { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReviewResult { get; set; }

        [Required]
        public decimal OldSalary { get; set; }  // Убран атрибут Column

        [Required]
        public decimal ProposedSalary { get; set; }  // Убран атрибут Column

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(10)]
        public string ContractType { get; set; }

        public DateTimeOffset? NewSalaryStartDate { get; set; }

        public DateTimeOffset? SalaryReceivedDate { get; set; }

        [MaxLength(500)]
        public string DivisionManagerComments { get; set; }

        [MaxLength(500)]
        public string TalentManagerComments { get; set; }
    }
}
