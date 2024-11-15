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
            timerMain = new Timer(components);
            timerConnection = new Timer(components);
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            tbCallsign = new TextBox();
            btnSaveSettings = new Button();
            btnSubmit = new Button();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            label13 = new Label();
            lbStartPosition = new Label();
            lbStartIata = new Label();
            lbStartFuel = new Label();
            lbStartTime = new Label();
            label8 = new Label();
            label4 = new Label();
            label3 = new Label();
            groupBox4 = new GroupBox();
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
            lbDesignationAvion = new Label();
            tbEndICAO = new TextBox();
            lbEndICAO = new Label();
            label5 = new Label();
            lbLibelleAvion = new Label();
            lbPayload = new Label();
            cbImmat = new ComboBox();
            toolTip1 = new ToolTip(components);
            btnReset = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            resetFlightToolStripMenuItem = new ToolStripMenuItem();
            submitFlightToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            engineStopTimer = new Timer(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox5 = new GroupBox();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox3.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // timerMain
            // 
            timerMain.Tick += timerMain_Tick;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(24, 141);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(105, 18);
            label9.TabIndex = 24;
            label9.Text = "Payload (Kg) :";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(24, 53);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(59, 18);
            label10.TabIndex = 31;
            label10.Text = "Aircraft";
            label10.Click += label10_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(19, 21);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(98, 18);
            label11.TabIndex = 33;
            label11.Text = "Pilot callsign";
            // 
            // tbCallsign
            // 
            tbCallsign.BackColor = Color.FromArgb(192, 255, 192);
            tbCallsign.Location = new Point(148, 18);
            tbCallsign.Margin = new Padding(4);
            tbCallsign.Name = "tbCallsign";
            tbCallsign.ShortcutsEnabled = false;
            tbCallsign.Size = new Size(111, 25);
            tbCallsign.TabIndex = 0;
            tbCallsign.TextAlign = HorizontalAlignment.Right;
            tbCallsign.TextChanged += TextBox1_TextChanged;
            // 
            // btnSaveSettings
            // 
            btnSaveSettings.Enabled = false;
            btnSaveSettings.ForeColor = Color.Gray;
            btnSaveSettings.Location = new Point(284, 18);
            btnSaveSettings.Margin = new Padding(4);
            btnSaveSettings.Name = "btnSaveSettings";
            btnSaveSettings.Size = new Size(58, 25);
            btnSaveSettings.TabIndex = 1;
            btnSaveSettings.Text = "Apply";
            btnSaveSettings.UseVisualStyleBackColor = true;
            btnSaveSettings.Click += BtnSaveSettings_Click;
            // 
            // btnSubmit
            // 
            btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSubmit.Enabled = false;
            btnSubmit.ForeColor = Color.Black;
            btnSubmit.Location = new Point(367, 15);
            btnSubmit.Margin = new Padding(4);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(96, 27);
            btnSubmit.TabIndex = 24;
            btnSubmit.Text = "Save flight";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += BtnSubmit_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Location = new Point(3, 193);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(470, 457);
            groupBox1.TabIndex = 39;
            groupBox1.TabStop = false;
            groupBox1.Text = "Flight summary";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(groupBox2, 0, 0);
            tableLayoutPanel2.Controls.Add(groupBox4, 0, 2);
            tableLayoutPanel2.Controls.Add(groupBox6, 0, 1);
            tableLayoutPanel2.Controls.Add(groupBox7, 0, 3);
            tableLayoutPanel2.Location = new Point(6, 24);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 109F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 53F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 108F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(458, 412);
            tableLayoutPanel2.TabIndex = 54;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(lbStartPosition);
            groupBox2.Controls.Add(lbStartIata);
            groupBox2.Controls.Add(lbStartFuel);
            groupBox2.Controls.Add(lbStartTime);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new Point(3, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(452, 101);
            groupBox2.TabIndex = 52;
            groupBox2.TabStop = false;
            groupBox2.Text = "Start";
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
            lbStartPosition.Location = new Point(137, 39);
            lbStartPosition.Name = "lbStartPosition";
            lbStartPosition.Size = new Size(17, 18);
            lbStartPosition.TabIndex = 54;
            lbStartPosition.Text = "?";
            // 
            // lbStartIata
            // 
            lbStartIata.AutoSize = true;
            lbStartIata.Location = new Point(137, 21);
            lbStartIata.Name = "lbStartIata";
            lbStartIata.Size = new Size(44, 18);
            lbStartIata.TabIndex = 53;
            lbStartIata.Text = "????";
            // 
            // lbStartFuel
            // 
            lbStartFuel.AutoSize = true;
            lbStartFuel.Location = new Point(135, 75);
            lbStartFuel.Name = "lbStartFuel";
            lbStartFuel.Size = new Size(44, 18);
            lbStartFuel.TabIndex = 52;
            lbStartFuel.Text = "????";
            lbStartFuel.TextAlign = ContentAlignment.MiddleCenter;
            lbStartFuel.Click += lbStartFuel_Click;
            // 
            // lbStartTime
            // 
            lbStartTime.AutoSize = true;
            lbStartTime.Location = new Point(137, 57);
            lbStartTime.Name = "lbStartTime";
            lbStartTime.Size = new Size(32, 18);
            lbStartTime.TabIndex = 51;
            lbStartTime.Text = "--:--";
            lbStartTime.TextAlign = ContentAlignment.MiddleCenter;
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
            label4.Location = new Point(10, 75);
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
            // groupBox4
            // 
            groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox4.Controls.Add(label1);
            groupBox4.Controls.Add(label14);
            groupBox4.Controls.Add(label16);
            groupBox4.Controls.Add(label17);
            groupBox4.Controls.Add(lbEndPosition);
            groupBox4.Controls.Add(lbEndIata);
            groupBox4.Controls.Add(lbEndFuel);
            groupBox4.Controls.Add(lbEndTime);
            groupBox4.Location = new Point(3, 165);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(452, 102);
            groupBox4.TabIndex = 53;
            groupBox4.TabStop = false;
            groupBox4.Text = "End";
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
            lbEndPosition.Location = new Point(137, 39);
            lbEndPosition.Name = "lbEndPosition";
            lbEndPosition.Size = new Size(17, 18);
            lbEndPosition.TabIndex = 55;
            lbEndPosition.Text = "?";
            // 
            // lbEndIata
            // 
            lbEndIata.AutoSize = true;
            lbEndIata.Location = new Point(137, 21);
            lbEndIata.Name = "lbEndIata";
            lbEndIata.Size = new Size(44, 18);
            lbEndIata.TabIndex = 54;
            lbEndIata.Text = "????";
            // 
            // lbEndFuel
            // 
            lbEndFuel.AutoSize = true;
            lbEndFuel.Location = new Point(134, 75);
            lbEndFuel.Name = "lbEndFuel";
            lbEndFuel.Size = new Size(44, 18);
            lbEndFuel.TabIndex = 53;
            lbEndFuel.Text = "????";
            lbEndFuel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbEndTime
            // 
            lbEndTime.AutoSize = true;
            lbEndTime.Location = new Point(135, 57);
            lbEndTime.Name = "lbEndTime";
            lbEndTime.Size = new Size(32, 18);
            lbEndTime.TabIndex = 52;
            lbEndTime.Text = "--:--";
            lbEndTime.TextAlign = ContentAlignment.MiddleCenter;
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
            groupBox6.Size = new Size(452, 45);
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
            lbTimeAirborn.Location = new Point(126, 19);
            lbTimeAirborn.Name = "lbTimeAirborn";
            lbTimeAirborn.Size = new Size(32, 18);
            lbTimeAirborn.TabIndex = 43;
            lbTimeAirborn.Text = "--:--";
            lbTimeAirborn.TextAlign = ContentAlignment.MiddleCenter;
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
            groupBox7.Location = new Point(3, 273);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(452, 136);
            groupBox7.TabIndex = 54;
            groupBox7.TabStop = false;
            groupBox7.Text = "Flight evaluation";
            // 
            // tbCommentaires
            // 
            tbCommentaires.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbCommentaires.Location = new Point(112, 18);
            tbCommentaires.Margin = new Padding(5, 4, 5, 4);
            tbCommentaires.Multiline = true;
            tbCommentaires.Name = "tbCommentaires";
            tbCommentaires.ScrollBars = ScrollBars.Vertical;
            tbCommentaires.Size = new Size(334, 74);
            tbCommentaires.TabIndex = 20;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(10, 42);
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
            label15.Location = new Point(197, 101);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(63, 18);
            label15.TabIndex = 32;
            label15.Text = "Mission";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(20, 21);
            label12.Margin = new Padding(5, 0, 5, 0);
            label12.Name = "label12";
            label12.Size = new Size(83, 18);
            label12.TabIndex = 7;
            label12.Text = "Comments";
            // 
            // cbMission
            // 
            cbMission.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cbMission.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMission.FormattingEnabled = true;
            cbMission.Location = new Point(267, 98);
            cbMission.Name = "cbMission";
            cbMission.Size = new Size(179, 26);
            cbMission.TabIndex = 22;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(20, 101);
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
            cbNote.Location = new Point(112, 98);
            cbNote.MaxDropDownItems = 10;
            cbNote.Name = "cbNote";
            cbNote.Size = new Size(66, 26);
            cbNote.TabIndex = 21;
            cbNote.MouseHover += CbNote_MouseHover;
            // 
            // lbFret
            // 
            lbFret.AutoSize = true;
            lbFret.Location = new Point(24, 111);
            lbFret.Name = "lbFret";
            lbFret.Size = new Size(110, 18);
            lbFret.TabIndex = 43;
            lbFret.Text = "Fret on airport";
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(lbDesignationAvion);
            groupBox3.Controls.Add(tbEndICAO);
            groupBox3.Controls.Add(lbEndICAO);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(lbLibelleAvion);
            groupBox3.Controls.Add(label10);
            groupBox3.Controls.Add(lbPayload);
            groupBox3.Controls.Add(label11);
            groupBox3.Controls.Add(tbCallsign);
            groupBox3.Controls.Add(lbFret);
            groupBox3.Controls.Add(btnSaveSettings);
            groupBox3.Controls.Add(cbImmat);
            groupBox3.Location = new Point(3, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(470, 183);
            groupBox3.TabIndex = 41;
            groupBox3.TabStop = false;
            groupBox3.Text = "Static data";
            // 
            // lbDesignationAvion
            // 
            lbDesignationAvion.AutoSize = true;
            lbDesignationAvion.Location = new Point(264, 53);
            lbDesignationAvion.Name = "lbDesignationAvion";
            lbDesignationAvion.Size = new Size(152, 18);
            lbDesignationAvion.TabIndex = 44;
            lbDesignationAvion.Text = "<no plane selected>";
            // 
            // tbEndICAO
            // 
            tbEndICAO.BackColor = Color.White;
            tbEndICAO.Location = new Point(376, 138);
            tbEndICAO.Margin = new Padding(4);
            tbEndICAO.Name = "tbEndICAO";
            tbEndICAO.ShortcutsEnabled = false;
            tbEndICAO.Size = new Size(63, 25);
            tbEndICAO.TabIndex = 48;
            tbEndICAO.TextAlign = HorizontalAlignment.Right;
            tbEndICAO.TextChanged += tbEndICAO_TextChanged;
            tbEndICAO.MouseHover += tbEndICAO_MouseHover;
            // 
            // lbEndICAO
            // 
            lbEndICAO.AutoSize = true;
            lbEndICAO.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lbEndICAO.Location = new Point(264, 143);
            lbEndICAO.Margin = new Padding(4, 0, 4, 0);
            lbEndICAO.Name = "lbEndICAO";
            lbEndICAO.Size = new Size(104, 16);
            lbEndICAO.TabIndex = 49;
            lbEndICAO.Text = "Opt: End ICAO";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(21, 82);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(156, 18);
            label5.TabIndex = 47;
            label5.Text = "Aircraft Designation :";
            // 
            // lbLibelleAvion
            // 
            lbLibelleAvion.AutoSize = true;
            lbLibelleAvion.Location = new Point(264, 82);
            lbLibelleAvion.Name = "lbLibelleAvion";
            lbLibelleAvion.Size = new Size(126, 18);
            lbLibelleAvion.TabIndex = 46;
            lbLibelleAvion.Text = "Not Yet Available";
            // 
            // lbPayload
            // 
            lbPayload.AutoSize = true;
            lbPayload.Location = new Point(131, 141);
            lbPayload.Name = "lbPayload";
            lbPayload.Size = new Size(126, 18);
            lbPayload.TabIndex = 45;
            lbPayload.Text = "Not Yet Available";
            // 
            // cbImmat
            // 
            cbImmat.DrawMode = DrawMode.OwnerDrawFixed;
            cbImmat.DropDownStyle = ComboBoxStyle.DropDownList;
            cbImmat.FormattingEnabled = true;
            cbImmat.Items.AddRange(new object[] { "none" });
            cbImmat.Location = new Point(148, 50);
            cbImmat.MaxDropDownItems = 20;
            cbImmat.Name = "cbImmat";
            cbImmat.Size = new Size(111, 26);
            cbImmat.TabIndex = 2;
            cbImmat.DrawItem += cbImmat_DrawItem;
            cbImmat.SelectedIndexChanged += CbImmat_SelectedIndexChanged;
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
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { resetFlightToolStripMenuItem, submitFlightToolStripMenuItem, toolStripSeparator1 });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(134, 54);
            // 
            // resetFlightToolStripMenuItem
            // 
            resetFlightToolStripMenuItem.Name = "resetFlightToolStripMenuItem";
            resetFlightToolStripMenuItem.Size = new Size(133, 22);
            resetFlightToolStripMenuItem.Text = "Reset flight";
            resetFlightToolStripMenuItem.Click += resetFlightToolStripMenuItem_Click;
            // 
            // submitFlightToolStripMenuItem
            // 
            submitFlightToolStripMenuItem.Enabled = false;
            submitFlightToolStripMenuItem.Name = "submitFlightToolStripMenuItem";
            submitFlightToolStripMenuItem.Size = new Size(133, 22);
            submitFlightToolStripMenuItem.Text = "Save Flight";
            submitFlightToolStripMenuItem.Click += submitFlightToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(130, 6);
            // 
            // engineStopTimer
            // 
            engineStopTimer.Interval = 1500;
            engineStopTimer.Tick += engineStopTimer_Tick;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox5, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 0);
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.Size = new Size(476, 708);
            tableLayoutPanel1.TabIndex = 42;
            // 
            // groupBox5
            // 
            groupBox5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox5.Controls.Add(btnReset);
            groupBox5.Controls.Add(btnSubmit);
            groupBox5.Location = new Point(3, 657);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(470, 48);
            groupBox5.TabIndex = 42;
            groupBox5.TabStop = false;
            // 
            // FlightRecCtrl
            // 
            AutoScaleDimensions = new SizeF(9F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 128, 0);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(tableLayoutPanel1);
            Font = new System.Drawing.Font("Arial", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Margin = new Padding(5, 4, 5, 4);
            Name = "FlightRecCtrl";
            Size = new Size(490, 711);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.Timer timerConnection;
        private Label label9;
        private Label label10;
        private Label label11;
        private TextBox tbCallsign;
        private Button btnSaveSettings;
        private Button btnSubmit;
        private GroupBox groupBox1;
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
        private GroupBox groupBox4;
        private GroupBox groupBox2;
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
        private ToolStripSeparator toolStripSeparator1;
        private TextBox tbEndICAO;
        private Label lbEndICAO;
        private System.Windows.Forms.Timer engineStopTimer;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox5;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private PictureBox pictureBox1;
    }
}
