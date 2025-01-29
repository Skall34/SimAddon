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
            btnOK.Location = new Point(564, 415);
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
            btnCancel.Location = new Point(483, 415);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(627, 397);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnRandomDeparture);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(lblDeparture);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(621, 54);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Departure";
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
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.TextChanged += comboBox1_TextChanged;
            comboBox1.KeyPress += comboBox1_KeyPress;
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
            groupBox2.Location = new Point(3, 153);
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
            groupBox3.Location = new Point(3, 63);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(621, 84);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Flight parameters";
            // 
            // btnSearchFlights
            // 
            btnSearchFlights.Location = new Point(513, 19);
            btnSearchFlights.Name = "btnSearchFlights";
            btnSearchFlights.Size = new Size(103, 53);
            btnSearchFlights.TabIndex = 5;
            btnSearchFlights.Text = "Search Flights";
            btnSearchFlights.UseVisualStyleBackColor = true;
            btnSearchFlights.Click += btnSearchFlights_Click;
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
            // BushtripCreator
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(128, 64, 0);
            CancelButton = btnCancel;
            ClientSize = new Size(651, 450);
            ControlBox = false;
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
        private Button btnSearchFlights;
    }
}