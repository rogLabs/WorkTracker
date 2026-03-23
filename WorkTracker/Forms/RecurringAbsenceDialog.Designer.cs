namespace WorkTracker.Forms;

partial class RecurringAbsenceDialog
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

        var lblDay  = new Label();
        cmbDay      = new ComboBox();
        var lblReason = new Label();
        txtReason   = new TextBox();
        btnOk       = new Button();
        btnCancel   = new Button();

        SuspendLayout();

        Text            = "Add Recurring Absence";
        Size            = new Size(320, 200);
        StartPosition   = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;

        lblDay.Text = "Day of Week:";
        lblDay.SetBounds(12, 16, 280, 22);

        cmbDay.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDay.Items.AddRange(new object[]
        {
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        });
        cmbDay.SelectedIndex = 1; // Monday
        cmbDay.SetBounds(12, 38, 280, 26);

        lblReason.Text = "Reason:";
        lblReason.SetBounds(12, 72, 280, 22);

        txtReason.SetBounds(12, 94, 280, 26);

        btnOk.Text     = "OK";
        btnOk.SetBounds(12, 132, 130, 30);
        btnOk.Click   += BtnOk_Click;

        btnCancel.Text   = "Cancel";
        btnCancel.SetBounds(150, 132, 142, 30);
        btnCancel.Click += BtnCancel_Click;

        Controls.AddRange(new Control[] { lblDay, cmbDay, lblReason, txtReason, btnOk, btnCancel });

        ResumeLayout(false);
    }

    private ComboBox cmbDay = null!;
    private TextBox txtReason = null!;
    private Button btnOk = null!;
    private Button btnCancel = null!;
}
