namespace WorkTracker.Forms;

public partial class ManageEmployeesForm : Form
{
    public ManageEmployeesForm() { InitializeComponent(); LoadEmployees(); }

    private void LoadEmployees()
    {
        lstEmployees.DataSource = null;
        lstEmployees.DataSource = Program.Database.GetAllEmployees().ToList();
        lstEmployees.DisplayMember = "Name";
        lstEmployees.ValueMember = "Id";
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        var name = txtName.Text.Trim();
        if (string.IsNullOrEmpty(name)) return;
        Program.Database.AddEmployee(name);
        txtName.Clear();
        LoadEmployees();
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (lstEmployees.SelectedItem is not WorkTracker.Database.Employee emp) return;
        if (MessageBox.Show($"Delete {emp.Name} and all their records?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        {
            Program.Database.DeleteEmployee(emp.Id);
            LoadEmployees();
        }
    }
}