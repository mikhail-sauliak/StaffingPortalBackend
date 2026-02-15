using System;
using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class SalaryReviewHistoryCreateDto
    {
        [Required]
        public int PersonId { get; set; }
        [Required]
        public int SalaryReviewId { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }
        public string Justification { get; set; }
        [Required]
        public decimal OldSalary { get; set; }
        [Required]
        public decimal ProposedSalary { get; set; }
        public bool IsApproved { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string ContractType { get; set; }
        public DateTime? NewSalaryStartDate { get; set; }
        public DateTime? SalaryReceivedDate { get; set; }
        public string DivisionManagerComments { get; set; }
        public string TalentManagerComments { get; set; }
        public string ReviewResult { get; set; }
    }
}