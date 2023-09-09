using System;
using System.Windows.Forms;

namespace AMRPC_WatchDog_Desktop
{
    public partial class Window : Form
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        public Window(string appVersion)
        {
            InitializeComponent();
            
            _notifyIcon.Text = $"AMRPC WatchDog {appVersion}";
            _notifyIcon.Visible = false;
            _runningStatusLabel.Text = "Service is running\nYou might want to minimize this window to tray.";
            
            var provider = new Provider();
        }

        private void Window_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                _notifyIcon.Visible = true;
            }
        }

        private void Window_Restore(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            _notifyIcon.Visible = false;
        }
        
        private void Window_Load(object sender, EventArgs e)
        {

        }
    }
}
