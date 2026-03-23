using WorkTracker.Database;
using WorkTracker.Forms;

namespace WorkTracker;

static class Program
{
    public static DatabaseService Database { get; private set; } = null!;

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Database = new DatabaseService();
        Application.Run(new MainForm());
    }
}