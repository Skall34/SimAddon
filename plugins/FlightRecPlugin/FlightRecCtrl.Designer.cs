using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FlightRecPlugin
{
    partial class FlightRecCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlightRecCtrl));
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            tbCallsign = new TextBox();
            btnSaveSettings = new Button();
            btnSubmit = new Button();
            gbDynamicData = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            gbEndInfos = new GroupBox();
            label1 = new Label();
            label14 = new Label();
            label16 = new Label();
            label17 = new Label();
            lbEndPosition = new Label();
            lbEndIata = new Label();
            lbEndFuel = new Label();
            lbEndTime = new Label();
            groupBox6 = new GroupBox();
            lbOnGround = new Label();
            lbAirborn = new Label();
            lbTimeOnGround = new Label();
            lbTimeAirborn = new Label();
            gbStartInfos = new GroupBox();
            label13 = new Label();
            lbStartPosition = new Label();
            lbStartIata = new Label();
            lbStartFuel = new Label();
            lbStartTime = new Label();
            label8 = new Label();
            label4 = new Label();
            label3 = new Label();
            groupBox7 = new GroupBox();
            tbCommentaires = new TextBox();
            pictureBox1 = new PictureBox();
            label15 = new Label();
            label12 = new Label();
            cbMission = new ComboBox();
            label2 = new Label();
            cbNote = new ComboBox();
            lbFret = new Label();
            groupBox3 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            tbEndICAO = new TextBox();
            lbDesignationAvion = new Label();
            lbPayload = new Label();
            cbImmat = new ComboBox();
            label5 = new Label();
            lbLibelleAvion = new Label();
            label6 = new Label();
            panelAircraftTypeIcon = new Panel();
            lbEndICAO = new Label();
            ledCheckCallsign = new SimAddonControls.LedBulb();
            ledCheckImmat = new SimAddonControls.LedBulb();
            ledCheckFreight = new SimAddonControls.LedBulb();
            ledCheckAircraft = new SimAddonControls.LedBulb();
            ledCheckPayload = new SimAddonControls.LedBulb();
            toolTip1 = new ToolTip(components);
            btnReset = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            resetFlightToolStripMenuItem = new ToolStripMenuItem();
            submitFlightToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            debugToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            loginToolStripMenuItem = new ToolStripMenuItem();
            logoutToolStripMenuItem = new ToolStripMenuItem();
            checkSessionToolStripMenuItem = new ToolStripMenuItem();
            engineStopTimer = new Timer(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox5 = new GroupBox();
            btnFlightbook = new Button();
            timerUpdateStaticValues = new Timer(components);
            splitContainer1 = new SplitContainer();
            updatePlaneStatusTimer = new Timer(components);
            timerUpdateFleetStatus = new Timer(components);
            gbDynamicData.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            gbEndInfos.SuspendLayout();
            groupBox6.SuspendLayout();
            gbStartInfos.SuspendLayout();
            groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.FromArgb(255, 128, 0);
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(4, 132);
            label9.Margin = new Padding(4);
            label9.Name = "label9";
            label9.Size = new Size(141, 24);
            label9.TabIndex = 24;
            label9.Text = "Payload (Kg) :";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.FromArgb(255, 128, 0);
            label10.Dock = DockStyle.Fill;
            label10.Location = new Point(4, 36);
            label10.Margin = new Padding(4);
            label10.Name = "label10";
            label10.Size = new Size(141, 24);
            label10.TabIndex = 31;
            label10.Text = "ACARS Aircraft :";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.FromArgb(255, 128, 0);
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(4, 4);
            label11.Margin = new Padding(4);
            label11.Name = "label11";
            label11.Size = new Size(141, 24);
            label11.TabIndex = 33;
            label11.Text = "Pilot callsign :";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tbCallsign
            // 
            tbCallsign.BackColor = Color.FromArgb(192, 255, 192);
            tbCallsign.Dock = DockStyle.Fill;
            tbCallsign.Location = new Point(153, 4);
            tbCallsign.Margin = new Padding(4);
            tbCallsign.Name = "tbCallsign";
            tbCallsign.ShortcutsEnabled = false;
            tbCallsign.Size = new Size(134, 25);
            tbCallsign.TabIndex = 0;
            tbCallsign.TextChanged += TextBox1_TextChanged;
            tbCallsign.KeyPress += tbCallsign_KeyPress;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Enabled = false;
            btnSaveSettings.ForeColor = Color.Gray;
            btnSaveSettings.Location = new Point(295, 4);
            btnSaveSettings.Margin = new Padding(4);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(58, 24);
            btnSaveSettings.TabIndex = 1;
            btnSaveSettings.Text = "Apply";
            btnSaveSettings.UseVisualStyleBackColor = true;
            btnSaveSettings.Click += BtnSaveSettings_Click;
            // 
            // btnSubmit
            // 
            btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSubmit.ForeColor = Color.Black;
            btnSubmit.Location = new Point(370, 15);
            btnSubmit.Margin = new Padding(4);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(163, 27);
            btnSubmit.TabIndex = 24;
            btnSubmit.Text = "Review flight record";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += BtnSubmit_Click;
            // 
            // gbDynamicData
            // 
            gbDynamicData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gbDynamicData.Controls.Add(tableLayoutPanel2);
            gbDynamicData.Location = new Point(3, 193);
            gbDynamicData.Name = "gbDynamicData";
            gbDynamicData.Size = new Size(542, 301);
            gbDynamicData.TabIndex = 39;
            gbDynamicData.TabStop = false;
            gbDynamicData.Text = "Flight summary";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(gbEndInfos, 0, 2);
            tableLayoutPanel2.Controls.Add(groupBox6, 0, 1);
            tableLayoutPanel2.Controls.Add(gbStartInfos, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 21);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 109F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            tableLayoutPanel2.Size = new Size(536, 277);
            tableLayoutPanel2.TabIndex = 54;
            // 
            // gbEndInfos
            // 
            gbEndInfos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbEndInfos.Controls.Add(label1);
            gbEndInfos.Controls.Add(label14);
            gbEndInfos.Controls.Add(label16);
            gbEndInfos.Controls.Add(label17);
            gbEndInfos.Controls.Add(lbEndPosition);
            gbEndInfos.Controls.Add(lbEndIata);
            gbEndInfos.Controls.Add(lbEndFuel);
            gbEndInfos.Controls.Add(lbEndTime);
            gbEndInfos.Location = new Point(3, 165);
            gbEndInfos.Name = "gbEndInfos";
            gbEndInfos.Size = new Size(530, 102);
            gbEndInfos.TabIndex = 53;
            gbEndInfos.TabStop = false;
            gbEndInfos.Text = "End";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 39);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(110, 18);
            label1.TabIndex = 59;
            label1.Text = "Position Name";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(7, 21);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(106, 18);
            label14.TabIndex = 58;
            label14.Text = "Position ICAO";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(7, 75);
            label16.Margin = new Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new Size(73, 18);
            label16.TabIndex = 57;
            label16.Text = "Fuel (Kg)";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(7, 57);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(43, 18);
            label17.TabIndex = 56;
            label17.Text = "Time";
            // 
            // lbEndPosition
            // 
            lbEndPosition.AutoSize = true;
            lbEndPosition.BackColor = Color.FromArgb(255, 128, 0);
            lbEndPosition.Location = new Point(126, 39);
            lbEndPosition.Name = "lbEndPosition";
            lbEndPosition.Size = new Size(17, 18);
            lbEndPosition.TabIndex = 55;
            lbEndPosition.Text = "?";
            lbEndPosition.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbEndIata
            // 
            lbEndIata.AutoSize = true;
            lbEndIata.BackColor = Color.FromArgb(255, 128, 0);
            lbEndIata.Location = new Point(126, 21);
            lbEndIata.Name = "lbEndIata";
            lbEndIata.Size = new Size(44, 18);
            lbEndIata.TabIndex = 54;
            lbEndIata.Text = "????";
            lbEndIata.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbEndFuel
            // 
            lbEndFuel.AutoSize = true;
            lbEndFuel.BackColor = Color.FromArgb(255, 128, 0);
            lbEndFuel.Location = new Point(126, 75);
            lbEndFuel.Name = "lbEndFuel";
            lbEndFuel.Size = new Size(44, 18);
            lbEndFuel.TabIndex = 53;
            lbEndFuel.Text = "????";
            lbEndFuel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbEndTime
            // 
            lbEndTime.AutoSize = true;
            lbEndTime.BackColor = Color.FromArgb(255, 128, 0);
            lbEndTime.Location = new Point(126, 57);
            lbEndTime.Name = "lbEndTime";
            lbEndTime.Size = new Size(32, 18);
            lbEndTime.TabIndex = 52;
            lbEndTime.Text = "--:--";
            lbEndTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox6
            // 
            groupBox6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox6.Controls.Add(lbOnGround);
            groupBox6.Controls.Add(lbAirborn);
            groupBox6.Controls.Add(lbTimeOnGround);
            groupBox6.Controls.Add(lbTimeAirborn);
            groupBox6.Location = new Point(3, 112);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(530, 45);
            groupBox6.TabIndex = 53;
            groupBox6.TabStop = false;
            groupBox6.Text = "Time blocks";
            // 
            // lbOnGround
            // 
            lbOnGround.AutoSize = true;
            lbOnGround.Location = new Point(267, 19);
            lbOnGround.Margin = new Padding(4, 0, 4, 0);
            lbOnGround.Name = "lbOnGround";
            lbOnGround.Size = new Size(87, 18);
            lbOnGround.TabIndex = 45;
            lbOnGround.Text = "On Ground";
            lbOnGround.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbAirborn
            // 
            lbAirborn.AutoSize = true;
            lbAirborn.Location = new Point(53, 19);
            lbAirborn.Margin = new Padding(4, 0, 4, 0);
            lbAirborn.Name = "lbAirborn";
            lbAirborn.Size = new Size(60, 18);
            lbAirborn.TabIndex = 42;
            lbAirborn.Text = "Airborn";
            lbAirborn.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbTimeOnGround
            // 
            lbTimeOnGround.AutoSize = true;
            lbTimeOnGround.BackColor = Color.FromArgb(255, 128, 0);
            lbTimeOnGround.Location = new Point(376, 19);
            lbTimeOnGround.Name = "lbTimeOnGround";
            lbTimeOnGround.Size = new Size(32, 18);
            lbTimeOnGround.TabIndex = 46;
            lbTimeOnGround.Text = "--:--";
            lbTimeOnGround.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbTimeAirborn
            // 
            lbTimeAirborn.AutoSize = true;
            lbTimeAirborn.BackColor = Color.FromArgb(255, 128, 0);
            lbTimeAirborn.Location = new Point(126, 19);
            lbTimeAirborn.Name = "lbTimeAirborn";
            lbTimeAirborn.Size = new Size(32, 18);
            lbTimeAirborn.TabIndex = 43;
            lbTimeAirborn.Text = "--:--";
            lbTimeAirborn.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbStartInfos
            // 
            gbStartInfos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gbStartInfos.Controls.Add(label13);
            gbStartInfos.Controls.Add(lbStartPosition);
            gbStartInfos.Controls.Add(lbStartIata);
            gbStartInfos.Controls.Add(lbStartFuel);
            gbStartInfos.Controls.Add(lbStartTime);
            gbStartInfos.Controls.Add(label8);
            gbStartInfos.Controls.Add(label4);
            gbStartInfos.Controls.Add(label3);
            gbStartInfos.Location = new Point(3, 3);
            gbStartInfos.Name = "gbStartInfos";
            gbStartInfos.Size = new Size(530, 101);
            gbStartInfos.TabIndex = 52;
            gbStartInfos.TabStop = false;
            gbStartInfos.Text = "Start";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(7, 39);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(110, 18);
            label13.TabIndex = 55;
            label13.Text = "Position Name";
            // 
            // lbStartPosition
            // 
            lbStartPosition.AutoSize = true;
            lbStartPosition.BackColor = Color.FromArgb(255, 128, 0);
            lbStartPosition.Location = new Point(126, 39);
            lbStartPosition.Name = "lbStartPosition";
            lbStartPosition.Size = new Size(17, 18);
            lbStartPosition.TabIndex = 54;
            lbStartPosition.Text = "?";
            lbStartPosition.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbStartIata
            // 
            lbStartIata.AutoSize = true;
            lbStartIata.BackColor = Color.FromArgb(255, 128, 0);
            lbStartIata.Location = new Point(126, 21);
            lbStartIata.Name = "lbStartIata";
            lbStartIata.Size = new Size(44, 18);
            lbStartIata.TabIndex = 53;
            lbStartIata.Text = "????";
            lbStartIata.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbStartFuel
            // 
            lbStartFuel.AutoSize = true;
            lbStartFuel.BackColor = Color.FromArgb(255, 128, 0);
            lbStartFuel.Location = new Point(126, 75);
            lbStartFuel.Name = "lbStartFuel";
            lbStartFuel.Size = new Size(44, 18);
            lbStartFuel.TabIndex = 52;
            lbStartFuel.Text = "????";
            lbStartFuel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbStartTime
            // 
            lbStartTime.AutoSize = true;
            lbStartTime.BackColor = Color.FromArgb(255, 128, 0);
            lbStartTime.Location = new Point(126, 57);
            lbStartTime.Name = "lbStartTime";
            lbStartTime.Size = new Size(32, 18);
            lbStartTime.TabIndex = 51;
            lbStartTime.Text = "--:--";
            lbStartTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(7, 21);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(106, 18);
            label8.TabIndex = 50;
            label8.Text = "Position ICAO";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 75);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(73, 18);
            label4.TabIndex = 49;
            label4.Text = "Fuel (Kg)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 57);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(43, 18);
            label3.TabIndex = 48;
            label3.Text = "Time";
            // 
            // groupBox7
            // 
            groupBox7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox7.Controls.Add(tbCommentaires);
            groupBox7.Controls.Add(pictureBox1);
            groupBox7.Controls.Add(label15);
            groupBox7.Controls.Add(label12);
            groupBox7.Controls.Add(cbMission);
            groupBox7.Controls.Add(label2);
            groupBox7.Controls.Add(cbNote);
            groupBox7.Location = new Point(5, 0);
            groupBox7.Margin = new Padding(0);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new Padding(0);
            groupBox7.Size = new Size(540, 233);
            groupBox7.TabIndex = 54;
            groupBox7.TabStop = false;
            groupBox7.Text = "Flight evaluation";
            // 
            // tbCommentaires
            // 
            tbCommentaires.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbCommentaires.BackColor = Color.FromArgb(255, 192, 128);
            tbCommentaires.Location = new Point(109, 22);
            tbCommentaires.Margin = new Padding(5, 4, 5, 4);
            tbCommentaires.Multiline = true;
            tbCommentaires.Name = "tbCommentaires";
            tbCommentaires.ReadOnly = true;
            tbCommentaires.ScrollBars = ScrollBars.Vertical;
            tbCommentaires.Size = new Size(426, 169);
            tbCommentaires.TabIndex = 20;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(7, 43);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 50;
            pictureBox1.TabStop = false;
            // 
            // label15
            // 
            label15.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label15.AutoSize = true;
            label15.Location = new Point(248, 201);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(63, 18);
            label15.TabIndex = 32;
            label15.Text = "Mission";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(17, 22);
            label12.Margin = new Padding(5, 0, 5, 0);
            label12.Name = "label12";
            label12.Size = new Size(83, 18);
            label12.TabIndex = 7;
            label12.Text = "Comments";
            // 
            // cbMission
            // 
            cbMission.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cbMission.DrawMode = DrawMode.OwnerDrawFixed;
            cbMission.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMission.FormattingEnabled = true;
            cbMission.Location = new Point(318, 198);
            cbMission.MaxDropDownItems = 12;
            cbMission.Name = "cbMission";
            cbMission.Size = new Size(219, 26);
            cbMission.TabIndex = 22;
            cbMission.DrawItem += cbMission_DrawItem;
            cbMission.SelectedIndexChanged += cbMission_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(17, 201);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(83, 18);
            label2.TabIndex = 16;
            label2.Text = "Evaluation";
            // 
            // cbNote
            // 
            cbNote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            cbNote.DropDownStyle = ComboBoxStyle.DropDownList;
            cbNote.FormattingEnabled = true;
            cbNote.Items.AddRange(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            cbNote.Location = new Point(109, 198);
            cbNote.MaxDropDownItems = 10;
            cbNote.Name = "cbNote";
            cbNote.Size = new Size(66, 26);
            cbNote.TabIndex = 21;
            cbNote.SelectedIndexChanged += cbNote_SelectedIndexChanged;
            cbNote.MouseHover += CbNote_MouseHover;
            // 
            // lbFret
            // 
            lbFret.AutoSize = true;
            lbFret.BackColor = Color.FromArgb(255, 128, 0);
            tableLayoutPanel3.SetColumnSpan(lbFret, 3);
            lbFret.Dock = DockStyle.Fill;
            lbFret.Location = new Point(153, 68);
            lbFret.Margin = new Padding(4);
            lbFret.Name = "lbFret";
            lbFret.Size = new Size(359, 24);
            lbFret.TabIndex = 43;
            lbFret.Text = "Available freight at ---- : ----";
            lbFret.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(tableLayoutPanel3);
            groupBox3.Location = new Point(3, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(542, 183);
            groupBox3.TabIndex = 41;
            groupBox3.TabStop = false;
            groupBox3.Text = "Static data";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 5;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 149F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 142F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 78F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Controls.Add(label11, 0, 0);
            tableLayoutPanel3.Controls.Add(tbEndICAO, 3, 4);
            tableLayoutPanel3.Controls.Add(lbDesignationAvion, 2, 1);
            tableLayoutPanel3.Controls.Add(tbCallsign, 1, 0);
            tableLayoutPanel3.Controls.Add(lbPayload, 1, 4);
            tableLayoutPanel3.Controls.Add(label9, 0, 4);
            tableLayoutPanel3.Controls.Add(btnSaveSettings, 2, 0);
            tableLayoutPanel3.Controls.Add(label10, 0, 1);
            tableLayoutPanel3.Controls.Add(cbImmat, 1, 1);
            tableLayoutPanel3.Controls.Add(label5, 0, 3);
            tableLayoutPanel3.Controls.Add(lbLibelleAvion, 1, 3);
            tableLayoutPanel3.Controls.Add(lbFret, 1, 2);
            tableLayoutPanel3.Controls.Add(label6, 0, 2);
            tableLayoutPanel3.Controls.Add(panelAircraftTypeIcon, 3, 0);
            tableLayoutPanel3.Controls.Add(lbEndICAO, 2, 4);
            tableLayoutPanel3.Controls.Add(ledCheckCallsign, 4, 0);
            tableLayoutPanel3.Controls.Add(ledCheckImmat, 4, 1);
            tableLayoutPanel3.Controls.Add(ledCheckFreight, 4, 2);
            tableLayoutPanel3.Controls.Add(ledCheckAircraft, 4, 3);
            tableLayoutPanel3.Controls.Add(ledCheckPayload, 4, 4);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 21);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 5;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(536, 159);
            tableLayoutPanel3.TabIndex = 50;
            // 
            // tbEndICAO
            // 
            tbEndICAO.BackColor = Color.White;
            tbEndICAO.Dock = DockStyle.Fill;
            tbEndICAO.Location = new Point(442, 132);
            tbEndICAO.Margin = new Padding(4);
            tbEndICAO.Name = "tbEndICAO";
            tbEndICAO.ShortcutsEnabled = false;
            tbEndICAO.Size = new Size(70, 25);
            tbEndICAO.TabIndex = 48;
            tbEndICAO.TextAlign = HorizontalAlignment.Right;
            tbEndICAO.TextChanged += tbEndICAO_TextChanged;
            tbEndICAO.MouseHover += tbEndICAO_MouseHover;
            // 
            // lbDesignationAvion
            // 
            lbDesignationAvion.AutoSize = true;
            lbDesignationAvion.BackColor = Color.FromArgb(255, 128, 0);
            lbDesignationAvion.Dock = DockStyle.Fill;
            lbDesignationAvion.Location = new Point(295, 36);
            lbDesignationAvion.Margin = new Padding(4);
            lbDesignationAvion.Name = "lbDesignationAvion";
            lbDesignationAvion.Size = new Size(139, 24);
            lbDesignationAvion.TabIndex = 44;
            lbDesignationAvion.Text = "<no plane selected>";
            lbDesignationAvion.TextAlign = ContentAlignment.MiddleLeft;
            lbDesignationAvion.Click += lbDesignationAvion_Click;
            // 
            // lbPayload
            // 
            lbPayload.AutoSize = true;
            lbPayload.BackColor = Color.FromArgb(255, 128, 0);
            lbPayload.Dock = DockStyle.Fill;
            lbPayload.Location = new Point(153, 132);
            lbPayload.Margin = new Padding(4);
            lbPayload.Name = "lbPayload";
            lbPayload.Size = new Size(134, 24);
            lbPayload.TabIndex = 45;
            lbPayload.Text = "Not Yet Available";
            lbPayload.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cbImmat
            // 
            cbImmat.Dock = DockStyle.Fill;
            cbImmat.DrawMode = DrawMode.OwnerDrawFixed;
            cbImmat.DropDownStyle = ComboBoxStyle.DropDownList;
            cbImmat.FormattingEnabled = true;
            cbImmat.Items.AddRange(new object[] { "none" });
            cbImmat.Location = new Point(152, 35);
            cbImmat.MaxDropDownItems = 20;
            cbImmat.Name = "cbImmat";
            cbImmat.Size = new Size(136, 26);
            cbImmat.TabIndex = 2;
            cbImmat.DrawItem += cbImmat_DrawItem;
            cbImmat.SelectedIndexChanged += CbImmat_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(255, 128, 0);
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(4, 100);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(141, 24);
            label5.TabIndex = 47;
            label5.Text = "SIM Aircraft  :";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbLibelleAvion
            // 
            lbLibelleAvion.AutoSize = true;
            lbLibelleAvion.BackColor = Color.FromArgb(255, 128, 0);
            tableLayoutPanel3.SetColumnSpan(lbLibelleAvion, 3);
            lbLibelleAvion.Dock = DockStyle.Fill;
            lbLibelleAvion.Location = new Point(153, 100);
            lbLibelleAvion.Margin = new Padding(4);
            lbLibelleAvion.Name = "lbLibelleAvion";
            lbLibelleAvion.Size = new Size(359, 24);
            lbLibelleAvion.TabIndex = 46;
            lbLibelleAvion.Text = "Not Yet Available";
            lbLibelleAvion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(255, 128, 0);
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(4, 68);
            label6.Margin = new Padding(4);
            label6.Name = "label6";
            label6.Size = new Size(141, 24);
            label6.TabIndex = 50;
            label6.Text = "Freight :";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panelAircraftTypeIcon
            // 
            panelAircraftTypeIcon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panelAircraftTypeIcon.BackColor = Color.White;
            panelAircraftTypeIcon.BackgroundImageLayout = ImageLayout.Stretch;
            panelAircraftTypeIcon.BorderStyle = BorderStyle.Fixed3D;
            panelAircraftTypeIcon.Location = new Point(444, 3);
            panelAircraftTypeIcon.Name = "panelAircraftTypeIcon";
            tableLayoutPanel3.SetRowSpan(panelAircraftTypeIcon, 2);
            panelAircraftTypeIcon.Size = new Size(69, 58);
            panelAircraftTypeIcon.TabIndex = 51;
            // 
            // lbEndICAO
            // 
            lbEndICAO.AutoSize = true;
            lbEndICAO.BackColor = Color.FromArgb(255, 128, 0);
            lbEndICAO.Dock = DockStyle.Fill;
            lbEndICAO.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lbEndICAO.Location = new Point(295, 132);
            lbEndICAO.Margin = new Padding(4);
            lbEndICAO.Name = "lbEndICAO";
            lbEndICAO.Size = new Size(139, 24);
            lbEndICAO.TabIndex = 49;
            lbEndICAO.Text = "Opt: End ICAO";
            lbEndICAO.TextAlign = ContentAlignment.MiddleRight;
            lbEndICAO.Click += lbEndICAO_Click;
            // 
            // ledCheckCallsign
            // 
            ledCheckCallsign.Color = Color.LightGray;
            ledCheckCallsign.Location = new Point(519, 3);
            ledCheckCallsign.Name = "ledCheckCallsign";
            ledCheckCallsign.On = true;
            ledCheckCallsign.Size = new Size(14, 26);
            ledCheckCallsign.TabIndex = 52;
            // 
            // ledCheckImmat
            // 
            ledCheckImmat.Color = Color.LightGray;
            ledCheckImmat.Location = new Point(519, 35);
            ledCheckImmat.Name = "ledCheckImmat";
            ledCheckImmat.On = true;
            ledCheckImmat.Size = new Size(14, 26);
            ledCheckImmat.TabIndex = 53;
            ledCheckImmat.DoubleClick += ledCheckAircraft_DoubleClick;
            ledCheckImmat.MouseEnter += ledCheckAircraft_MouseEnter;
            ledCheckImmat.MouseLeave += ledCheckAircraft_MouseLeave;
            ledCheckImmat.MouseHover += ledCheckAircraft_MouseHover;
            // 
            // ledCheckFreight
            // 
            ledCheckFreight.Color = Color.LightGray;
            ledCheckFreight.Location = new Point(519, 67);
            ledCheckFreight.Name = "ledCheckFreight";
            ledCheckFreight.On = true;
            ledCheckFreight.Size = new Size(14, 26);
            ledCheckFreight.TabIndex = 54;
            // 
            // ledCheckAircraft
            // 
            ledCheckAircraft.Color = Color.LightGray;
            ledCheckAircraft.Location = new Point(519, 99);
            ledCheckAircraft.Name = "ledCheckAircraft";
            ledCheckAircraft.On = true;
            ledCheckAircraft.Size = new Size(14, 26);
            ledCheckAircraft.TabIndex = 55;
            ledCheckAircraft.DoubleClick += ledCheckAircraft_DoubleClick;
            ledCheckAircraft.MouseEnter += ledCheckAircraft_MouseEnter;
            ledCheckAircraft.MouseLeave += ledCheckAircraft_MouseLeave;
            ledCheckAircraft.MouseHover += ledCheckAircraft_MouseHover;
            // 
            // ledCheckPayload
            // 
            ledCheckPayload.Color = Color.LightGray;
            ledCheckPayload.Location = new Point(519, 131);
            ledCheckPayload.Name = "ledCheckPayload";
            ledCheckPayload.On = true;
            ledCheckPayload.Size = new Size(14, 26);
            ledCheckPayload.TabIndex = 56;
            // 
            // btnReset
            // 
            btnReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnReset.ForeColor = Color.Black;
            btnReset.Location = new Point(6, 15);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(101, 27);
            btnReset.TabIndex = 25;
            btnReset.Text = "Reset flight";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += BtnReset_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { resetFlightToolStripMenuItem, submitFlightToolStripMenuItem, toolStripSeparator1, debugToolStripMenuItem, toolStripSeparator2, loginToolStripMenuItem, logoutToolStripMenuItem, checkSessionToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(147, 148);
            // 
            // resetFlightToolStripMenuItem
            // 
            resetFlightToolStripMenuItem.Name = "resetFlightToolStripMenuItem";
            resetFlightToolStripMenuItem.Size = new Size(146, 22);
            resetFlightToolStripMenuItem.Text = "Reset flight";
            resetFlightToolStripMenuItem.Click += resetFlightToolStripMenuItem_Click;
            // 
            // submitFlightToolStripMenuItem
            // 
            submitFlightToolStripMenuItem.Enabled = false;
            submitFlightToolStripMenuItem.Name = "submitFlightToolStripMenuItem";
            submitFlightToolStripMenuItem.Size = new Size(146, 22);
            submitFlightToolStripMenuItem.Text = "Save Flight";
            submitFlightToolStripMenuItem.Click += submitFlightToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(143, 6);
            // 
            // debugToolStripMenuItem
            // 
            debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            debugToolStripMenuItem.Size = new Size(146, 22);
            debugToolStripMenuItem.Text = "Debug";
            debugToolStripMenuItem.Click += debugToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(143, 6);
            // 
            // loginToolStripMenuItem
            // 
            loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            loginToolStripMenuItem.Size = new Size(146, 22);
            loginToolStripMenuItem.Text = "Login";
            loginToolStripMenuItem.Click += loginToolStripMenuItem_Click;
            // 
            // logoutToolStripMenuItem
            // 
            logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            logoutToolStripMenuItem.Size = new Size(146, 22);
            logoutToolStripMenuItem.Text = "Logout";
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;
            // 
            // checkSessionToolStripMenuItem
            // 
            checkSessionToolStripMenuItem.Name = "checkSessionToolStripMenuItem";
            checkSessionToolStripMenuItem.Size = new Size(146, 22);
            checkSessionToolStripMenuItem.Text = "CheckSession";
            checkSessionToolStripMenuItem.Click += checkSessionToolStripMenuItem_Click;
            // 
            // engineStopTimer
            // 
            engineStopTimer.Interval = 5000;
            engineStopTimer.Tick += engineStopTimer_Tick;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 0);
            tableLayoutPanel1.Controls.Add(gbDynamicData, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(548, 497);
            tableLayoutPanel1.TabIndex = 42;
            // 
            // groupBox5
            // 
            groupBox5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox5.Controls.Add(btnFlightbook);
            groupBox5.Controls.Add(btnReset);
            groupBox5.Controls.Add(btnSubmit);
            groupBox5.Location = new Point(5, 232);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(540, 48);
            groupBox5.TabIndex = 42;
            groupBox5.TabStop = false;
            // 
            // btnFlightbook
            // 
            btnFlightbook.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnFlightbook.Location = new Point(112, 15);
            btnFlightbook.Name = "btnFlightbook";
            btnFlightbook.Size = new Size(95, 27);
            btnFlightbook.TabIndex = 26;
            btnFlightbook.Text = "Flightbook";
            btnFlightbook.UseVisualStyleBackColor = true;
            btnFlightbook.Click += btnFlightbook_Click;
            // 
            // timerUpdateStaticValues
            // 
            timerUpdateStaticValues.Interval = 3000;
            timerUpdateStaticValues.Tick += timerUpdateStaticValues_Tick;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(groupBox5);
            splitContainer1.Panel2.Controls.Add(groupBox7);
            splitContainer1.Size = new Size(548, 784);
            splitContainer1.SplitterDistance = 497;
            splitContainer1.TabIndex = 43;
            // 
            // updatePlaneStatusTimer
            // 
            updatePlaneStatusTimer.Interval = 30000;
            updatePlaneStatusTimer.Tick += updatePlaneStatusTimer_Tick;
            // 
            // timerUpdateFleetStatus
            // 
            timerUpdateFleetStatus.Interval = 300000;
            timerUpdateFleetStatus.Tick += timerUpdateFleetStatus_Tick;
            // 
            // FlightRecCtrl
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 128, 0);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(splitContainer1);
            Enabled = false;
            Font = new System.Drawing.Font("Arial", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Margin = new Padding(5, 4, 5, 4);
            Name = "FlightRecCtrl";
            Size = new Size(548, 784);
            gbDynamicData.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            gbEndInfos.ResumeLayout(false);
            gbEndInfos.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            gbStartInfos.ResumeLayout(false);
            gbStartInfos.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Label label9;
        private Label label10;
        private Label label11;
        private TextBox tbCallsign;
        private Button btnSaveSettings;
        private Button btnSubmit;
        private GroupBox gbDynamicData;
        private GroupBox groupBox3;
        private Label label2;
        private TextBox tbCommentaires;
        private Label label12;
        private ComboBox cbNote;
        private ToolTip toolTip1;
        private ComboBox cbImmat;
        private Label lbFret;
        private Label label15;
        private ComboBox cbMission;
        private Button btnReset;
        private Label lbDesignationAvion;
        private Label lbPayload;
        private Label lbTimeOnGround;
        private Label lbOnGround;
        private Label lbTimeAirborn;
        private Label lbAirborn;
        private Label lbLibelleAvion;
        private Label label5;
        private GroupBox gbEndInfos;
        private GroupBox gbStartInfos;
        private Label label13;
        private Label lbStartPosition;
        private Label lbStartIata;
        private Label lbStartFuel;
        private Label lbStartTime;
        private Label label8;
        private Label label4;
        private Label label3;
        private Label label1;
        private Label label14;
        private Label label16;
        private Label label17;
        private Label lbEndPosition;
        private Label lbEndIata;
        private Label lbEndFuel;
        private Label lbEndTime;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem resetFlightToolStripMenuItem;
        private ToolStripMenuItem submitFlightToolStripMenuItem;
        private TextBox tbEndICAO;
        private Label lbEndICAO;
        private System.Windows.Forms.Timer engineStopTimer;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox5;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private PictureBox pictureBox1;
        private Timer timerUpdateStaticValues;
        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label6;
        private Panel panelAircraftTypeIcon;
        private SimAddonControls.LedBulb ledCheckCallsign;
        private SimAddonControls.LedBulb ledCheckImmat;
        private SimAddonControls.LedBulb ledCheckFreight;
        private SimAddonControls.LedBulb ledCheckAircraft;
        private SimAddonControls.LedBulb ledCheckPayload;
        private ToolStripMenuItem debugToolStripMenuItem;
        private Button btnFlightbook;
        private Timer updatePlaneStatusTimer;
        private ToolStripMenuItem loginToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private ToolStripMenuItem checkSessionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private Timer timerUpdateFleetStatus;
    }
}
