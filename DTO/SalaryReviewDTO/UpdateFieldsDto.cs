namespace StaffingPortalBackend.DTO
{
    public class UpdateFieldsDto
    {
        public decimal ProposedSalary { get; set; }
        public string Currency { get; set; }
        public string ContractType { get; set; }
        public DateTime? NewSalaryStartDate { get; set; }
        public DateTime? SalaryReceivedDate { get; set; }
        public string DivisionManagerComments { get; set; }
        public bool? IsApproved { get; set; }
        public bool? ApprovedByDM { get; set; }
    }
}
