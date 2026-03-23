namespace WorkTracker.Forms;

partial class WeeklyReportForm
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

        var pnlTop  = new FlowLayoutPanel();
        var btnPrev = new Button();
        lblWeek     = new Label();
        var btnNext = new Button();
        grid        = new DataGridView();

        SuspendLayout();

        Text          = "Weekly Report";
        Size          = new Size(800, 500);
        StartPosition = FormStartPosition.CenterParent;

        // Top strip
        pnlTop.Dock          = DockStyle.Top;
        pnlTop.Height        = 40;
        pnlTop.Padding       = new Padding(4);
        pnlTop.FlowDirection = FlowDirection.LeftToRight;

        btnPrev.Text    = "◀ Prev";
        btnPrev.Height  = 30;
        btnPrev.AutoSize = true;
        btnPrev.Click  += BtnPrev_Click;

        lblWeek.Text      = "";
        lblWeek.Font      = new Font(Font, FontStyle.Bold);
        lblWeek.AutoSize  = false;
        lblWeek.Width     = 200;
        lblWeek.Height    = 30;
        lblWeek.TextAlign = ContentAlignment.MiddleCenter;

        btnNext.Text    = "Next ▶";
        btnNext.Height  = 30;
        btnNext.AutoSize = true;
        btnNext.Click  += BtnNext_Click;

        pnlTop.Controls.Add(btnPrev);
        pnlTop.Controls.Add(lblWeek);
        pnlTop.Controls.Add(btnNext);

        // Grid
        ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
        grid.Dock                = DockStyle.Fill;
        grid.ReadOnly            = true;
        grid.AllowUserToAddRows  = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.SelectionMode       = DataGridViewSelectionMode.FullRowSelect;
        ((System.ComponentModel.ISupportInitialize)grid).EndInit();

        Controls.Add(grid);
        Controls.Add(pnlTop);

        ResumeLayout(false);
    }

    private Label lblWeek = null!;
    private DataGridView grid = null!;
}