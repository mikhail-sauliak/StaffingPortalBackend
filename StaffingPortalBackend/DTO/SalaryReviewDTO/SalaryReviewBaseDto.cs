using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class SalaryReviewBaseDto
    {
        [Required(ErrorMessage = "PersonId is required.")]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "ReviewDate is required.")]
        public DateTime ReviewDate { get; set; }

        [MaxLength(500, ErrorMessage = "Justification cannot exceed 500 characters.")]
        public string Justification { get; set; }

        [Required(ErrorMessage = "ReviewResult is required.")]
        [MaxLength(100, ErrorMessage = "ReviewResult cannot exceed 100 characters.")]
        public string ReviewResult { get; set; }

        [Required(ErrorMessage = "OldSalary is required.")]
        public decimal OldSalary { get; set; } 

        [Required(ErrorMessage = "ProposedSalary is required.")]
        public decimal ProposedSalary { get; set; }

        [Required(ErrorMessage = "IsApproved is required.")]
        public bool IsApproved { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        [MaxLength(10, ErrorMessage = "Currency cannot exceed 10 characters.")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "ContractType is required.")]
        [MaxLength(10, ErrorMessage = "ContractType cannot exceed 10 characters.")]
        public string ContractType { get; set; }

        public DateTime? NewSalaryStartDate { get; set; }

        public DateTime? SalaryReceivedDate { get; set; }

        [MaxLength(500, ErrorMessage = "DivisionManagerComments cannot exceed 500 characters.")]
        public string DivisionManagerComments { get; set; }

        [MaxLength(500, ErrorMessage = "TalentManagerComments cannot exceed 500 characters.")]
        public string TalentManagerComments { get; set; }
    }
}
