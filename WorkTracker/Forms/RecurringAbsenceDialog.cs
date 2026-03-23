namespace WorkTracker.Forms;

public partial class RecurringAbsenceDialog : Form
{
    public DayOfWeek SelectedDay => (DayOfWeek)cmbDay.SelectedIndex;
    public string Reason => txtReason.Text.Trim();

    public RecurringAbsenceDialog() { InitializeComponent(); }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Reason)) { MessageBox.Show("Enter a reason."); return; }
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}