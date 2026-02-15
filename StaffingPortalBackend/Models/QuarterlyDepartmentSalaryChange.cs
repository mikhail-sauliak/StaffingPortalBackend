namespace StaffingPortalBackend.Models;
public class QuarterlyDepartmentSalaryChange
{
    public int Id { get; set; }
    public int Year { get; set; }
    public decimal Q1 { get; set; }
    public decimal Q2 { get; set; }
    public decimal Q3 { get; set; }
    public decimal Q4 { get; set; }
    public decimal Total { get; set; }    
    public int DepartmentId { get; set; } //may be needed in the future to calculate separate sum for SDET and QA streams
}