namespace WorkTracker.Database;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class Attendance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = "Present";
    public string? Note { get; set; }
}

public class RecurringAbsence
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DayOfWeek DayOfWeek { get; set; }
    public string Reason { get; set; } = string.Empty;
}