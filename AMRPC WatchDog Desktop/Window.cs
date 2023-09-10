using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AMRPC_WatchDog_Desktop
{
    public partial class Window : Form
    {
        private const string StartupValue = "AMRPC WatchDog";
        private const string StartMinimizedValue = "StartMinimized";
        private readonly RegistryKey _startupRegKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private static readonly RegistryKey AppDataRegKey = Application.UserAppDataRegistry;
        private static readonly object StartsMinimized = AppDataRegKey.GetValue(StartMinimizedValue);
        private readonly bool _startsMinimizedBool = (string) StartsMinimized == "True";
        
        private NotifyIcon _notifyIcon;
        
        public Window(string appVersion)
        {
            InitializeComponent();
            SetupNotifyIcon(appVersion);
            SetupAutostartCheckbox();
            SetupStartMinimized();
        }

        private void SetupNotifyIcon(string appVersion)
        {
            _notifyIcon.Text = $"AMRPC WatchDog {appVersion}";
            _notifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem("Quit", (_, __) => Application.Exit())
            });
        }
        
        private void SetupAutostartCheckbox()
        {
            var value = _startupRegKey.GetValue(StartupValue);
            _autoStartCheckbox.Checked = value != null;
        }

        private void SetupStartMinimized()
        {
            if (StartsMinimized == null)
            {
                AppDataRegKey.SetValue(StartMinimizedValue, false);
            }
            else
            {
                _startMinimizedCheckbox.Checked = _startsMinimizedBool;
            }
        }

        private void _autoStartCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox) sender).Checked)
            {
                _startupRegKey.SetValue(StartupValue, Application.ExecutablePath);                
            }
            else
            {
                _startupRegKey.DeleteValue(StartupValue);
            }
        }
        
        private void _startMinimizedCheckbox_CheckedChanged(object sender, EventArgs e)
        {
                AppDataRegKey.SetValue(StartMinimizedValue, ((CheckBox) sender).Checked);
        }

        private void Restore(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            _notifyIcon.Visible = false;
            Show();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            if (_startsMinimizedBool)
            {
                ToTray();
            }
        }

        private void ToTray()
        {
            WindowState = FormWindowState.Minimized;
            _notifyIcon.Visible = true;
            ShowInTaskbar = false;
            Hide();
        }
    }
}
