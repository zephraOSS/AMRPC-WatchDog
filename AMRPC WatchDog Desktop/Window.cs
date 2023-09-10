using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AMRPC_WatchDog_Desktop
{
    public partial class Window : Form
    {
        private const string StartupKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string StartupValue = "AMRPC WatchDog";
        private readonly RegistryKey _regKey = Registry.CurrentUser.OpenSubKey(StartupKey, true);
        private NotifyIcon _notifyIcon;
        public Window(string appVersion)
        {
            InitializeComponent();
            SetupNotifyIcon(appVersion);
            SetupAutostartCheckbox();

            var provider = new Provider();
        }

        private void SetupNotifyIcon(string appVersion)
        {
            _notifyIcon.Text = $"AMRPC WatchDog {appVersion}";
            _notifyIcon.Visible = false;
        }
        
        private void SetupAutostartCheckbox()
        {
            var value = _regKey.GetValue(StartupValue);
            _autoStartCheckbox.Checked = value != null;
        }

        private void _autoStartCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox) sender).Checked)
            {
                _regKey.SetValue(StartupValue, Application.ExecutablePath);                
            }
            else
            {
                _regKey.DeleteValue(StartupValue);
            }
        }
        
        private void Window_Load(object sender, EventArgs e) { }
    }
}
