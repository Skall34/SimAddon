namespace Calculator
{
    partial class CalculatorCtrl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculatorCtrl));
            groupBox1 = new GroupBox();
            CapaConverter = new Converter();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            FuelConverter = new Converter();
            groupBox3 = new GroupBox();
            DistanceConverter = new Converter();
            groupBox4 = new GroupBox();
            PressureConverter = new Converter();
            groupBox5 = new GroupBox();
            TempConverter = new Converter();
            groupBox6 = new GroupBox();
            SpeedConverter = new Converter();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(CapaConverter);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(794, 68);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Capacity";
            // 
            // CapaConverter
            // 
            CapaConverter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CapaConverter.Location = new Point(6, 22);
            CapaConverter.Name = "CapaConverter";
            CapaConverter.Size = new Size(782, 32);
            CapaConverter.TabIndex = 0;
            CapaConverter.units = (List<string>)resources.GetObject("CapaConverter.units");
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox4, 0, 3);
            tableLayoutPanel1.Controls.Add(groupBox5, 0, 4);
            tableLayoutPanel1.Controls.Add(groupBox6, 0, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666641F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666641F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666641F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666641F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666641F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(FuelConverter);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 77);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(794, 68);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Fuel";
            // 
            // FuelConverter
            // 
            FuelConverter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FuelConverter.Location = new Point(6, 22);
            FuelConverter.Name = "FuelConverter";
            FuelConverter.Size = new Size(782, 30);
            FuelConverter.TabIndex = 1;
            FuelConverter.units = (List<string>)resources.GetObject("FuelConverter.units");
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(DistanceConverter);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 151);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(794, 68);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Distance";
            // 
            // DistanceConverter
            // 
            DistanceConverter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            DistanceConverter.Location = new Point(6, 22);
            DistanceConverter.Name = "DistanceConverter";
            DistanceConverter.Size = new Size(782, 30);
            DistanceConverter.TabIndex = 0;
            DistanceConverter.units = (List<string>)resources.GetObject("DistanceConverter.units");
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(PressureConverter);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 225);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(794, 68);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Pressure";
            // 
            // PressureConverter
            // 
            PressureConverter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PressureConverter.Location = new Point(6, 22);
            PressureConverter.Name = "PressureConverter";
            PressureConverter.Size = new Size(782, 30);
            PressureConverter.TabIndex = 0;
            PressureConverter.units = (List<string>)resources.GetObject("PressureConverter.units");
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(TempConverter);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(3, 299);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(794, 68);
            groupBox5.TabIndex = 5;
            groupBox5.TabStop = false;
            groupBox5.Text = "Temperature";
            // 
            // TempConverter
            // 
            TempConverter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TempConverter.Location = new Point(6, 22);
            TempConverter.Name = "TempConverter";
            TempConverter.Size = new Size(782, 30);
            TempConverter.TabIndex = 0;
            TempConverter.units = (List<string>)resources.GetObject("TempConverter.units");
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(SpeedConverter);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(3, 373);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(794, 68);
            groupBox6.TabIndex = 6;
            groupBox6.TabStop = false;
            groupBox6.Text = "Speed";
            // 
            // SpeedConverter
            // 
            SpeedConverter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SpeedConverter.Location = new Point(6, 22);
            SpeedConverter.Name = "SpeedConverter";
            SpeedConverter.Size = new Size(782, 30);
            SpeedConverter.TabIndex = 0;
            SpeedConverter.units = (List<string>)resources.GetObject("SpeedConverter.units");
            // 
            // CalculatorCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(tableLayoutPanel1);
            Name = "CalculatorCtrl";
            Size = new Size(800, 450);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private Converter CapaConverter;
        private GroupBox groupBox2;
        private Converter FuelConverter;
        private GroupBox groupBox3;
        private Converter DistanceConverter;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private Converter PressureConverter;
        private Converter TempConverter;
        private Converter SpeedConverter;
    }
}
