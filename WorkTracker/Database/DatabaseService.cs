using Dapper;
using Microsoft.Data.Sqlite;

namespace WorkTracker.Database;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string dbPath = "worktracker.db")
    {
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

    private SqliteConnection CreateConnection() => new SqliteConnection(_connectionString);

    private void InitializeDatabase()
    {
        using var conn = CreateConnection();
        conn.Open();

        conn.Execute("""
            CREATE TABLE IF NOT EXISTS Employees (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );
        """);

        conn.Execute("""
            CREATE TABLE IF NOT EXISTS Attendance (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EmployeeId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                Status TEXT NOT NULL DEFAULT 'Present',
                Note TEXT,
                FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
                UNIQUE(EmployeeId, Date)
            );
        """);

        conn.Execute("""
            CREATE TABLE IF NOT EXISTS RecurringAbsences (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EmployeeId INTEGER NOT NULL,
                DayOfWeek INTEGER NOT NULL,
                Reason TEXT NOT NULL,
                FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
                UNIQUE(EmployeeId, DayOfWeek)
            );
        """);
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        using var conn = CreateConnection();
        return conn.Query<Employee>("SELECT * FROM Employees ORDER BY Name");
    }

    public void AddEmployee(string name)
    {
        using var conn = CreateConnection();
        conn.Execute("INSERT OR IGNORE INTO Employees (Name) VALUES (@Name)", new { Name = name.Trim() });
    }

    public void DeleteEmployee(int employeeId)
    {
        using var conn = CreateConnection();
        conn.Execute("DELETE FROM Attendance WHERE EmployeeId = @Id", new { Id = employeeId });
        conn.Execute("DELETE FROM RecurringAbsences WHERE EmployeeId = @Id", new { Id = employeeId });
        conn.Execute("DELETE FROM Employees WHERE Id = @Id", new { Id = employeeId });
    }

    public void UpsertAttendance(int employeeId, DateTime date, string status, string? note)
    {
        using var conn = CreateConnection();
        conn.Execute("""
            INSERT INTO Attendance (EmployeeId, Date, Status, Note)
            VALUES (@EmployeeId, @Date, @Status, @Note)
            ON CONFLICT(EmployeeId, Date) DO UPDATE SET Status = @Status, Note = @Note
        """, new { EmployeeId = employeeId, Date = date.ToString("yyyy-MM-dd"), Status = status, Note = note });
    }

    public void DeleteAttendance(int employeeId, DateTime date)
    {
        using var conn = CreateConnection();
        conn.Execute("DELETE FROM Attendance WHERE EmployeeId = @EmployeeId AND Date = @Date",
            new { EmployeeId = employeeId, Date = date.ToString("yyyy-MM-dd") });
    }

    public IEnumerable<Attendance> GetAttendanceForMonth(int year, int month)
    {
        using var conn = CreateConnection();
        var from = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
        var to   = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("yyyy-MM-dd");
        return conn.Query<Attendance>("""
            SELECT a.Id, a.EmployeeId, e.Name AS EmployeeName,
                   DATE(a.Date) AS Date, a.Status, a.Note
            FROM Attendance a
            JOIN Employees e ON e.Id = a.EmployeeId
            WHERE a.Date BETWEEN @From AND @To
            ORDER BY a.Date, e.Name
        """, new { From = from, To = to });
    }

    public IEnumerable<Attendance> GetAttendanceForEmployee(int employeeId, DateTime from, DateTime to)
    {
        using var conn = CreateConnection();
        return conn.Query<Attendance>("""
            SELECT a.Id, a.EmployeeId, e.Name AS EmployeeName,
                   DATE(a.Date) AS Date, a.Status, a.Note
            FROM Attendance a
            JOIN Employees e ON e.Id = a.EmployeeId
            WHERE a.EmployeeId = @EmployeeId
              AND a.Date BETWEEN @From AND @To
            ORDER BY a.Date
        """, new { EmployeeId = employeeId, From = from.ToString("yyyy-MM-dd"), To = to.ToString("yyyy-MM-dd") });
    }

    public IEnumerable<Attendance> GetAttendanceForWeek(int year, int weekNumber)
    {
        var monday = GetMondayOfWeek(year, weekNumber);
        var sunday = monday.AddDays(6);
        using var conn = CreateConnection();
        return conn.Query<Attendance>("""
            SELECT a.Id, a.EmployeeId, e.Name AS EmployeeName,
                   DATE(a.Date) AS Date, a.Status, a.Note
            FROM Attendance a
            JOIN Employees e ON e.Id = a.EmployeeId
            WHERE a.Date BETWEEN @From AND @To
            ORDER BY a.Date, e.Name
        """, new { From = monday.ToString("yyyy-MM-dd"), To = sunday.ToString("yyyy-MM-dd") });
    }

    public IEnumerable<RecurringAbsence> GetRecurringAbsences()
    {
        using var conn = CreateConnection();
        return conn.Query<RecurringAbsence>("""
            SELECT r.Id, r.EmployeeId, e.Name AS EmployeeName, r.DayOfWeek, r.Reason
            FROM RecurringAbsences r
            JOIN Employees e ON e.Id = r.EmployeeId
            ORDER BY e.Name, r.DayOfWeek
        """);
    }

    public IEnumerable<RecurringAbsence> GetRecurringAbsencesForEmployee(int employeeId)
    {
        using var conn = CreateConnection();
        return conn.Query<RecurringAbsence>("""
            SELECT r.Id, r.EmployeeId, e.Name AS EmployeeName, r.DayOfWeek, r.Reason
            FROM RecurringAbsences r
            JOIN Employees e ON e.Id = r.EmployeeId
            WHERE r.EmployeeId = @EmployeeId
        """, new { EmployeeId = employeeId });
    }

    public void AddRecurringAbsence(int employeeId, DayOfWeek day, string reason)
    {
        using var conn = CreateConnection();
        conn.Execute("""
            INSERT OR REPLACE INTO RecurringAbsences (EmployeeId, DayOfWeek, Reason)
            VALUES (@EmployeeId, @DayOfWeek, @Reason)
        """, new { EmployeeId = employeeId, DayOfWeek = (int)day, Reason = reason });
    }

    public void DeleteRecurringAbsence(int id)
    {
        using var conn = CreateConnection();
        conn.Execute("DELETE FROM RecurringAbsences WHERE Id = @Id", new { Id = id });
    }

    public static int GetIsoWeekNumber(DateTime date)
    {
        var cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
        return cal.GetWeekOfYear(date,
            System.Globalization.CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday);
    }

    public static DateTime GetMondayOfWeek(int year, int weekNumber)
    {
        var jan4 = new DateTime(year, 1, 4);
        int dow = (int)jan4.DayOfWeek;
        dow = dow == 0 ? 7 : dow;
        var week1Monday = jan4.AddDays(-(dow - 1));
        return week1Monday.AddDays((weekNumber - 1) * 7);
    }
}