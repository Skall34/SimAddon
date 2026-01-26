using SimAddon.Properties;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Drawing;
using SimAddonControls;

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
            statusStrip = new VSStatusStrip();
            lblConnectionStatus = new ToolStripStatusLabel();
            lblPluginStatus = new ToolStripStatusLabel();
            toolStripHeureZulu = new ToolStripStatusLabel();
            timerMain = new System.Windows.Forms.Timer(components);
            timerConnection = new System.Windows.Forms.Timer(components);
            tabControl1 = new VSTabControl();
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
            toolStripSeparator1 = new ToolStripSeparator();
            quitToolStripMenuItem = new ToolStripMenuItem();
            networkToolStripMenuItem1 = new ToolStripMenuItem();
            vATSIMToolStripMenuItem1 = new ToolStripMenuItem();
            iVAOToolStripMenuItem1 = new ToolStripMenuItem();
            linksToolStripMenuItem = new ToolStripMenuItem();
            openWebSiteToolStripMenuItem1 = new ToolStripMenuItem();
            documentationToolStripMenuItem1 = new ToolStripMenuItem();
            skyboundsAIToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutSimAddonToolStripMenuItem = new ToolStripMenuItem();
            btnClose = new ToolStripButton();
            btnMaximize = new ToolStripButton();
            btnMinimize = new ToolStripButton();
            ledConnectionStatus = new ToolStripControlHost(new SimAddonControls.LedBulb());
            statusStrip.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.BackColor = Color.FromArgb(0, 122, 204);
            statusStrip.BorderColor = Color.FromArgb(0, 100, 180);
            statusStrip.ForeColor = Color.White;
            statusStrip.GripColor = Color.FromArgb(70, 70, 74);
            statusStrip.GripStyle = ToolStripGripStyle.Visible;
            statusStrip.Items.AddRange(new ToolStripItem[] { lblConnectionStatus, lblPluginStatus, toolStripHeureZulu });
            statusStrip.Location = new Point(0, 846);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 22, 0);
            statusStrip.Size = new Size(590, 24);
            statusStrip.TabIndex = 6;
            statusStrip.Text = "statusStrip1";
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.BackColor = Color.Transparent;
            lblConnectionStatus.ForeColor = Color.White;
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(104, 19);
            lblConnectionStatus.Text = "Connection Status";
            // 
            // lblPluginStatus
            // 
            lblPluginStatus.BackColor = Color.Transparent;
            lblPluginStatus.BorderSides = ToolStripStatusLabelBorderSides.Left;
            lblPluginStatus.ForeColor = Color.White;
            lblPluginStatus.Name = "lblPluginStatus";
            lblPluginStatus.Size = new Size(80, 19);
            lblPluginStatus.Text = "Plugin Status";
            // 
            // toolStripHeureZulu
            // 
            toolStripHeureZulu.BackColor = Color.Transparent;
            toolStripHeureZulu.BorderSides = ToolStripStatusLabelBorderSides.Left;
            toolStripHeureZulu.ForeColor = Color.White;
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
            tabControl1.AccentColor = Color.FromArgb(0, 122, 204);
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tabControl1.ItemSize = new Size(80, 25);
            tabControl1.Location = new Point(0, 26);
            tabControl1.Margin = new Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new Point(0, 0);
            tabControl1.Size = new Size(591, 820);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.TabBackColor = Color.FromArgb(45, 45, 48);
            tabControl1.TabHoverBackColor = Color.DarkGray;
            tabControl1.TabIndex = 7;
            tabControl1.TabPageBackColor = Color.FromArgb(37, 37, 38);
            tabControl1.TabSelectedBackColor = Color.MidnightBlue;
            tabControl1.TabSelectedTextColor = Color.White;
            tabControl1.TabTextColor = Color.White;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.DimGray;
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, networkToolStripMenuItem1, linksToolStripMenuItem, helpToolStripMenuItem, btnClose, btnMaximize, btnMinimize, ledConnectionStatus });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(0);
            menuStrip1.Size = new Size(590, 26);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginToolStripMenuItem1, logoutToolStripMenuItem1, checkSessionToolStripMenuItem1, toolStripSeparator6, screenshotToolStripMenuItem1, generateFlightReportToolStripMenuItem1, toolStripSeparator7, settingsToolStripMenuItem1, tracesFolderToolStripMenuItem, toolStripSeparator1, quitToolStripMenuItem });
            fileToolStripMenuItem.ForeColor = Color.Black;
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F;
            fileToolStripMenuItem.Size = new Size(37, 26);
            fileToolStripMenuItem.Text = "File";
            // 
            // loginToolStripMenuItem1
            // 
            loginToolStripMenuItem1.BackColor = Color.DimGray;
            loginToolStripMenuItem1.ForeColor = Color.White;
            loginToolStripMenuItem1.Name = "loginToolStripMenuItem1";
            loginToolStripMenuItem1.Size = new Size(187, 22);
            loginToolStripMenuItem1.Text = "Login";
            loginToolStripMenuItem1.Click += loginToolStripMenuItem1_Click;
            // 
            // logoutToolStripMenuItem1
            // 
            logoutToolStripMenuItem1.BackColor = Color.DimGray;
            logoutToolStripMenuItem1.ForeColor = Color.White;
            logoutToolStripMenuItem1.Name = "logoutToolStripMenuItem1";
            logoutToolStripMenuItem1.Size = new Size(187, 22);
            logoutToolStripMenuItem1.Text = "Logout";
            logoutToolStripMenuItem1.Click += logoutToolStripMenuItem1_Click;
            // 
            // checkSessionToolStripMenuItem1
            // 
            checkSessionToolStripMenuItem1.BackColor = Color.DimGray;
            checkSessionToolStripMenuItem1.ForeColor = Color.White;
            checkSessionToolStripMenuItem1.Name = "checkSessionToolStripMenuItem1";
            checkSessionToolStripMenuItem1.Size = new Size(187, 22);
            checkSessionToolStripMenuItem1.Text = "Check Session";
            checkSessionToolStripMenuItem1.Click += checkSessionToolStripMenuItem1_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.BackColor = Color.DimGray;
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(184, 6);
            // 
            // screenshotToolStripMenuItem1
            // 
            screenshotToolStripMenuItem1.BackColor = Color.DimGray;
            screenshotToolStripMenuItem1.ForeColor = Color.White;
            screenshotToolStripMenuItem1.Name = "screenshotToolStripMenuItem1";
            screenshotToolStripMenuItem1.ShortcutKeys = Keys.F12;
            screenshotToolStripMenuItem1.Size = new Size(187, 22);
            screenshotToolStripMenuItem1.Text = "Screenshot";
            screenshotToolStripMenuItem1.Click += screenshotToolStripMenuItem1_Click;
            // 
            // generateFlightReportToolStripMenuItem1
            // 
            generateFlightReportToolStripMenuItem1.BackColor = Color.DimGray;
            generateFlightReportToolStripMenuItem1.ForeColor = Color.White;
            generateFlightReportToolStripMenuItem1.Name = "generateFlightReportToolStripMenuItem1";
            generateFlightReportToolStripMenuItem1.Size = new Size(187, 22);
            generateFlightReportToolStripMenuItem1.Text = "Generate flight report";
            generateFlightReportToolStripMenuItem1.Click += generateFlightReportToolStripMenuItem1_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.BackColor = Color.DimGray;
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(184, 6);
            // 
            // settingsToolStripMenuItem1
            // 
            settingsToolStripMenuItem1.BackColor = Color.DimGray;
            settingsToolStripMenuItem1.ForeColor = Color.White;
            settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            settingsToolStripMenuItem1.Size = new Size(187, 22);
            settingsToolStripMenuItem1.Text = "Settings";
            settingsToolStripMenuItem1.Click += settingsToolStripMenuItem1_Click;
            // 
            // tracesFolderToolStripMenuItem
            // 
            tracesFolderToolStripMenuItem.BackColor = Color.DimGray;
            tracesFolderToolStripMenuItem.ForeColor = Color.White;
            tracesFolderToolStripMenuItem.Name = "tracesFolderToolStripMenuItem";
            tracesFolderToolStripMenuItem.Size = new Size(187, 22);
            tracesFolderToolStripMenuItem.Text = "Open traces folder";
            tracesFolderToolStripMenuItem.Click += tracesFolderToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(184, 6);
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.BackColor = Color.DimGray;
            quitToolStripMenuItem.ForeColor = Color.White;
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(187, 22);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // networkToolStripMenuItem1
            // 
            networkToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { vATSIMToolStripMenuItem1, iVAOToolStripMenuItem1 });
            networkToolStripMenuItem1.ForeColor = Color.Black;
            networkToolStripMenuItem1.Name = "networkToolStripMenuItem1";
            networkToolStripMenuItem1.ShortcutKeys = Keys.Alt | Keys.N;
            networkToolStripMenuItem1.Size = new Size(64, 26);
            networkToolStripMenuItem1.Text = "Network";
            // 
            // vATSIMToolStripMenuItem1
            // 
            vATSIMToolStripMenuItem1.BackColor = Color.DimGray;
            vATSIMToolStripMenuItem1.Checked = true;
            vATSIMToolStripMenuItem1.CheckOnClick = true;
            vATSIMToolStripMenuItem1.CheckState = CheckState.Checked;
            vATSIMToolStripMenuItem1.ForeColor = Color.White;
            vATSIMToolStripMenuItem1.Name = "vATSIMToolStripMenuItem1";
            vATSIMToolStripMenuItem1.Size = new Size(113, 22);
            vATSIMToolStripMenuItem1.Text = "VATSIM";
            vATSIMToolStripMenuItem1.Click += vATSIMToolStripMenuItem1_Click;
            // 
            // iVAOToolStripMenuItem1
            // 
            iVAOToolStripMenuItem1.BackColor = Color.DimGray;
            iVAOToolStripMenuItem1.CheckOnClick = true;
            iVAOToolStripMenuItem1.ForeColor = Color.White;
            iVAOToolStripMenuItem1.Name = "iVAOToolStripMenuItem1";
            iVAOToolStripMenuItem1.Size = new Size(113, 22);
            iVAOToolStripMenuItem1.Text = "IVAO";
            iVAOToolStripMenuItem1.Click += iVAOToolStripMenuItem1_Click;
            // 
            // linksToolStripMenuItem
            // 
            linksToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openWebSiteToolStripMenuItem1, documentationToolStripMenuItem1, skyboundsAIToolStripMenuItem });
            linksToolStripMenuItem.ForeColor = Color.Black;
            linksToolStripMenuItem.Name = "linksToolStripMenuItem";
            linksToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.L;
            linksToolStripMenuItem.Size = new Size(46, 26);
            linksToolStripMenuItem.Text = "Links";
            // 
            // openWebSiteToolStripMenuItem1
            // 
            openWebSiteToolStripMenuItem1.BackColor = Color.DimGray;
            openWebSiteToolStripMenuItem1.ForeColor = Color.White;
            openWebSiteToolStripMenuItem1.Name = "openWebSiteToolStripMenuItem1";
            openWebSiteToolStripMenuItem1.Size = new Size(157, 22);
            openWebSiteToolStripMenuItem1.Text = "Open web site";
            openWebSiteToolStripMenuItem1.Click += openWebSiteToolStripMenuItem1_Click;
            // 
            // documentationToolStripMenuItem1
            // 
            documentationToolStripMenuItem1.BackColor = Color.DimGray;
            documentationToolStripMenuItem1.ForeColor = Color.White;
            documentationToolStripMenuItem1.Name = "documentationToolStripMenuItem1";
            documentationToolStripMenuItem1.Size = new Size(157, 22);
            documentationToolStripMenuItem1.Text = "Documentation";
            documentationToolStripMenuItem1.Click += documentationToolStripMenuItem1_Click;
            // 
            // skyboundsAIToolStripMenuItem
            // 
            skyboundsAIToolStripMenuItem.BackColor = Color.MidnightBlue;
            skyboundsAIToolStripMenuItem.ForeColor = Color.White;
            skyboundsAIToolStripMenuItem.Name = "skyboundsAIToolStripMenuItem";
            skyboundsAIToolStripMenuItem.Size = new Size(157, 22);
            skyboundsAIToolStripMenuItem.Text = "Skybounds AI";
            skyboundsAIToolStripMenuItem.Click += skyboundsAIToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutSimAddonToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 26);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutSimAddonToolStripMenuItem
            // 
            aboutSimAddonToolStripMenuItem.BackColor = Color.DimGray;
            aboutSimAddonToolStripMenuItem.Name = "aboutSimAddonToolStripMenuItem";
            aboutSimAddonToolStripMenuItem.Size = new Size(166, 22);
            aboutSimAddonToolStripMenuItem.Text = "About SimAddon";
            aboutSimAddonToolStripMenuItem.Click += aboutSimAddonToolStripMenuItem_Click;
            // 
            // btnClose
            // 
            btnClose.Alignment = ToolStripItemAlignment.Right;
            btnClose.AutoSize = false;
            btnClose.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnClose.Font = new Font("Marlett", 12F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(30, 23);
            btnClose.Text = "r";
            btnClose.Click += btnClose_Click;
            // 
            // btnMaximize
            // 
            btnMaximize.Alignment = ToolStripItemAlignment.Right;
            btnMaximize.AutoSize = false;
            btnMaximize.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnMaximize.Font = new Font("Marlett", 12F, FontStyle.Bold);
            btnMaximize.ForeColor = Color.White;
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(30, 23);
            btnMaximize.Tag = "maximize";
            btnMaximize.Text = "1";
            btnMaximize.Click += btnMaximize_Click;
            // 
            // btnMinimize
            // 
            btnMinimize.Alignment = ToolStripItemAlignment.Right;
            btnMinimize.AutoSize = false;
            btnMinimize.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnMinimize.Font = new Font("Marlett", 12F, FontStyle.Bold);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(30, 23);
            btnMinimize.Text = "0";
            btnMinimize.Click += btnMinimize_Click;
            // 
            // ledConnectionStatus
            // 
            ledConnectionStatus.Alignment = ToolStripItemAlignment.Right;
            ledConnectionStatus.AutoSize = false;
            ledConnectionStatus.Margin = new Padding(0);
            ledConnectionStatus.Name = "ledConnectionStatus";
            ledConnectionStatus.Size = new Size(16, 16);
            ledConnectionStatus.ToolTipText = "État de connexion au serveur";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(590, 870);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip1);
            Font = new Font("Arial", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
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

        private SimAddonControls.VSStatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblConnectionStatus;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.Timer timerConnection;
        private VSTabControl tabControl1;
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
        private ToolStripMenuItem skyboundsAIToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem quitToolStripMenuItem;
        private ToolStripButton btnMinimize;
        private ToolStripButton btnMaximize;
        private ToolStripButton btnClose;
        private ToolStripControlHost ledConnectionStatus;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutSimAddonToolStripMenuItem;
    }
}

