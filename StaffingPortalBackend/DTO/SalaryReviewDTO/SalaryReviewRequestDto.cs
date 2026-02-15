using System;
using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class SalaryReviewRequestDto
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public string TalentManagerComments { get; set; }

        public string? DivisionManagerComments { get; set; }

        public bool IsApproved { get; set; }

        public bool? ApprovedByDM { get; set; }

        public bool? ApprovedByHead { get; set; }

        public bool? ApprovedByVP { get; set; }

        [Required(ErrorMessage = "The Justification field is required.")]
        public string Justification { get; set; }

        [Required(ErrorMessage = "The NewSalaryStartDate field is required.")]
        public DateTime NewSalaryStartDate { get; set; }

        [Required(ErrorMessage = "The SalaryReceivedDate field is required.")]
        public DateTime SalaryReceivedDate { get; set; }

        [Required(ErrorMessage = "The ContractType field is required.")]
        public string ContractType { get; set; }

        [Required(ErrorMessage = "The Currency field is required.")]
        public string Currency { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The OldSalary field must be a positive number.")]
        public decimal OldSalary { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The NewSalary field must be a positive number.")]
        public decimal NewSalary { get; set; }

        [Required(ErrorMessage = "The ReviewDate field is required.")]
        public DateTime ReviewDate { get; set; }

        [Required(ErrorMessage = "The Status field is required.")]
        public string Status { get; set; }
    }
}
