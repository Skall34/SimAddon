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
            toolStripHeureZulu = new ToolStripStatusLabel();
            timerMain = new System.Windows.Forms.Timer(components);
            timerConnection = new System.Windows.Forms.Timer(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            documentationToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            alwaysOnTopToolStripMenuItem = new ToolStripMenuItem();
            autoHideToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            screenshotToolStripMenuItem = new ToolStripMenuItem();
            generateFlightReportToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            loginToolStripMenuItem = new ToolStripMenuItem();
            logoutToolStripMenuItem = new ToolStripMenuItem();
            checkSessionToolStripMenuItem = new ToolStripMenuItem();
            openWebSiteToolStripMenuItem = new ToolStripMenuItem();
            traceFolderToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            networkToolStripMenuItem = new ToolStripMenuItem();
            vATSIMToolStripMenuItem = new ToolStripMenuItem();
            iVAOToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            tabControl1 = new TabControl();
            statusStrip.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { lblConnectionStatus, lblPluginStatus, toolStripHeureZulu });
            statusStrip.Location = new Point(0, 807);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 22, 0);
            statusStrip.Size = new Size(574, 24);
            statusStrip.TabIndex = 6;
            statusStrip.Text = "statusStrip1";
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(104, 19);
            lblConnectionStatus.Text = "Connection Status";
            // 
            // lblPluginStatus
            // 
            lblPluginStatus.BorderSides = ToolStripStatusLabelBorderSides.Left;
            lblPluginStatus.Name = "lblPluginStatus";
            lblPluginStatus.Size = new Size(80, 19);
            lblPluginStatus.Text = "Plugin Status";
            // 
            // toolStripHeureZulu
            // 
            toolStripHeureZulu.BorderSides = ToolStripStatusLabelBorderSides.Left;
            toolStripHeureZulu.Name = "toolStripHeureZulu";
            toolStripHeureZulu.Size = new Size(70, 19);
            toolStripHeureZulu.Text = "Heure Zulu";
            toolStripHeureZulu.TextAlign = ContentAlignment.BottomRight;
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
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { openWebSiteToolStripMenuItem, documentationToolStripMenuItem, traceFolderToolStripMenuItem, toolStripSeparator5, screenshotToolStripMenuItem, generateFlightReportToolStripMenuItem, toolStripSeparator2, loginToolStripMenuItem, logoutToolStripMenuItem, checkSessionToolStripMenuItem, toolStripSeparator3, networkToolStripMenuItem, toolStripSeparator4, alwaysOnTopToolStripMenuItem, autoHideToolStripMenuItem, toolStripSeparator1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(188, 298);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // documentationToolStripMenuItem
            // 
            documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            documentationToolStripMenuItem.Size = new Size(187, 22);
            documentationToolStripMenuItem.Text = "Documentation";
            documentationToolStripMenuItem.Click += documentationToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(184, 6);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            alwaysOnTopToolStripMenuItem.Size = new Size(187, 22);
            alwaysOnTopToolStripMenuItem.Text = "Always On Top";
            alwaysOnTopToolStripMenuItem.Click += alwaysOnTopToolStripMenuItem_Click;
            // 
            // autoHideToolStripMenuItem
            // 
            autoHideToolStripMenuItem.Name = "autoHideToolStripMenuItem";
            autoHideToolStripMenuItem.Size = new Size(187, 22);
            autoHideToolStripMenuItem.Text = "Auto hide";
            autoHideToolStripMenuItem.Click += autoHideToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(184, 6);
            // 
            // screenshotToolStripMenuItem
            // 
            screenshotToolStripMenuItem.Name = "screenshotToolStripMenuItem";
            screenshotToolStripMenuItem.Size = new Size(187, 22);
            screenshotToolStripMenuItem.Text = "Screenshot";
            screenshotToolStripMenuItem.Click += screenshotToolStripMenuItem_Click;
            // 
            // generateFlightReportToolStripMenuItem
            // 
            generateFlightReportToolStripMenuItem.Name = "generateFlightReportToolStripMenuItem";
            generateFlightReportToolStripMenuItem.Size = new Size(187, 22);
            generateFlightReportToolStripMenuItem.Text = "Generate flight report";
            generateFlightReportToolStripMenuItem.Click += generateFlightReportToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(184, 6);
            // 
            // loginToolStripMenuItem
            // 
            loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            loginToolStripMenuItem.Size = new Size(187, 22);
            loginToolStripMenuItem.Text = "Login";
            loginToolStripMenuItem.Click += loginToolStripMenuItem_Click;
            // 
            // logoutToolStripMenuItem
            // 
            logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            logoutToolStripMenuItem.Size = new Size(187, 22);
            logoutToolStripMenuItem.Text = "Logout";
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;
            // 
            // checkSessionToolStripMenuItem
            // 
            checkSessionToolStripMenuItem.Name = "checkSessionToolStripMenuItem";
            checkSessionToolStripMenuItem.Size = new Size(187, 22);
            checkSessionToolStripMenuItem.Text = "Check session";
            checkSessionToolStripMenuItem.Click += checkSessionToolStripMenuItem_Click;
            // 
            // openWebSiteToolStripMenuItem
            // 
            openWebSiteToolStripMenuItem.Name = "openWebSiteToolStripMenuItem";
            openWebSiteToolStripMenuItem.Size = new Size(187, 22);
            openWebSiteToolStripMenuItem.Text = "Open web site";
            openWebSiteToolStripMenuItem.Click += openWebSiteToolStripMenuItem_Click;
            // 
            // traceFolderToolStripMenuItem
            // 
            traceFolderToolStripMenuItem.Name = "traceFolderToolStripMenuItem";
            traceFolderToolStripMenuItem.Size = new Size(187, 22);
            traceFolderToolStripMenuItem.Text = "Open trace folder";
            traceFolderToolStripMenuItem.Click += traceFolderToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(184, 6);
            // 
            // networkToolStripMenuItem
            // 
            networkToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { vATSIMToolStripMenuItem, iVAOToolStripMenuItem });
            networkToolStripMenuItem.Name = "networkToolStripMenuItem";
            networkToolStripMenuItem.Size = new Size(187, 22);
            networkToolStripMenuItem.Text = "Network";
            // 
            // vATSIMToolStripMenuItem
            // 
            vATSIMToolStripMenuItem.CheckOnClick = true;
            vATSIMToolStripMenuItem.Name = "vATSIMToolStripMenuItem";
            vATSIMToolStripMenuItem.Size = new Size(113, 22);
            vATSIMToolStripMenuItem.Text = "VATSIM";
            vATSIMToolStripMenuItem.Click += vATSIMToolStripMenuItem_Click;
            // 
            // iVAOToolStripMenuItem
            // 
            iVAOToolStripMenuItem.CheckOnClick = true;
            iVAOToolStripMenuItem.Name = "iVAOToolStripMenuItem";
            iVAOToolStripMenuItem.Size = new Size(113, 22);
            iVAOToolStripMenuItem.Text = "IVAO";
            iVAOToolStripMenuItem.Click += iVAOToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(184, 6);
            // 
            // tabControl1
            // 
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(574, 807);
            tabControl1.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(574, 831);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(tabControl1);
            Controls.Add(statusStrip);
            Font = new Font("Arial", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(590, 870);
            Name = "Form1";
            Text = "SimAddon";
            FormClosing += FrmMain_FormClosing;
            Load += Form1_Load;
            LocationChanged += Form1_LocationChanged;
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
        private ToolStripMenuItem autoHideToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem screenshotToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem openWebSiteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripStatusLabel toolStripHeureZulu;
        private ToolStripMenuItem traceFolderToolStripMenuItem;
        private ToolStripMenuItem networkToolStripMenuItem;
        private ToolStripMenuItem vATSIMToolStripMenuItem;
        private ToolStripMenuItem iVAOToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem documentationToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem loginToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private ToolStripMenuItem checkSessionToolStripMenuItem;
        private ToolStripMenuItem generateFlightReportToolStripMenuItem;
    }
}

