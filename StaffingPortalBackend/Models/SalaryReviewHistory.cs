using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffingPortalBackend.Models
{
    public class SalaryReviewHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryId { get; set; }
        
        public int SalaryReviewId { get; set; }

        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public DateTime ReviewDate { get; set; }
        public string Justification { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OldSalary { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProposedSalary { get; set; }
        public bool IsApproved { get; set; }
        public string Currency { get; set; }
        public string ContractType { get; set; }
        public DateTime? NewSalaryStartDate { get; set; }
        public DateTime? SalaryReceivedDate { get; set; }
        public string DivisionManagerComments { get; set; }
        public string TalentManagerComments { get; set; }
        public string ReviewResult { get; set; }
        public DateTime ArchivedDate { get; set; }  // Date when this record was archived
    }
}
