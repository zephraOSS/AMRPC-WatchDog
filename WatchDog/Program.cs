using System;
using System.Windows.Forms;

namespace WatchDog
{
    internal static class Program
    {

        private const string AppVersion = "v0.4.4";

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            new Provider();

            // var window = new Window(AppVersion);
            // window.FormBorderStyle = FormBorderStyle.FixedSingle;
            Application.Run();
        }
    }
}
