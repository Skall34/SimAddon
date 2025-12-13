namespace BushTripPlugin
{
    partial class BushtripCreator
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
            btnOK = new Button();
            btnCancel = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            chkSea = new CheckBox();
            chkHeli = new CheckBox();
            chkSmall = new CheckBox();
            chkMedium = new CheckBox();
            label2 = new Label();
            chkLarge = new CheckBox();
            checkMultiHop = new CheckBox();
            btnRandomArrival = new Button();
            comboBox2 = new ComboBox();
            label1 = new Label();
            btnRandomDeparture = new Button();
            comboBox1 = new ComboBox();
            lblDeparture = new Label();
            groupBox2 = new GroupBox();
            lbArrivals = new ListBox();
            groupBox3 = new GroupBox();
            btnSearchFlights = new Button();
            dateTimePicker1 = new DateTimePicker();
            lblFlightTime = new Label();
            lblGndSpeed = new Label();
            trackBar1 = new TrackBar();
            lblSpeed = new Label();
            progressBar1 = new ProgressBar();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.Enabled = false;
            btnOK.Location = new Point(564, 527);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += button1_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(483, 527);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 1);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 166F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(627, 509);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkSea);
            groupBox1.Controls.Add(chkHeli);
            groupBox1.Controls.Add(chkSmall);
            groupBox1.Controls.Add(chkMedium);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(chkLarge);
            groupBox1.Controls.Add(checkMultiHop);
            groupBox1.Controls.Add(btnRandomArrival);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnRandomDeparture);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(lblDeparture);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(621, 160);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Departure";
            // 
            // chkSea
            // 
            chkSea.AutoSize = true;
            chkSea.Checked = true;
            chkSea.CheckState = CheckState.Checked;
            chkSea.Location = new Point(372, 119);
            chkSea.Name = "chkSea";
            chkSea.Size = new Size(73, 19);
            chkSea.TabIndex = 12;
            chkSea.Text = "SeaPlane";
            chkSea.UseVisualStyleBackColor = true;
            // 
            // chkHeli
            // 
            chkHeli.AutoSize = true;
            chkHeli.Checked = true;
            chkHeli.CheckState = CheckState.Checked;
            chkHeli.Location = new Point(301, 118);
            chkHeli.Name = "chkHeli";
            chkHeli.Size = new Size(69, 19);
            chkHeli.TabIndex = 11;
            chkHeli.Text = "Heliport";
            chkHeli.UseVisualStyleBackColor = true;
            // 
            // chkSmall
            // 
            chkSmall.AutoSize = true;
            chkSmall.Checked = true;
            chkSmall.CheckState = CheckState.Checked;
            chkSmall.Location = new Point(240, 118);
            chkSmall.Name = "chkSmall";
            chkSmall.Size = new Size(55, 19);
            chkSmall.TabIndex = 10;
            chkSmall.Text = "Small";
            chkSmall.UseVisualStyleBackColor = true;
            // 
            // chkMedium
            // 
            chkMedium.AutoSize = true;
            chkMedium.Checked = true;
            chkMedium.CheckState = CheckState.Checked;
            chkMedium.Location = new Point(163, 118);
            chkMedium.Name = "chkMedium";
            chkMedium.Size = new Size(71, 19);
            chkMedium.TabIndex = 9;
            chkMedium.Text = "Medium";
            chkMedium.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 119);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 8;
            label2.Text = "Airport size";
            // 
            // chkLarge
            // 
            chkLarge.AutoSize = true;
            chkLarge.Checked = true;
            chkLarge.CheckState = CheckState.Checked;
            chkLarge.Location = new Point(102, 118);
            chkLarge.Name = "chkLarge";
            chkLarge.Size = new Size(55, 19);
            chkLarge.TabIndex = 7;
            chkLarge.Text = "Large";
            chkLarge.UseVisualStyleBackColor = true;
            // 
            // checkMultiHop
            // 
            checkMultiHop.AutoSize = true;
            checkMultiHop.Location = new Point(11, 50);
            checkMultiHop.Name = "checkMultiHop";
            checkMultiHop.Size = new Size(99, 19);
            checkMultiHop.TabIndex = 6;
            checkMultiHop.Text = "Multi hop trip";
            checkMultiHop.UseVisualStyleBackColor = true;
            checkMultiHop.CheckedChanged += checkMultiHop_CheckedChanged;
            // 
            // btnRandomArrival
            // 
            btnRandomArrival.Enabled = false;
            btnRandomArrival.Location = new Point(541, 75);
            btnRandomArrival.Name = "btnRandomArrival";
            btnRandomArrival.Size = new Size(75, 23);
            btnRandomArrival.TabIndex = 5;
            btnRandomArrival.Text = "Random";
            btnRandomArrival.UseVisualStyleBackColor = true;
            btnRandomArrival.Click += btnRandomArrival_Click;
            // 
            // comboBox2
            // 
            comboBox2.CausesValidation = false;
            comboBox2.Enabled = false;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(102, 75);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(433, 23);
            comboBox2.TabIndex = 4;
            comboBox2.KeyPress += comboBox_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 78);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 3;
            label1.Text = "Arrival ICAO";
            label1.Click += label1_Click;
            // 
            // btnRandomDeparture
            // 
            btnRandomDeparture.Location = new Point(541, 16);
            btnRandomDeparture.Name = "btnRandomDeparture";
            btnRandomDeparture.Size = new Size(75, 23);
            btnRandomDeparture.TabIndex = 2;
            btnRandomDeparture.Text = "Random";
            btnRandomDeparture.UseVisualStyleBackColor = true;
            btnRandomDeparture.Click += btnRandomDeparture_Click;
            // 
            // comboBox1
            // 
            comboBox1.CausesValidation = false;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(102, 16);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(433, 23);
            comboBox1.TabIndex = 1;
            comboBox1.KeyPress += comboBox_KeyPress;
            // 
            // lblDeparture
            // 
            lblDeparture.AutoSize = true;
            lblDeparture.Location = new Point(6, 19);
            lblDeparture.Name = "lblDeparture";
            lblDeparture.Size = new Size(90, 15);
            lblDeparture.TabIndex = 0;
            lblDeparture.Text = "Departure ICAO";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lbArrivals);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 265);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(621, 241);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Possible arrivals";
            groupBox2.Enter += groupBox2_Enter;
            // 
            // lbArrivals
            // 
            lbArrivals.Dock = DockStyle.Fill;
            lbArrivals.FormattingEnabled = true;
            lbArrivals.ItemHeight = 15;
            lbArrivals.Location = new Point(3, 19);
            lbArrivals.Name = "lbArrivals";
            lbArrivals.Size = new Size(615, 219);
            lbArrivals.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnSearchFlights);
            groupBox3.Controls.Add(dateTimePicker1);
            groupBox3.Controls.Add(lblFlightTime);
            groupBox3.Controls.Add(lblGndSpeed);
            groupBox3.Controls.Add(trackBar1);
            groupBox3.Controls.Add(lblSpeed);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 169);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(621, 90);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Flight parameters";
            // 
            // btnSearchFlights
            // 
            btnSearchFlights.Location = new Point(510, 19);
            btnSearchFlights.Name = "btnSearchFlights";
            btnSearchFlights.Size = new Size(105, 53);
            btnSearchFlights.TabIndex = 6;
            btnSearchFlights.Text = "Search flights";
            btnSearchFlights.UseVisualStyleBackColor = true;
            btnSearchFlights.Click += button1_Click_1;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.Location = new Point(138, 49);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.Size = new Size(83, 23);
            dateTimePicker1.TabIndex = 4;
            dateTimePicker1.Value = new DateTime(2025, 1, 29, 1, 30, 0, 0);
            // 
            // lblFlightTime
            // 
            lblFlightTime.AutoSize = true;
            lblFlightTime.Location = new Point(6, 49);
            lblFlightTime.Name = "lblFlightTime";
            lblFlightTime.Size = new Size(64, 15);
            lblFlightTime.TabIndex = 3;
            lblFlightTime.Text = "Flight time";
            // 
            // lblGndSpeed
            // 
            lblGndSpeed.AutoSize = true;
            lblGndSpeed.Location = new Point(324, 19);
            lblGndSpeed.Name = "lblGndSpeed";
            lblGndSpeed.Size = new Size(22, 15);
            lblGndSpeed.TabIndex = 2;
            lblGndSpeed.Text = "---";
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(138, 19);
            trackBar1.Maximum = 500;
            trackBar1.Minimum = 50;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(180, 45);
            trackBar1.SmallChange = 10;
            trackBar1.TabIndex = 1;
            trackBar1.TickFrequency = 100;
            trackBar1.Value = 180;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new Point(6, 19);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(126, 15);
            lblSpeed.TabIndex = 0;
            lblSpeed.Text = "Average ground speed";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(11, 527);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(449, 23);
            progressBar1.TabIndex = 3;
            // 
            // BushtripCreator
            // 
            AcceptButton = btnSearchFlights;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(128, 64, 0);
            CancelButton = btnCancel;
            ClientSize = new Size(651, 562);
            ControlBox = false;
            Controls.Add(progressBar1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "BushtripCreator";
            Text = "Bushtrip Creator";
            Load += BushtripCreator_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnOK;
        private Button btnCancel;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private ComboBox comboBox1;
        private Label lblDeparture;
        private ListBox lbArrivals;
        private TrackBar trackBar1;
        private Label lblSpeed;
        private Label lblGndSpeed;
        private Label lblFlightTime;
        private DateTimePicker dateTimePicker1;
        private Button btnRandomDeparture;
        private Button btnRandomArrival;
        private ComboBox comboBox2;
        private Label label1;
        private CheckBox checkMultiHop;
        private Button btnSearchFlights;
        private ProgressBar progressBar1;
        private CheckBox chkSea;
        private CheckBox chkHeli;
        private CheckBox chkSmall;
        private CheckBox chkMedium;
        private Label label2;
        private CheckBox chkLarge;
    }
}