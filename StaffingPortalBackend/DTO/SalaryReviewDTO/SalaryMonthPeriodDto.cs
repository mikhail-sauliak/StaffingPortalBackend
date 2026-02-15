using System;

namespace StaffingPortalBackend.DTO
{
    public class SalaryMonthPeriodDto
    {
        public int Month { get; set; }
        public decimal Salary { get; set; }
        public string Currency { get; set; }
    }
}