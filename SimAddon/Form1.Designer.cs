using SimAddon.Properties;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace SimAddon
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            statusStrip = new StatusStrip();
            lblConnectionStatus = new ToolStripStatusLabel();
            lblPluginStatus = new ToolStripStatusLabel();
            timerMain = new System.Windows.Forms.Timer(components);
            timerConnection = new System.Windows.Forms.Timer(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            alwaysOnTopToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            statusStrip.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { lblConnectionStatus, lblPluginStatus });
            statusStrip.Location = new Point(0, 289);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 22, 0);
            statusStrip.Size = new Size(574, 22);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 6;
            statusStrip.Text = "statusStrip1";
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(104, 17);
            lblConnectionStatus.Text = "Connection Status";
            // 
            // lblPluginStatus
            // 
            lblPluginStatus.Name = "lblPluginStatus";
            lblPluginStatus.Size = new Size(76, 17);
            lblPluginStatus.Text = "Plugin Status";
            // 
            // timerMain
            // 
            timerMain.Tick += TimerMain_Tick;
            // 
            // timerConnection
            // 
            timerConnection.Interval = 1000;
            timerConnection.Tick += TimerConnection_Tick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { alwaysOnTopToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(153, 26);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            alwaysOnTopToolStripMenuItem.Size = new Size(152, 22);
            alwaysOnTopToolStripMenuItem.Text = "Always On Top";
            alwaysOnTopToolStripMenuItem.Click += alwaysOnTopToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(574, 289);
            tabControl1.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(574, 311);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(tabControl1);
            Controls.Add(statusStrip);
            Font = new Font("Arial", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MaximumSize = new Size(590, 870);
            Name = "Form1";
            Text = "SimAddon";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmMain_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblConnectionStatus;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.Timer timerConnection;
        private ContextMenuStrip contextMenuStrip1;
        private TabControl tabControl1;
        private ToolStripStatusLabel lblPluginStatus;
        private ToolStripMenuItem alwaysOnTopToolStripMenuItem;
    }
}

