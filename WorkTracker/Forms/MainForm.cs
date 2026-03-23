using WorkTracker.Database;

namespace WorkTracker.Forms;

public partial class MainForm : Form
{
    private DateTime _viewDate = DateTime.Today;

    public MainForm()
    {
        InitializeComponent();
        LoadEmployees();
        RefreshGrid();
    }

    private void LoadEmployees()
    {
        var selected = cmbEmployee.SelectedItem as Employee;
        cmbEmployee.DataSource = null;
        var employees = Program.Database.GetAllEmployees().ToList();
        cmbEmployee.DataSource = employees;
        cmbEmployee.DisplayMember = "Name";
        cmbEmployee.ValueMember = "Id";

        if (selected != null)
        {
            var match = employees.FirstOrDefault(e => e.Id == selected.Id);
            if (match != null) cmbEmployee.SelectedItem = match;
        }
        else if (employees.Count > 0)
        {
            cmbEmployee.SelectedIndex = 0;
        }

        LoadRecurringAbsences();
    }

    private void LoadRecurringAbsences()
    {
        lstRecurring.Items.Clear();
        if (cmbEmployee.SelectedItem is not Employee emp) return;
        var recurring = Program.Database.GetRecurringAbsencesForEmployee(emp.Id).ToList();
        foreach (var r in recurring)
            lstRecurring.Items.Add(new RecurringItem(r));
    }

    private void RefreshGrid()
    {
        lblMonth.Text = _viewDate.ToString("MMMM yyyy");
        var attendance = Program.Database.GetAttendanceForMonth(_viewDate.Year, _viewDate.Month).ToList();
        var recurringAll = Program.Database.GetRecurringAbsences().ToList();
        var employees = Program.Database.GetAllEmployees().ToList();

        var rows = new List<GridRow>();
        int daysInMonth = DateTime.DaysInMonth(_viewDate.Year, _viewDate.Month);

        foreach (var emp in employees)
        {
            var recurring = recurringAll.Where(r => r.EmployeeId == emp.Id).ToList();
            for (int d = 1; d <= daysInMonth; d++)
            {
                var date = new DateTime(_viewDate.Year, _viewDate.Month, d);
                var att = attendance.FirstOrDefault(a => a.EmployeeId == emp.Id && a.Date.Date == date.Date);

                string status;
                string note = att?.Note ?? "";

                if (att != null)
                {
                    status = att.Status;
                }
                else
                {
                    var rec = recurring.FirstOrDefault(r => r.DayOfWeek == date.DayOfWeek);
                    status = rec != null ? $"Absent (recurring: {rec.Reason})" : "Present";
                }

                rows.Add(new GridRow { Date = date, EmployeeName = emp.Name, Status = status, Note = note, EmployeeId = emp.Id });
            }
        }

        grid.DataSource = rows;

        if (grid.Columns.Count > 0)
        {
            grid.Columns["Date"]!.DefaultCellStyle.Format = "ddd dd MMM";
            grid.Columns["EmployeeId"]!.Visible = false;
        }

        foreach (DataGridViewRow row in grid.Rows)
        {
            var status = row.Cells["Status"].Value?.ToString() ?? "";
            row.DefaultCellStyle.BackColor = status switch
            {
                "Present" => Color.FromArgb(230, 255, 230),
                "Remote"  => Color.FromArgb(230, 240, 255),
                _ when status.StartsWith("Absent") => Color.FromArgb(255, 230, 230),
                _ => Color.White
            };
        }
    }

    private void CmbEmployee_SelectedIndexChanged(object? sender, EventArgs e)
    {
        LoadRecurringAbsences();
        LoadFormForSelectedDateAndEmployee();
    }

    private void DtpDate_ValueChanged(object? sender, EventArgs e) => LoadFormForSelectedDateAndEmployee();

    private void LoadFormForSelectedDateAndEmployee()
    {
        if (cmbEmployee.SelectedItem is not Employee emp) return;
        var att = Program.Database.GetAttendanceForEmployee(emp.Id, dtpDate.Value.Date, dtpDate.Value.Date).FirstOrDefault();
        if (att != null)
        {
            cmbStatus.SelectedItem = att.Status;
            txtNote.Text = att.Note ?? "";
        }
        else
        {
            cmbStatus.SelectedIndex = 0;
            txtNote.Clear();
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (cmbEmployee.SelectedItem is not Employee emp) { MessageBox.Show("Select an employee first."); return; }
        Program.Database.UpsertAttendance(emp.Id, dtpDate.Value.Date, cmbStatus.SelectedItem!.ToString()!, txtNote.Text.Trim() == "" ? null : txtNote.Text.Trim());
        RefreshGrid();
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (cmbEmployee.SelectedItem is not Employee emp) return;
        Program.Database.DeleteAttendance(emp.Id, dtpDate.Value.Date);
        LoadFormForSelectedDateAndEmployee();
        RefreshGrid();
    }

    private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = (GridRow)grid.Rows[e.RowIndex].DataBoundItem;
        dtpDate.Value = row.Date;
        var emp = Program.Database.GetAllEmployees().FirstOrDefault(x => x.Id == row.EmployeeId);
        if (emp != null) cmbEmployee.SelectedItem = emp;
    }

    private void BtnAddRecurring_Click(object? sender, EventArgs e)
    {
        if (cmbEmployee.SelectedItem is not Employee emp) return;
        using var dlg = new RecurringAbsenceDialog();
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            Program.Database.AddRecurringAbsence(emp.Id, dlg.SelectedDay, dlg.Reason);
            LoadRecurringAbsences();
            RefreshGrid();
        }
    }

    private void BtnRemoveRecurring_Click(object? sender, EventArgs e)
    {
        if (lstRecurring.SelectedItem is not RecurringItem item) return;
        Program.Database.DeleteRecurringAbsence(item.Absence.Id);
        LoadRecurringAbsences();
        RefreshGrid();
    }

    private class GridRow
    {
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; } = "";
        public string Status { get; set; } = "";
        public string Note { get; set; } = "";
        public int EmployeeId { get; set; }
    }

    private class RecurringItem(RecurringAbsence absence)
    {
        public RecurringAbsence Absence { get; } = absence;
        public override string ToString() => $"{Absence.DayOfWeek} - {Absence.Reason}";
    }
}