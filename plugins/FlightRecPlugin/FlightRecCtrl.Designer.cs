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
            this.components = new System.ComponentModel.Container();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.timerConnection = new System.Windows.Forms.Timer(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbCallsign = new System.Windows.Forms.TextBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lbStartPosition = new System.Windows.Forms.Label();
            this.lbStartIata = new System.Windows.Forms.Label();
            this.lbStartFuel = new System.Windows.Forms.Label();
            this.lbStartTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lbEndPosition = new System.Windows.Forms.Label();
            this.lbEndIata = new System.Windows.Forms.Label();
            this.lbEndFuel = new System.Windows.Forms.Label();
            this.lbEndTime = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lbOnGround = new System.Windows.Forms.Label();
            this.lbAirborn = new System.Windows.Forms.Label();
            this.lbTimeOnGround = new System.Windows.Forms.Label();
            this.lbTimeAirborn = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tbCommentaires = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cbMission = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbNote = new System.Windows.Forms.ComboBox();
            this.lbFret = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbEndICAO = new System.Windows.Forms.TextBox();
            this.lbEndICAO = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbLibelleAvion = new System.Windows.Forms.Label();
            this.lbPayload = new System.Windows.Forms.Label();
            this.lbDesignationAvion = new System.Windows.Forms.Label();
            this.cbImmat = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetFlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.submitFlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.engineStopTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerMain
            // 
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 141);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 18);
            this.label9.TabIndex = 24;
            this.label9.Text = "Payload (Kg) :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 53);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 18);
            this.label10.TabIndex = 31;
            this.label10.Text = "Aircraft";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 21);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 18);
            this.label11.TabIndex = 33;
            this.label11.Text = "Pilot callsign";
            // 
            // tbCallsign
            // 
            this.tbCallsign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tbCallsign.Location = new System.Drawing.Point(148, 18);
            this.tbCallsign.Margin = new System.Windows.Forms.Padding(4);
            this.tbCallsign.Name = "tbCallsign";
            this.tbCallsign.ShortcutsEnabled = false;
            this.tbCallsign.Size = new System.Drawing.Size(111, 25);
            this.tbCallsign.TabIndex = 0;
            this.tbCallsign.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbCallsign.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Enabled = false;
            this.btnSaveSettings.ForeColor = System.Drawing.Color.Gray;
            this.btnSaveSettings.Location = new System.Drawing.Point(284, 18);
            this.btnSaveSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(58, 25);
            this.btnSaveSettings.TabIndex = 1;
            this.btnSaveSettings.Text = "Apply";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Enabled = false;
            this.btnSubmit.ForeColor = System.Drawing.Color.Black;
            this.btnSubmit.Location = new System.Drawing.Point(367, 15);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(96, 27);
            this.btnSubmit.TabIndex = 24;
            this.btnSubmit.Text = "Save flight";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Location = new System.Drawing.Point(3, 193);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 457);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flight summary";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.groupBox6, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox7, 0, 3);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 24);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(458, 412);
            this.tableLayoutPanel2.TabIndex = 54;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.lbStartPosition);
            this.groupBox2.Controls.Add(this.lbStartIata);
            this.groupBox2.Controls.Add(this.lbStartFuel);
            this.groupBox2.Controls.Add(this.lbStartTime);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(452, 101);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Start";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 39);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(110, 18);
            this.label13.TabIndex = 55;
            this.label13.Text = "Position Name";
            // 
            // lbStartPosition
            // 
            this.lbStartPosition.AutoSize = true;
            this.lbStartPosition.Location = new System.Drawing.Point(137, 39);
            this.lbStartPosition.Name = "lbStartPosition";
            this.lbStartPosition.Size = new System.Drawing.Size(17, 18);
            this.lbStartPosition.TabIndex = 54;
            this.lbStartPosition.Text = "?";
            // 
            // lbStartIata
            // 
            this.lbStartIata.AutoSize = true;
            this.lbStartIata.Location = new System.Drawing.Point(137, 21);
            this.lbStartIata.Name = "lbStartIata";
            this.lbStartIata.Size = new System.Drawing.Size(44, 18);
            this.lbStartIata.TabIndex = 53;
            this.lbStartIata.Text = "????";
            // 
            // lbStartFuel
            // 
            this.lbStartFuel.AutoSize = true;
            this.lbStartFuel.Location = new System.Drawing.Point(137, 75);
            this.lbStartFuel.Name = "lbStartFuel";
            this.lbStartFuel.Size = new System.Drawing.Size(44, 18);
            this.lbStartFuel.TabIndex = 52;
            this.lbStartFuel.Text = "????";
            this.lbStartFuel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbStartFuel.Click += new System.EventHandler(this.lbStartFuel_Click);
            // 
            // lbStartTime
            // 
            this.lbStartTime.AutoSize = true;
            this.lbStartTime.Location = new System.Drawing.Point(137, 57);
            this.lbStartTime.Name = "lbStartTime";
            this.lbStartTime.Size = new System.Drawing.Size(32, 18);
            this.lbStartTime.TabIndex = 51;
            this.lbStartTime.Text = "--:--";
            this.lbStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 21);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 18);
            this.label8.TabIndex = 50;
            this.label8.Text = "Position ICAO";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 75);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 18);
            this.label4.TabIndex = 49;
            this.label4.Text = "Fuel (Kg)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 57);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 18);
            this.label3.TabIndex = 48;
            this.label3.Text = "Time";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.lbEndPosition);
            this.groupBox4.Controls.Add(this.lbEndIata);
            this.groupBox4.Controls.Add(this.lbEndFuel);
            this.groupBox4.Controls.Add(this.lbEndTime);
            this.groupBox4.Location = new System.Drawing.Point(3, 165);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(452, 102);
            this.groupBox4.TabIndex = 53;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "End";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 18);
            this.label1.TabIndex = 59;
            this.label1.Text = "Position Name";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 21);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(106, 18);
            this.label14.TabIndex = 58;
            this.label14.Text = "Position ICAO";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 75);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(73, 18);
            this.label16.TabIndex = 57;
            this.label16.Text = "Fuel (Kg)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 57);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 18);
            this.label17.TabIndex = 56;
            this.label17.Text = "Time";
            // 
            // lbEndPosition
            // 
            this.lbEndPosition.AutoSize = true;
            this.lbEndPosition.Location = new System.Drawing.Point(137, 39);
            this.lbEndPosition.Name = "lbEndPosition";
            this.lbEndPosition.Size = new System.Drawing.Size(17, 18);
            this.lbEndPosition.TabIndex = 55;
            this.lbEndPosition.Text = "?";
            // 
            // lbEndIata
            // 
            this.lbEndIata.AutoSize = true;
            this.lbEndIata.Location = new System.Drawing.Point(137, 21);
            this.lbEndIata.Name = "lbEndIata";
            this.lbEndIata.Size = new System.Drawing.Size(44, 18);
            this.lbEndIata.TabIndex = 54;
            this.lbEndIata.Text = "????";
            // 
            // lbEndFuel
            // 
            this.lbEndFuel.AutoSize = true;
            this.lbEndFuel.Location = new System.Drawing.Point(137, 75);
            this.lbEndFuel.Name = "lbEndFuel";
            this.lbEndFuel.Size = new System.Drawing.Size(44, 18);
            this.lbEndFuel.TabIndex = 53;
            this.lbEndFuel.Text = "????";
            this.lbEndFuel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbEndTime
            // 
            this.lbEndTime.AutoSize = true;
            this.lbEndTime.Location = new System.Drawing.Point(137, 57);
            this.lbEndTime.Name = "lbEndTime";
            this.lbEndTime.Size = new System.Drawing.Size(32, 18);
            this.lbEndTime.TabIndex = 52;
            this.lbEndTime.Text = "--:--";
            this.lbEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.lbOnGround);
            this.groupBox6.Controls.Add(this.lbAirborn);
            this.groupBox6.Controls.Add(this.lbTimeOnGround);
            this.groupBox6.Controls.Add(this.lbTimeAirborn);
            this.groupBox6.Location = new System.Drawing.Point(3, 112);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(452, 45);
            this.groupBox6.TabIndex = 53;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Time blocks";
            // 
            // lbOnGround
            // 
            this.lbOnGround.AutoSize = true;
            this.lbOnGround.Location = new System.Drawing.Point(186, 21);
            this.lbOnGround.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbOnGround.Name = "lbOnGround";
            this.lbOnGround.Size = new System.Drawing.Size(87, 18);
            this.lbOnGround.TabIndex = 45;
            this.lbOnGround.Text = "On Ground";
            this.lbOnGround.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbAirborn
            // 
            this.lbAirborn.AutoSize = true;
            this.lbAirborn.Location = new System.Drawing.Point(10, 21);
            this.lbAirborn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAirborn.Name = "lbAirborn";
            this.lbAirborn.Size = new System.Drawing.Size(60, 18);
            this.lbAirborn.TabIndex = 42;
            this.lbAirborn.Text = "Airborn";
            this.lbAirborn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTimeOnGround
            // 
            this.lbTimeOnGround.AutoSize = true;
            this.lbTimeOnGround.Location = new System.Drawing.Point(295, 21);
            this.lbTimeOnGround.Name = "lbTimeOnGround";
            this.lbTimeOnGround.Size = new System.Drawing.Size(32, 18);
            this.lbTimeOnGround.TabIndex = 46;
            this.lbTimeOnGround.Text = "--:--";
            this.lbTimeOnGround.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTimeAirborn
            // 
            this.lbTimeAirborn.AutoSize = true;
            this.lbTimeAirborn.Location = new System.Drawing.Point(83, 21);
            this.lbTimeAirborn.Name = "lbTimeAirborn";
            this.lbTimeAirborn.Size = new System.Drawing.Size(32, 18);
            this.lbTimeAirborn.TabIndex = 43;
            this.lbTimeAirborn.Text = "--:--";
            this.lbTimeAirborn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.tbCommentaires);
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Controls.Add(this.cbMission);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.cbNote);
            this.groupBox7.Location = new System.Drawing.Point(3, 273);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(452, 136);
            this.groupBox7.TabIndex = 54;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Flight evaluation";
            // 
            // tbCommentaires
            // 
            this.tbCommentaires.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCommentaires.Location = new System.Drawing.Point(112, 18);
            this.tbCommentaires.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbCommentaires.Multiline = true;
            this.tbCommentaires.Name = "tbCommentaires";
            this.tbCommentaires.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCommentaires.Size = new System.Drawing.Size(334, 74);
            this.tbCommentaires.TabIndex = 20;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(197, 101);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 18);
            this.label15.TabIndex = 32;
            this.label15.Text = "Mission";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 21);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(83, 18);
            this.label12.TabIndex = 7;
            this.label12.Text = "Comments";
            // 
            // cbMission
            // 
            this.cbMission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMission.FormattingEnabled = true;
            this.cbMission.Location = new System.Drawing.Point(267, 98);
            this.cbMission.Name = "cbMission";
            this.cbMission.Size = new System.Drawing.Size(179, 26);
            this.cbMission.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 101);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Evaluation";
            // 
            // cbNote
            // 
            this.cbNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbNote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNote.FormattingEnabled = true;
            this.cbNote.Items.AddRange(new object[] {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10});
            this.cbNote.Location = new System.Drawing.Point(127, 98);
            this.cbNote.MaxDropDownItems = 10;
            this.cbNote.Name = "cbNote";
            this.cbNote.Size = new System.Drawing.Size(66, 26);
            this.cbNote.TabIndex = 21;
            this.cbNote.MouseHover += new System.EventHandler(this.CbNote_MouseHover);
            // 
            // lbFret
            // 
            this.lbFret.AutoSize = true;
            this.lbFret.Location = new System.Drawing.Point(24, 111);
            this.lbFret.Name = "lbFret";
            this.lbFret.Size = new System.Drawing.Size(110, 18);
            this.lbFret.TabIndex = 43;
            this.lbFret.Text = "Fret on airport";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tbEndICAO);
            this.groupBox3.Controls.Add(this.lbEndICAO);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.lbLibelleAvion);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.lbPayload);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.lbDesignationAvion);
            this.groupBox3.Controls.Add(this.tbCallsign);
            this.groupBox3.Controls.Add(this.lbFret);
            this.groupBox3.Controls.Add(this.btnSaveSettings);
            this.groupBox3.Controls.Add(this.cbImmat);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(470, 183);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Static data";
            // 
            // tbEndICAO
            // 
            this.tbEndICAO.BackColor = System.Drawing.Color.White;
            this.tbEndICAO.Location = new System.Drawing.Point(376, 138);
            this.tbEndICAO.Margin = new System.Windows.Forms.Padding(4);
            this.tbEndICAO.Name = "tbEndICAO";
            this.tbEndICAO.ShortcutsEnabled = false;
            this.tbEndICAO.Size = new System.Drawing.Size(63, 25);
            this.tbEndICAO.TabIndex = 48;
            this.tbEndICAO.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbEndICAO.TextChanged += new System.EventHandler(this.tbEndICAO_TextChanged);
            this.tbEndICAO.MouseHover += new System.EventHandler(this.tbEndICAO_MouseHover);
            // 
            // lbEndICAO
            // 
            this.lbEndICAO.AutoSize = true;
            this.lbEndICAO.Font = new System.Drawing.Font("Arial", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEndICAO.Location = new System.Drawing.Point(264, 143);
            this.lbEndICAO.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbEndICAO.Name = "lbEndICAO";
            this.lbEndICAO.Size = new System.Drawing.Size(104, 16);
            this.lbEndICAO.TabIndex = 49;
            this.lbEndICAO.Text = "Opt: End ICAO";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 82);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 18);
            this.label5.TabIndex = 47;
            this.label5.Text = "Aircraft Designation :";
            // 
            // lbLibelleAvion
            // 
            this.lbLibelleAvion.AutoSize = true;
            this.lbLibelleAvion.Location = new System.Drawing.Point(264, 82);
            this.lbLibelleAvion.Name = "lbLibelleAvion";
            this.lbLibelleAvion.Size = new System.Drawing.Size(126, 18);
            this.lbLibelleAvion.TabIndex = 46;
            this.lbLibelleAvion.Text = "Not Yet Available";
            // 
            // lbPayload
            // 
            this.lbPayload.AutoSize = true;
            this.lbPayload.Location = new System.Drawing.Point(131, 141);
            this.lbPayload.Name = "lbPayload";
            this.lbPayload.Size = new System.Drawing.Size(126, 18);
            this.lbPayload.TabIndex = 45;
            this.lbPayload.Text = "Not Yet Available";
            // 
            // lbDesignationAvion
            // 
            this.lbDesignationAvion.AutoSize = true;
            this.lbDesignationAvion.Location = new System.Drawing.Point(264, 53);
            this.lbDesignationAvion.Name = "lbDesignationAvion";
            this.lbDesignationAvion.Size = new System.Drawing.Size(152, 18);
            this.lbDesignationAvion.TabIndex = 44;
            this.lbDesignationAvion.Text = "<no plane selected>";
            // 
            // cbImmat
            // 
            this.cbImmat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbImmat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbImmat.FormattingEnabled = true;
            this.cbImmat.Items.AddRange(new object[] {
            "none"});
            this.cbImmat.Location = new System.Drawing.Point(148, 50);
            this.cbImmat.Name = "cbImmat";
            this.cbImmat.Size = new System.Drawing.Size(109, 26);
            this.cbImmat.TabIndex = 2;
            this.cbImmat.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbImmat_DrawItem);
            this.cbImmat.SelectedIndexChanged += new System.EventHandler(this.CbImmat_SelectedIndexChanged);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.ForeColor = System.Drawing.Color.Black;
            this.btnReset.Location = new System.Drawing.Point(6, 15);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(101, 27);
            this.btnReset.TabIndex = 25;
            this.btnReset.Text = "Reset flight";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetFlightToolStripMenuItem,
            this.submitFlightToolStripMenuItem,
            this.toolStripSeparator1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 76);
            // 
            // resetFlightToolStripMenuItem
            // 
            this.resetFlightToolStripMenuItem.Name = "resetFlightToolStripMenuItem";
            this.resetFlightToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.resetFlightToolStripMenuItem.Text = "Reset flight";
            this.resetFlightToolStripMenuItem.Click += new System.EventHandler(this.resetFlightToolStripMenuItem_Click);
            // 
            // submitFlightToolStripMenuItem
            // 
            this.submitFlightToolStripMenuItem.Enabled = false;
            this.submitFlightToolStripMenuItem.Name = "submitFlightToolStripMenuItem";
            this.submitFlightToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.submitFlightToolStripMenuItem.Text = "Save Flight";
            this.submitFlightToolStripMenuItem.Click += new System.EventHandler(this.submitFlightToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // engineStopTimer
            // 
            this.engineStopTimer.Interval = 3000;
            this.engineStopTimer.Tick += new System.EventHandler(this.engineStopTimer_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(476, 708);
            this.tableLayoutPanel1.TabIndex = 42;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.btnReset);
            this.groupBox5.Controls.Add(this.btnSubmit);
            this.groupBox5.Location = new System.Drawing.Point(3, 657);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(470, 48);
            this.groupBox5.TabIndex = 42;
            this.groupBox5.TabStop = false;
            // 
            // FlightRecCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FlightRecCtrl";
            this.Size = new System.Drawing.Size(490, 711);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}
