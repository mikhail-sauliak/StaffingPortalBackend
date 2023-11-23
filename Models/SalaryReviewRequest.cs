using System.ComponentModel.DataAnnotations.Schema;

namespace StaffingPortalBackend.Models;
public class SalaryReviewRequest
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
    public string Justification { get; set; }
    public DateTime ReviewDate { get; set; }
    public DateTime NewSalaryStartDate { get; set; }
    public DateTime SalaryReceivedDate { get; set; }
    public string TalentManagerComments { get; set; }
    public string? DivisionManagerComments { get; set; }
    public string Status { get; set; }
    public string Currency { get; set; }
    public string ContractType { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal OldSalary { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal NewSalary { get; set; }
}