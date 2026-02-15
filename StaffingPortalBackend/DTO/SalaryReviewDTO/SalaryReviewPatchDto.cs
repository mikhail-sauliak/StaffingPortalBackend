using System.ComponentModel.DataAnnotations;

namespace StaffingPortalBackend.DTO
{
    public class SalaryReviewPatchDto
    {
        public DateTime? ReviewDate { get; set; }
        public string Justification { get; set; }
        public decimal? OldSalary { get; set; }
        public decimal? ProposedSalary { get; set; }
        public string Currency { get; set; }
        public string ContractType { get; set; }
        public DateTime? NewSalaryStartDate { get; set; }
        public DateTime? SalaryReceivedDate { get; set; }
        public string DivisionManagerComments { get; set; }
        public string TalentManagerComments { get; set; }
        public bool? IsApproved { get; set; }
    }
}