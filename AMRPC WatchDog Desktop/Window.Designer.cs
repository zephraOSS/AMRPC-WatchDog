using System.Drawing;
using System.Windows.Forms;

namespace AMRPC_WatchDog_Desktop
{
    partial class Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this._notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this._runningStatusLabel = new System.Windows.Forms.Label();
            this._autoStartCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _notifyIcon
            // 
            this._notifyIcon.BalloonTipText = "Running in background";
            this._notifyIcon.BalloonTipTitle = "AMRPC WatchDog";
            this._notifyIcon.Icon = ((System.Drawing.Icon) (resources.GetObject("_notifyIcon.Icon")));
            
            this._notifyIcon.Click += new System.EventHandler((_, __) =>
            {
                Show();
                WindowState = FormWindowState.Normal;
                _notifyIcon.Visible = false;
            });
            // 
            // _runningStatusLabel
            // 
            this._runningStatusLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._runningStatusLabel.Location = new System.Drawing.Point(0, 0);
            this._runningStatusLabel.Name = "_runningStatusLabel";
            this._runningStatusLabel.Size = new System.Drawing.Size(382, 153);
            this._runningStatusLabel.TabIndex = 0;
            this._runningStatusLabel.Text = "Service is running\r\nYou might want to minimize this window to tray.";
            this._runningStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _autoStartCheckbox
            // 
            this._autoStartCheckbox.Location = new System.Drawing.Point(87, 129);
            this._autoStartCheckbox.Name = "_autoStartCheckbox";
            this._autoStartCheckbox.Size = new System.Drawing.Size(200, 24);
            this._autoStartCheckbox.TabIndex = 1;
            this._autoStartCheckbox.Text = "Autostart with system";
            this._autoStartCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._autoStartCheckbox.UseVisualStyleBackColor = true;
            this._autoStartCheckbox.CheckedChanged += new System.EventHandler(this._autoStartCheckbox_CheckedChanged);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 153);
            this.Controls.Add(this._autoStartCheckbox);
            this.Controls.Add(this._runningStatusLabel);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 200);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AMRPC WatchDog";
            this.Load += new System.EventHandler(this.Window_Load);
            this.Resize += new System.EventHandler((_, __) =>
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    Hide();
                    _notifyIcon.Visible = true;
                }
            });
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.CheckBox _autoStartCheckbox;

        private System.Windows.Forms.Label _runningStatusLabel;

        #endregion
    }
}

