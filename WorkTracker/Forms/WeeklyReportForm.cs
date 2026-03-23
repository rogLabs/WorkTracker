namespace WorkTracker.Forms;

public partial class WeeklyReportForm : Form
{
    private int _year = DateTime.Today.Year;
    private int _week = WorkTracker.Database.DatabaseService.GetIsoWeekNumber(DateTime.Today);

    public WeeklyReportForm() { InitializeComponent(); UpdateLabel(); LoadReport(); }

    private void UpdateLabel() => lblWeek.Text = $"Week {_week} of {_year}";

    private void LoadReport()
    {
        var data = Program.Database.GetAttendanceForWeek(_year, _week).ToList();
        grid.DataSource = data;
        if (grid.Columns.Count > 0)
        {
            if (grid.Columns["Id"] != null) grid.Columns["Id"]!.Visible = false;
            if (grid.Columns["EmployeeId"] != null) grid.Columns["EmployeeId"]!.Visible = false;
            if (grid.Columns["Date"] != null) grid.Columns["Date"]!.DefaultCellStyle.Format = "ddd dd MMM";
        }
        foreach (DataGridViewRow row in grid.Rows)
        {
            var status = row.Cells["Status"]?.Value?.ToString() ?? "";
            row.DefaultCellStyle.BackColor = status switch
            {
                "Present" => Color.FromArgb(230, 255, 230),
                "Remote"  => Color.FromArgb(230, 240, 255),
                _ when status.StartsWith("Absent") => Color.FromArgb(255, 230, 230),
                _ => Color.White
            };
        }
    }

    private void BtnPrev_Click(object? sender, EventArgs e)
    {
        _week--;
        if (_week < 1) { _year--; _week = WorkTracker.Database.DatabaseService.GetIsoWeekNumber(new DateTime(_year, 12, 28)); }
        UpdateLabel(); LoadReport();
    }

    private void BtnNext_Click(object? sender, EventArgs e)
    {
        _week++;
        int maxWeek = WorkTracker.Database.DatabaseService.GetIsoWeekNumber(new DateTime(_year, 12, 28));
        if (_week > maxWeek) { _year++; _week = 1; }
        UpdateLabel(); LoadReport();
    }
}