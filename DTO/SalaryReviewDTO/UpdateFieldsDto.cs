namespace StaffingPortalBackend.DTO
{
    public class UpdateFieldsDto
    {
        public decimal ProposedSalary { get; set; }
        public string Currency { get; set; }
        public string ContractType { get; set; }
        public DateTimeOffset? NewSalaryStartDate { get; set; }
        public DateTimeOffset? SalaryReceivedDate { get; set; }
        public string DivisionManagerComments { get; set; }
        public bool? IsApproved { get; set; }
        public bool? ApprovedByDM { get; set; }
    }
}
