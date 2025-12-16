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
            tabControl1 = new TabControl();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            loginToolStripMenuItem1 = new ToolStripMenuItem();
            logoutToolStripMenuItem1 = new ToolStripMenuItem();
            checkSessionToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            screenshotToolStripMenuItem1 = new ToolStripMenuItem();
            generateFlightReportToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            settingsToolStripMenuItem1 = new ToolStripMenuItem();
            tracesFolderToolStripMenuItem = new ToolStripMenuItem();
            networkToolStripMenuItem1 = new ToolStripMenuItem();
            vATSIMToolStripMenuItem1 = new ToolStripMenuItem();
            iVAOToolStripMenuItem1 = new ToolStripMenuItem();
            linksToolStripMenuItem = new ToolStripMenuItem();
            openWebSiteToolStripMenuItem1 = new ToolStripMenuItem();
            documentationToolStripMenuItem1 = new ToolStripMenuItem();
            statusStrip.SuspendLayout();
            menuStrip1.SuspendLayout();
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
            // tabControl1
            // 
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 24);
            tabControl1.Margin = new Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(574, 783);
            tabControl1.TabIndex = 7;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, networkToolStripMenuItem1, linksToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(574, 24);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginToolStripMenuItem1, logoutToolStripMenuItem1, checkSessionToolStripMenuItem1, toolStripSeparator6, screenshotToolStripMenuItem1, generateFlightReportToolStripMenuItem1, toolStripSeparator7, settingsToolStripMenuItem1, tracesFolderToolStripMenuItem });
            fileToolStripMenuItem.ForeColor = Color.Black;
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F;
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loginToolStripMenuItem1
            // 
            loginToolStripMenuItem1.Name = "loginToolStripMenuItem1";
            loginToolStripMenuItem1.Size = new Size(187, 22);
            loginToolStripMenuItem1.Text = "Login";
            loginToolStripMenuItem1.Click += loginToolStripMenuItem1_Click;
            // 
            // logoutToolStripMenuItem1
            // 
            logoutToolStripMenuItem1.Name = "logoutToolStripMenuItem1";
            logoutToolStripMenuItem1.Size = new Size(187, 22);
            logoutToolStripMenuItem1.Text = "Logout";
            logoutToolStripMenuItem1.Click += logoutToolStripMenuItem1_Click;
            // 
            // checkSessionToolStripMenuItem1
            // 
            checkSessionToolStripMenuItem1.Name = "checkSessionToolStripMenuItem1";
            checkSessionToolStripMenuItem1.Size = new Size(187, 22);
            checkSessionToolStripMenuItem1.Text = "Check Session";
            checkSessionToolStripMenuItem1.Click += checkSessionToolStripMenuItem1_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(184, 6);
            // 
            // screenshotToolStripMenuItem1
            // 
            screenshotToolStripMenuItem1.Name = "screenshotToolStripMenuItem1";
            screenshotToolStripMenuItem1.ShortcutKeys = Keys.F12;
            screenshotToolStripMenuItem1.Size = new Size(187, 22);
            screenshotToolStripMenuItem1.Text = "Screenshot";
            screenshotToolStripMenuItem1.Click += screenshotToolStripMenuItem1_Click;
            // 
            // generateFlightReportToolStripMenuItem1
            // 
            generateFlightReportToolStripMenuItem1.Name = "generateFlightReportToolStripMenuItem1";
            generateFlightReportToolStripMenuItem1.Size = new Size(187, 22);
            generateFlightReportToolStripMenuItem1.Text = "Generate flight report";
            generateFlightReportToolStripMenuItem1.Click += generateFlightReportToolStripMenuItem1_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(184, 6);
            // 
            // settingsToolStripMenuItem1
            // 
            settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            settingsToolStripMenuItem1.Size = new Size(187, 22);
            settingsToolStripMenuItem1.Text = "Settings";
            settingsToolStripMenuItem1.Click += settingsToolStripMenuItem1_Click;
            // 
            // tracesFolderToolStripMenuItem
            // 
            tracesFolderToolStripMenuItem.Name = "tracesFolderToolStripMenuItem";
            tracesFolderToolStripMenuItem.Size = new Size(187, 22);
            tracesFolderToolStripMenuItem.Text = "Open traces folder";
            tracesFolderToolStripMenuItem.Click += tracesFolderToolStripMenuItem_Click;
            // 
            // networkToolStripMenuItem1
            // 
            networkToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { vATSIMToolStripMenuItem1, iVAOToolStripMenuItem1 });
            networkToolStripMenuItem1.ForeColor = Color.Black;
            networkToolStripMenuItem1.Name = "networkToolStripMenuItem1";
            networkToolStripMenuItem1.ShortcutKeys = Keys.Alt | Keys.N;
            networkToolStripMenuItem1.Size = new Size(64, 20);
            networkToolStripMenuItem1.Text = "Network";
            // 
            // vATSIMToolStripMenuItem1
            // 
            vATSIMToolStripMenuItem1.Checked = true;
            vATSIMToolStripMenuItem1.CheckOnClick = true;
            vATSIMToolStripMenuItem1.CheckState = CheckState.Checked;
            vATSIMToolStripMenuItem1.Name = "vATSIMToolStripMenuItem1";
            vATSIMToolStripMenuItem1.Size = new Size(113, 22);
            vATSIMToolStripMenuItem1.Text = "VATSIM";
            vATSIMToolStripMenuItem1.Click += vATSIMToolStripMenuItem1_Click;
            // 
            // iVAOToolStripMenuItem1
            // 
            iVAOToolStripMenuItem1.CheckOnClick = true;
            iVAOToolStripMenuItem1.Name = "iVAOToolStripMenuItem1";
            iVAOToolStripMenuItem1.Size = new Size(113, 22);
            iVAOToolStripMenuItem1.Text = "IVAO";
            iVAOToolStripMenuItem1.Click += iVAOToolStripMenuItem1_Click;
            // 
            // linksToolStripMenuItem
            // 
            linksToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openWebSiteToolStripMenuItem1, documentationToolStripMenuItem1 });
            linksToolStripMenuItem.ForeColor = Color.Black;
            linksToolStripMenuItem.Name = "linksToolStripMenuItem";
            linksToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.L;
            linksToolStripMenuItem.Size = new Size(46, 20);
            linksToolStripMenuItem.Text = "Links";
            // 
            // openWebSiteToolStripMenuItem1
            // 
            openWebSiteToolStripMenuItem1.Name = "openWebSiteToolStripMenuItem1";
            openWebSiteToolStripMenuItem1.Size = new Size(157, 22);
            openWebSiteToolStripMenuItem1.Text = "Open web site";
            openWebSiteToolStripMenuItem1.Click += openWebSiteToolStripMenuItem1_Click;
            // 
            // documentationToolStripMenuItem1
            // 
            documentationToolStripMenuItem1.Name = "documentationToolStripMenuItem1";
            documentationToolStripMenuItem1.Size = new Size(157, 22);
            documentationToolStripMenuItem1.Text = "Documentation";
            documentationToolStripMenuItem1.Click += documentationToolStripMenuItem1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Silver;
            ClientSize = new Size(574, 831);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip1);
            Font = new Font("Arial", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
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
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblConnectionStatus;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.Timer timerConnection;
        private TabControl tabControl1;
        private ToolStripStatusLabel lblPluginStatus;
        private ToolStripStatusLabel toolStripHeureZulu;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loginToolStripMenuItem1;
        private ToolStripMenuItem logoutToolStripMenuItem1;
        private ToolStripMenuItem checkSessionToolStripMenuItem1;
        private ToolStripMenuItem networkToolStripMenuItem1;
        private ToolStripMenuItem linksToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem screenshotToolStripMenuItem1;
        private ToolStripMenuItem generateFlightReportToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem settingsToolStripMenuItem1;
        private ToolStripMenuItem tracesFolderToolStripMenuItem;
        private ToolStripMenuItem vATSIMToolStripMenuItem1;
        private ToolStripMenuItem iVAOToolStripMenuItem1;
        private ToolStripMenuItem openWebSiteToolStripMenuItem1;
        private ToolStripMenuItem documentationToolStripMenuItem1;
    }
}

