using System;

namespace StaffingPortalBackend.DTO
{
    public class MonthlySalaryAggregationDto
    {
        public int PersonId { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public string FullName { get; set; }
        public List<SalaryMonthPeriodDto> Salaries { get; set; }
        public bool IsDismissed { get; set; } 
        public DateTime? EndDate { get; set; }
        public string Currency { get; set; }
    }
}