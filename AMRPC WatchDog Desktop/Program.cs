using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AMRPC_WatchDog_Desktop
{
    internal static class Program
    {

        private const string AppVersion = "v0.2.1"; 
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            new Provider();
            
            var window = new Window(AppVersion);
            window.FormBorderStyle = FormBorderStyle.FixedSingle;
            Application.Run(window);
        }
    }
}
