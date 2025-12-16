namespace SimAddon
{
    partial class SimaddonSettingsForm
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
            cbAlwaysOnTop = new CheckBox();
            cbAutoHide = new CheckBox();
            btnApply = new Button();
            btnCancel = new Button();
            listView1 = new ListView();
            Plugins = new ColumnHeader();
            tabControl1 = new TabControl();
            tabPageWindow = new TabPage();
            tabPagePlugins = new TabPage();
            tabPageFolders = new TabPage();
            btnChooseScreenshotFolder = new Button();
            tbScreenshotsFolder = new TextBox();
            label2 = new Label();
            tabSiteURL = new TabPage();
            linkTest = new LinkLabel();
            tbSiteUrl = new TextBox();
            tabControl1.SuspendLayout();
            tabPageWindow.SuspendLayout();
            tabPagePlugins.SuspendLayout();
            tabPageFolders.SuspendLayout();
            tabSiteURL.SuspendLayout();
            SuspendLayout();
            // 
            // cbAlwaysOnTop
            // 
            cbAlwaysOnTop.AutoSize = true;
            cbAlwaysOnTop.Location = new Point(17, 15);
            cbAlwaysOnTop.Name = "cbAlwaysOnTop";
            cbAlwaysOnTop.Size = new Size(101, 19);
            cbAlwaysOnTop.TabIndex = 0;
            cbAlwaysOnTop.Text = "Always on top";
            cbAlwaysOnTop.UseVisualStyleBackColor = true;
            cbAlwaysOnTop.CheckedChanged += cbAlwaysOnTop_CheckedChanged;
            // 
            // cbAutoHide
            // 
            cbAutoHide.AutoSize = true;
            cbAutoHide.Location = new Point(17, 40);
            cbAutoHide.Name = "cbAutoHide";
            cbAutoHide.Size = new Size(174, 19);
            cbAutoHide.TabIndex = 1;
            cbAutoHide.Text = "Auto hide on engine startup";
            cbAutoHide.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            btnApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnApply.Location = new Point(296, 275);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(75, 23);
            btnApply.TabIndex = 1;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancel.Location = new Point(18, 275);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // listView1
            // 
            listView1.CheckBoxes = true;
            listView1.Columns.AddRange(new ColumnHeader[] { Plugins });
            listView1.Dock = DockStyle.Fill;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView1.Location = new Point(3, 3);
            listView1.Name = "listView1";
            listView1.ShowGroups = false;
            listView1.Size = new Size(369, 232);
            listView1.TabIndex = 3;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // Plugins
            // 
            Plugins.Text = "Select visible plugins";
            Plugins.Width = 200;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPageWindow);
            tabControl1.Controls.Add(tabPagePlugins);
            tabControl1.Controls.Add(tabPageFolders);
            tabControl1.Controls.Add(tabSiteURL);
            tabControl1.Location = new Point(1, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(383, 266);
            tabControl1.TabIndex = 4;
            // 
            // tabPageWindow
            // 
            tabPageWindow.Controls.Add(cbAutoHide);
            tabPageWindow.Controls.Add(cbAlwaysOnTop);
            tabPageWindow.Location = new Point(4, 24);
            tabPageWindow.Name = "tabPageWindow";
            tabPageWindow.Padding = new Padding(3);
            tabPageWindow.Size = new Size(375, 238);
            tabPageWindow.TabIndex = 0;
            tabPageWindow.Text = "Window";
            tabPageWindow.UseVisualStyleBackColor = true;
            // 
            // tabPagePlugins
            // 
            tabPagePlugins.Controls.Add(listView1);
            tabPagePlugins.Location = new Point(4, 24);
            tabPagePlugins.Name = "tabPagePlugins";
            tabPagePlugins.Padding = new Padding(3);
            tabPagePlugins.Size = new Size(375, 238);
            tabPagePlugins.TabIndex = 1;
            tabPagePlugins.Text = "Plugins";
            tabPagePlugins.UseVisualStyleBackColor = true;
            // 
            // tabPageFolders
            // 
            tabPageFolders.Controls.Add(btnChooseScreenshotFolder);
            tabPageFolders.Controls.Add(tbScreenshotsFolder);
            tabPageFolders.Controls.Add(label2);
            tabPageFolders.Location = new Point(4, 24);
            tabPageFolders.Name = "tabPageFolders";
            tabPageFolders.Padding = new Padding(3);
            tabPageFolders.Size = new Size(375, 238);
            tabPageFolders.TabIndex = 2;
            tabPageFolders.Text = "Folders";
            tabPageFolders.UseVisualStyleBackColor = true;
            // 
            // btnChooseScreenshotFolder
            // 
            btnChooseScreenshotFolder.Location = new Point(343, 33);
            btnChooseScreenshotFolder.Name = "btnChooseScreenshotFolder";
            btnChooseScreenshotFolder.Size = new Size(26, 23);
            btnChooseScreenshotFolder.TabIndex = 2;
            btnChooseScreenshotFolder.Text = "...";
            btnChooseScreenshotFolder.UseVisualStyleBackColor = true;
            btnChooseScreenshotFolder.Click += btnChooseScreenshotFolder_Click;
            // 
            // tbScreenshotsFolder
            // 
            tbScreenshotsFolder.Location = new Point(6, 33);
            tbScreenshotsFolder.Name = "tbScreenshotsFolder";
            tbScreenshotsFolder.Size = new Size(331, 23);
            tbScreenshotsFolder.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 15);
            label2.Name = "label2";
            label2.Size = new Size(157, 15);
            label2.TabIndex = 0;
            label2.Text = "Simulator screenshots folder";
            // 
            // tabSiteURL
            // 
            tabSiteURL.Controls.Add(linkTest);
            tabSiteURL.Controls.Add(tbSiteUrl);
            tabSiteURL.Location = new Point(4, 24);
            tabSiteURL.Name = "tabSiteURL";
            tabSiteURL.Size = new Size(375, 238);
            tabSiteURL.TabIndex = 3;
            tabSiteURL.Text = "URL";
            tabSiteURL.UseVisualStyleBackColor = true;
            // 
            // linkTest
            // 
            linkTest.AutoSize = true;
            linkTest.Location = new Point(304, 20);
            linkTest.Name = "linkTest";
            linkTest.Size = new Size(62, 15);
            linkTest.TabIndex = 1;
            linkTest.TabStop = true;
            linkTest.Text = "Check link";
            linkTest.LinkClicked += linkTest_LinkClicked;
            // 
            // tbSiteUrl
            // 
            tbSiteUrl.Location = new Point(13, 17);
            tbSiteUrl.Name = "tbSiteUrl";
            tbSiteUrl.Size = new Size(285, 23);
            tbSiteUrl.TabIndex = 0;
            tbSiteUrl.TextChanged += tbSiteUrl_TextChanged;
            // 
            // SimaddonSettingsForm
            // 
            AcceptButton = btnApply;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(383, 310);
            Controls.Add(tabControl1);
            Controls.Add(btnCancel);
            Controls.Add(btnApply);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "SimaddonSettingsForm";
            Text = "SimaddonSettingsForm";
            Load += SimaddonSettingsForm_Load;
            tabControl1.ResumeLayout(false);
            tabPageWindow.ResumeLayout(false);
            tabPageWindow.PerformLayout();
            tabPagePlugins.ResumeLayout(false);
            tabPageFolders.ResumeLayout(false);
            tabPageFolders.PerformLayout();
            tabSiteURL.ResumeLayout(false);
            tabSiteURL.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private CheckBox cbAutoHide;
        private CheckBox cbAlwaysOnTop;
        private Button btnApply;
        private Button btnCancel;
        private ListView listView1;
        private TabControl tabControl1;
        private TabPage tabPageWindow;
        private TabPage tabPagePlugins;
        private TabPage tabPageFolders;
        private TextBox tbScreenshotsFolder;
        private Label label2;
        private Button btnChooseScreenshotFolder;
        private ColumnHeader Plugins;
        private TabPage tabSiteURL;
        private LinkLabel linkTest;
        private TextBox tbSiteUrl;
    }
}