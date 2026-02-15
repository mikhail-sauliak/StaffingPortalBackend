namespace StaffingPortalBackend.DTO
{
    public class AggregatedSalaryDto
    {
        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string Stream { get; set; }
        public string Currency { get; set; }
        public List<SalaryPeriodDto> Salaries { get; set; } = new List<SalaryPeriodDto>();
        public bool IsDismissed { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
