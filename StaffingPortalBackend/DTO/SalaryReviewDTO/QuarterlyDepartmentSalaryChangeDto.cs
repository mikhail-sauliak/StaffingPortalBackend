namespace StaffingPortalBackend.DTO
{
    public class QuarterlyDepartmentSalaryChangeDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public decimal Q1 { get; set; }
        public decimal Q2 { get; set; }
        public decimal Q3 { get; set; }
        public decimal Q4 { get; set; }
        public decimal Total { get; set; }

        // if will be needed in the future
        public int DepartmentId { get; set; }
    }
}