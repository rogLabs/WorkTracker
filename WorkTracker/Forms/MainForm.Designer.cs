namespace WorkTracker.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // ── Top toolbar ──────────────────────────────────────────────────────
        var pnlToolbar        = new FlowLayoutPanel();
        var btnManageEmployees = new Button();
        var btnWeeklyReport   = new Button();

        // ── Left panel (calendar) ────────────────────────────────────────────
        var pnlLeft      = new Panel();
        var lblCalendarTitle = new Label();
        calMonth         = new MonthCalendar();

        // ── Centre panel (grid) ──────────────────────────────────────────────
        var pnlCentre = new Panel();
        grid          = new DataGridView();

        // ── Right panel (entry) ──────────────────────────────────────────────
        var pnlRight         = new Panel();
        var lblMonthCaption  = new Label();
        lblMonth             = new Label();
        var lblEmployee      = new Label();
        cmbEmployee          = new ComboBox();
        var lblDate          = new Label();
        dtpDate              = new DateTimePicker();
        var lblStatus        = new Label();
        cmbStatus            = new ComboBox();
        var lblNote          = new Label();
        txtNote              = new TextBox();
        btnSave              = new Button();
        btnDelete            = new Button();
        var lblRecurring     = new Label();
        lstRecurring         = new ListBox();
        btnAddRecurring      = new Button();
        btnRemoveRecurring   = new Button();

        SuspendLayout();

        // ── Form ─────────────────────────────────────────────────────────────
        Text            = "WorkTracker";
        Size            = new Size(1200, 700);
        MinimumSize     = new Size(900, 600);
        StartPosition   = FormStartPosition.CenterScreen;

        // ── Top toolbar ──────────────────────────────────────────────────────
        pnlToolbar.Dock          = DockStyle.Top;
        pnlToolbar.Height        = 40;
        pnlToolbar.Padding       = new Padding(4);
        pnlToolbar.FlowDirection = FlowDirection.LeftToRight;

        btnManageEmployees.Text    = "Manage Employees";
        btnManageEmployees.Height  = 30;
        btnManageEmployees.AutoSize = true;
        btnManageEmployees.Click  += (s, e) =>
        {
            using var dlg = new ManageEmployeesForm();
            dlg.ShowDialog(this);
            LoadEmployees();
            RefreshGrid();
        };

        btnWeeklyReport.Text    = "Weekly Report";
        btnWeeklyReport.Height  = 30;
        btnWeeklyReport.AutoSize = true;
        btnWeeklyReport.Click  += (s, e) =>
        {
            using var dlg = new WeeklyReportForm();
            dlg.ShowDialog(this);
        };

        pnlToolbar.Controls.Add(btnManageEmployees);
        pnlToolbar.Controls.Add(btnWeeklyReport);

        // ── Left panel (calendar) ────────────────────────────────────────────
        pnlLeft.Dock  = DockStyle.Left;
        pnlLeft.Width = 280;
        pnlLeft.Padding = new Padding(8);

        lblCalendarTitle.Text      = "Calendar";
        lblCalendarTitle.Font      = new Font(Font, FontStyle.Bold);
        lblCalendarTitle.Dock      = DockStyle.Top;
        lblCalendarTitle.Height    = 24;
        lblCalendarTitle.TextAlign = ContentAlignment.MiddleLeft;

        calMonth.MaxSelectionCount = 1;
        calMonth.Dock              = DockStyle.Fill;
        calMonth.DateChanged      += (s, e) =>
        {
            var sel = calMonth.SelectionStart;
            _viewDate = new DateTime(sel.Year, sel.Month, 1);
            if (dtpDate.Value.Month != sel.Month || dtpDate.Value.Year != sel.Year || dtpDate.Value.Day != sel.Day)
                dtpDate.Value = sel;
            RefreshGrid();
        };

        pnlLeft.Controls.Add(calMonth);
        pnlLeft.Controls.Add(lblCalendarTitle);

        // ── Right panel (entry) ──────────────────────────────────────────────
        pnlRight.Dock    = DockStyle.Right;
        pnlRight.Width   = 280;
        pnlRight.Padding = new Padding(8);

        int y = 8;
        const int lh = 22;
        const int ch = 26;
        const int gap = 4;
        const int w = 260;

        lblMonthCaption.Text     = "Month:";
        lblMonthCaption.SetBounds(8, y, w, lh);

        y += lh;
        lblMonth.Text      = "";
        lblMonth.Font      = new Font(Font, FontStyle.Bold);
        lblMonth.SetBounds(8, y, w, lh);

        y += lh + gap;
        lblEmployee.Text = "Employee:";
        lblEmployee.SetBounds(8, y, w, lh);

        y += lh;
        cmbEmployee.DropDownStyle           = ComboBoxStyle.DropDownList;
        cmbEmployee.SetBounds(8, y, w, ch);
        cmbEmployee.SelectedIndexChanged   += CmbEmployee_SelectedIndexChanged;

        y += ch + gap;
        lblDate.Text = "Date:";
        lblDate.SetBounds(8, y, w, lh);

        y += lh;
        dtpDate.Format      = DateTimePickerFormat.Short;
        dtpDate.SetBounds(8, y, w, ch);
        dtpDate.ValueChanged += DtpDate_ValueChanged;

        y += ch + gap;
        lblStatus.Text = "Status:";
        lblStatus.SetBounds(8, y, w, lh);

        y += lh;
        cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatus.Items.AddRange(new object[] { "Present", "Absent", "Remote" });
        cmbStatus.SelectedIndex = 0;
        cmbStatus.SetBounds(8, y, w, ch);

        y += ch + gap;
        lblNote.Text = "Note:";
        lblNote.SetBounds(8, y, w, lh);

        y += lh;
        txtNote.SetBounds(8, y, w, ch);

        y += ch + gap;
        btnSave.Text     = "Save";
        btnSave.SetBounds(8, y, 124, 30);
        btnSave.Click   += BtnSave_Click;

        btnDelete.Text   = "Delete Entry";
        btnDelete.SetBounds(136, y, 132, 30);
        btnDelete.Click += BtnDelete_Click;

        y += 34 + gap;
        lblRecurring.Text = "Recurring Absences:";
        lblRecurring.Font = new Font(Font, FontStyle.Bold);
        lblRecurring.SetBounds(8, y, w, lh);

        y += lh;
        lstRecurring.SetBounds(8, y, w, 100);

        y += 104 + gap;
        btnAddRecurring.Text     = "Add Recurring";
        btnAddRecurring.SetBounds(8, y, 124, 30);
        btnAddRecurring.Click   += BtnAddRecurring_Click;

        btnRemoveRecurring.Text   = "Remove Selected";
        btnRemoveRecurring.SetBounds(136, y, 132, 30);
        btnRemoveRecurring.Click += BtnRemoveRecurring_Click;

        pnlRight.Controls.AddRange(new Control[]
        {
            lblMonthCaption, lblMonth,
            lblEmployee, cmbEmployee,
            lblDate, dtpDate,
            lblStatus, cmbStatus,
            lblNote, txtNote,
            btnSave, btnDelete,
            lblRecurring, lstRecurring,
            btnAddRecurring, btnRemoveRecurring
        });

        // ── Centre panel (grid) ──────────────────────────────────────────────
        pnlCentre.Dock = DockStyle.Fill;

        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        grid.Dock                  = DockStyle.Fill;
        grid.ReadOnly              = true;
        grid.AllowUserToAddRows    = false;
        grid.AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.Fill;
        grid.SelectionMode         = DataGridViewSelectionMode.FullRowSelect;
        grid.CellClick            += Grid_CellClick;
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();

        pnlCentre.Controls.Add(grid);

        // ── Assemble form ────────────────────────────────────────────────────
        Controls.Add(pnlCentre);
        Controls.Add(pnlRight);
        Controls.Add(pnlLeft);
        Controls.Add(pnlToolbar);

        ResumeLayout(false);
    }

    // Controls referenced in MainForm.cs
    private MonthCalendar calMonth = null!;
    private Label lblMonth = null!;
    private ComboBox cmbEmployee = null!;
    private DateTimePicker dtpDate = null!;
    private ComboBox cmbStatus = null!;
    private TextBox txtNote = null!;
    private Button btnSave = null!;
    private Button btnDelete = null!;
    private ListBox lstRecurring = null!;
    private Button btnAddRecurring = null!;
    private Button btnRemoveRecurring = null!;
    private DataGridView grid = null!;
}