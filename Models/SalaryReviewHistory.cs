using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffingPortalBackend.Models
{
    public class SalaryReviewHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [ForeignKey("SalaryReview")]
        public int SalaryReviewId { get; set; }
        public SalaryReview SalaryReview { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public DateTimeOffset ReviewDate { get; set; }
        public string Justification { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OldSalary { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProposedSalary { get; set; }
        public bool IsApproved { get; set; }
        public string Currency { get; set; }
        public string ContractType { get; set; }
        public DateTimeOffset? NewSalaryStartDate { get; set; }
        public DateTimeOffset? SalaryReceivedDate { get; set; }
        public string DivisionManagerComments { get; set; }
        public string TalentManagerComments { get; set; }
        public string ReviewResult { get; set; }
        public DateTimeOffset ArchivedDate { get; set; }  // Date when this record was archived
    }
}
