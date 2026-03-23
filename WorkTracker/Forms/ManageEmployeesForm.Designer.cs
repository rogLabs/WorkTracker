namespace WorkTracker.Forms;

partial class ManageEmployeesForm
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

        txtName      = new TextBox();
        btnAdd       = new Button();
        lstEmployees = new ListBox();
        btnDelete    = new Button();

        SuspendLayout();

        Text            = "Manage Employees";
        Size            = new Size(360, 400);
        StartPosition   = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;

        txtName.SetBounds(12, 12, 320, 26);
        txtName.PlaceholderText = "Employee name…";

        btnAdd.Text     = "Add Employee";
        btnAdd.SetBounds(12, 44, 320, 30);
        btnAdd.Click   += BtnAdd_Click;

        lstEmployees.SetBounds(12, 82, 320, 230);
        lstEmployees.IntegralHeight = false;

        btnDelete.Text   = "Delete Selected";
        btnDelete.SetBounds(12, 320, 320, 30);
        btnDelete.Click += BtnDelete_Click;

        Controls.AddRange(new Control[] { txtName, btnAdd, lstEmployees, btnDelete });

        ResumeLayout(false);
    }

    private TextBox txtName = null!;
    private Button btnAdd = null!;
    private ListBox lstEmployees = null!;
    private Button btnDelete = null!;
}