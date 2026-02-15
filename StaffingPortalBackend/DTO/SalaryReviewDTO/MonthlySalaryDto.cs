namespace StaffingPortalBackend.DTO
{
    public class MonthlySalaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalSalary { get; set; }
        public string Currency { get; set; }
        public bool IsDismissed { get; set; }
        public DateTime? EndDate { get; set; }
    }
}