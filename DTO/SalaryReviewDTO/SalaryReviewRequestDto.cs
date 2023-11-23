namespace StaffingPortalBackend.DTO
{
    public class SalaryReviewRequestDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Justification { get; set; }
        public DateTime NewSalaryStartDate { get; set; }
        public DateTime SalaryReceivedDate { get; set; }
        public string TalentManagerComments { get; set; }
         public string? DivisionManagerComments { get; set; }
        public string ContractType { get; set; }
        public string Currency { get; set; }
        public decimal OldSalary { get; set; }
        public decimal NewSalary { get; set; } 
        public DateTime ReviewDate { get; set; }
    }
}
