
namespace CommPlugin
{
    partial class CommCtrl
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
            btnChangeCOM1Freq = new Button();
            lblCom1 = new Label();
            lblComStdby = new Label();
            lblSquawk1 = new Label();
            lblSquawk2 = new Label();
            lblSquawk3 = new Label();
            lblSquawk4 = new Label();
            rotaryKnob1 = new SimAddonControls.RotaryKnob();
            rotaryKnob2 = new SimAddonControls.RotaryKnob();
            rotaryKnobS1 = new SimAddonControls.RotaryKnob();
            rotaryKnobS2 = new SimAddonControls.RotaryKnob();
            rotaryKnobS3 = new SimAddonControls.RotaryKnob();
            rotaryKnobS4 = new SimAddonControls.RotaryKnob();
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            rotaryKnobMode = new SimAddonControls.RotaryKnob();
            groupBox2 = new GroupBox();
            label1 = new Label();
            btnUnicom = new Button();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btnChangeCOM1Freq
            // 
            btnChangeCOM1Freq.BackColor = Color.Gray;
            btnChangeCOM1Freq.FlatStyle = FlatStyle.Flat;
            btnChangeCOM1Freq.Location = new Point(122, 53);
            btnChangeCOM1Freq.Name = "btnChangeCOM1Freq";
            btnChangeCOM1Freq.Size = new Size(42, 32);
            btnChangeCOM1Freq.TabIndex = 3;
            btnChangeCOM1Freq.Text = "<->";
            btnChangeCOM1Freq.UseVisualStyleBackColor = false;
            btnChangeCOM1Freq.Click += btnChangeCOM1Freq_Click;
            // 
            // lblCom1
            // 
            lblCom1.AutoSize = true;
            lblCom1.BackColor = Color.FromArgb(64, 0, 0);
            lblCom1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCom1.ForeColor = Color.Red;
            lblCom1.Location = new Point(17, 48);
            lblCom1.Margin = new Padding(3);
            lblCom1.Name = "lblCom1";
            lblCom1.Padding = new Padding(5);
            lblCom1.Size = new Size(89, 42);
            lblCom1.TabIndex = 4;
            lblCom1.Text = "---.---";
            // 
            // lblComStdby
            // 
            lblComStdby.AutoSize = true;
            lblComStdby.BackColor = Color.FromArgb(64, 0, 0);
            lblComStdby.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblComStdby.ForeColor = Color.Red;
            lblComStdby.Location = new Point(182, 48);
            lblComStdby.Margin = new Padding(5);
            lblComStdby.Name = "lblComStdby";
            lblComStdby.Padding = new Padding(5);
            lblComStdby.Size = new Size(89, 42);
            lblComStdby.TabIndex = 5;
            lblComStdby.Text = "---.---";
            // 
            // lblSquawk1
            // 
            lblSquawk1.AutoSize = true;
            lblSquawk1.BackColor = Color.FromArgb(64, 0, 0);
            lblSquawk1.Font = new Font("Segoe UI", 28F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSquawk1.ForeColor = Color.Red;
            lblSquawk1.Location = new Point(6, 3);
            lblSquawk1.Name = "lblSquawk1";
            lblSquawk1.Padding = new Padding(3);
            lblSquawk1.Size = new Size(43, 52);
            lblSquawk1.TabIndex = 6;
            lblSquawk1.Text = "-";
            // 
            // lblSquawk2
            // 
            lblSquawk2.AutoSize = true;
            lblSquawk2.BackColor = Color.FromArgb(64, 0, 0);
            lblSquawk2.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSquawk2.ForeColor = Color.Red;
            lblSquawk2.Location = new Point(68, 3);
            lblSquawk2.Name = "lblSquawk2";
            lblSquawk2.Padding = new Padding(3);
            lblSquawk2.Size = new Size(43, 52);
            lblSquawk2.TabIndex = 7;
            lblSquawk2.Text = "-";
            // 
            // lblSquawk3
            // 
            lblSquawk3.AutoSize = true;
            lblSquawk3.BackColor = Color.FromArgb(64, 0, 0);
            lblSquawk3.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSquawk3.ForeColor = Color.Red;
            lblSquawk3.Location = new Point(130, 3);
            lblSquawk3.Name = "lblSquawk3";
            lblSquawk3.Padding = new Padding(3);
            lblSquawk3.Size = new Size(43, 52);
            lblSquawk3.TabIndex = 8;
            lblSquawk3.Text = "-";
            // 
            // lblSquawk4
            // 
            lblSquawk4.AutoSize = true;
            lblSquawk4.BackColor = Color.FromArgb(64, 0, 0);
            lblSquawk4.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSquawk4.ForeColor = Color.Red;
            lblSquawk4.Location = new Point(192, 3);
            lblSquawk4.Name = "lblSquawk4";
            lblSquawk4.Padding = new Padding(3);
            lblSquawk4.Size = new Size(43, 52);
            lblSquawk4.TabIndex = 9;
            lblSquawk4.Text = "-";
            lblSquawk4.Click += label3_Click;
            // 
            // rotaryKnob1
            // 
            rotaryKnob1.BackColor = Color.Transparent;
            rotaryKnob1.Increment = 1;
            rotaryKnob1.Location = new Point(279, 45);
            rotaryKnob1.Maximum = 136;
            rotaryKnob1.Minimum = 118;
            rotaryKnob1.Name = "rotaryKnob1";
            rotaryKnob1.Size = new Size(45, 45);
            rotaryKnob1.TabIndex = 12;
            rotaryKnob1.Value = 118;
            rotaryKnob1.ValueChanged += rotaryKnob1_ValueChanged;
            // 
            // rotaryKnob2
            // 
            rotaryKnob2.BackColor = Color.Transparent;
            rotaryKnob2.Increment = 5;
            rotaryKnob2.Location = new Point(321, 45);
            rotaryKnob2.Maximum = 995;
            rotaryKnob2.Minimum = 0;
            rotaryKnob2.Name = "rotaryKnob2";
            rotaryKnob2.Size = new Size(45, 45);
            rotaryKnob2.TabIndex = 13;
            rotaryKnob2.Value = 8;
            rotaryKnob2.ValueChanged += rotaryKnob2_ValueChanged;
            // 
            // rotaryKnobS1
            // 
            rotaryKnobS1.BackColor = Color.Transparent;
            rotaryKnobS1.Increment = 1;
            rotaryKnobS1.Location = new Point(6, 58);
            rotaryKnobS1.Maximum = 7;
            rotaryKnobS1.Minimum = 0;
            rotaryKnobS1.Name = "rotaryKnobS1";
            rotaryKnobS1.Size = new Size(40, 43);
            rotaryKnobS1.TabIndex = 14;
            rotaryKnobS1.Value = 0;
            rotaryKnobS1.ValueChanged += rotaryKnobS1_ValueChanged;
            // 
            // rotaryKnobS2
            // 
            rotaryKnobS2.BackColor = Color.Transparent;
            rotaryKnobS2.Increment = 1;
            rotaryKnobS2.Location = new Point(68, 58);
            rotaryKnobS2.Maximum = 7;
            rotaryKnobS2.Minimum = 0;
            rotaryKnobS2.Name = "rotaryKnobS2";
            rotaryKnobS2.Size = new Size(40, 43);
            rotaryKnobS2.TabIndex = 15;
            rotaryKnobS2.Value = 0;
            rotaryKnobS2.ValueChanged += rotaryKnobS1_ValueChanged;
            // 
            // rotaryKnobS3
            // 
            rotaryKnobS3.BackColor = Color.Transparent;
            rotaryKnobS3.Increment = 1;
            rotaryKnobS3.Location = new Point(130, 58);
            rotaryKnobS3.Maximum = 7;
            rotaryKnobS3.Minimum = 0;
            rotaryKnobS3.Name = "rotaryKnobS3";
            rotaryKnobS3.Size = new Size(40, 43);
            rotaryKnobS3.TabIndex = 16;
            rotaryKnobS3.Value = 0;
            rotaryKnobS3.ValueChanged += rotaryKnobS1_ValueChanged;
            // 
            // rotaryKnobS4
            // 
            rotaryKnobS4.BackColor = Color.Transparent;
            rotaryKnobS4.Increment = 1;
            rotaryKnobS4.Location = new Point(192, 58);
            rotaryKnobS4.Maximum = 7;
            rotaryKnobS4.Minimum = 0;
            rotaryKnobS4.Name = "rotaryKnobS4";
            rotaryKnobS4.Size = new Size(40, 43);
            rotaryKnobS4.TabIndex = 17;
            rotaryKnobS4.Value = 0;
            rotaryKnobS4.ValueChanged += rotaryKnobS1_ValueChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(103, 141);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(372, 133);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            groupBox1.Text = "Transponder";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(rotaryKnobS4, 3, 1);
            tableLayoutPanel1.Controls.Add(lblSquawk4, 3, 0);
            tableLayoutPanel1.Controls.Add(lblSquawk1, 0, 0);
            tableLayoutPanel1.Controls.Add(rotaryKnobS1, 0, 1);
            tableLayoutPanel1.Controls.Add(lblSquawk2, 1, 0);
            tableLayoutPanel1.Controls.Add(rotaryKnobS2, 1, 1);
            tableLayoutPanel1.Controls.Add(rotaryKnobS3, 2, 1);
            tableLayoutPanel1.Controls.Add(lblSquawk3, 2, 0);
            tableLayoutPanel1.Controls.Add(panel1, 4, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(3);
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(366, 111);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(rotaryKnobMode);
            panel1.Location = new Point(254, 6);
            panel1.Name = "panel1";
            tableLayoutPanel1.SetRowSpan(panel1, 2);
            panel1.Size = new Size(104, 99);
            panel1.TabIndex = 19;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(9, 80);
            label5.Name = "label5";
            label5.Size = new Size(27, 15);
            label5.TabIndex = 23;
            label5.Text = "Test";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(78, 80);
            label4.Name = "label4";
            label4.Size = new Size(23, 15);
            label4.TabIndex = 22;
            label4.Text = "On";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(64, 11);
            label3.Name = "label3";
            label3.Size = new Size(37, 15);
            label3.TabIndex = 21;
            label3.Text = "Stdby";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(9, 11);
            label2.Name = "label2";
            label2.Size = new Size(24, 15);
            label2.TabIndex = 20;
            label2.Text = "Off";
            label2.Click += Off;
            // 
            // rotaryKnobMode
            // 
            rotaryKnobMode.BackColor = Color.Transparent;
            rotaryKnobMode.Increment = 1;
            rotaryKnobMode.Location = new Point(33, 34);
            rotaryKnobMode.Maximum = 3;
            rotaryKnobMode.Minimum = 0;
            rotaryKnobMode.Name = "rotaryKnobMode";
            rotaryKnobMode.Size = new Size(40, 43);
            rotaryKnobMode.TabIndex = 18;
            rotaryKnobMode.Value = 0;
            rotaryKnobMode.ValueChanged += rotaryKnobMode_ValueChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(btnUnicom);
            groupBox2.Controls.Add(rotaryKnob2);
            groupBox2.Controls.Add(rotaryKnob1);
            groupBox2.Controls.Add(lblComStdby);
            groupBox2.Controls.Add(lblCom1);
            groupBox2.Controls.Add(btnChangeCOM1Freq);
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(103, 16);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(372, 119);
            groupBox2.TabIndex = 19;
            groupBox2.TabStop = false;
            groupBox2.Text = "COM1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(281, 19);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 15;
            label1.Text = "Frequency";
            // 
            // btnUnicom
            // 
            btnUnicom.BackColor = Color.Gray;
            btnUnicom.FlatStyle = FlatStyle.Flat;
            btnUnicom.Location = new Point(281, 91);
            btnUnicom.Name = "btnUnicom";
            btnUnicom.Size = new Size(85, 23);
            btnUnicom.TabIndex = 14;
            btnUnicom.Text = "Unicom";
            btnUnicom.UseVisualStyleBackColor = false;
            btnUnicom.Click += btnUnicom_Click;
            // 
            // CommCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(64, 64, 64);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "CommCtrl";
            Size = new Size(800, 450);
            Load += CommCtrl_Load;
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }



        #endregion
        private Button btnChangeCOM1Freq;
        private Label lblCom1;
        private Label lblComStdby;
        private Label lblSquawk1;
        private Label lblSquawk2;
        private Label lblSquawk3;
        private Label lblSquawk4;
        private SimAddonControls.RotaryKnob rotaryKnob1;
        private SimAddonControls.RotaryKnob rotaryKnob2;
        private SimAddonControls.RotaryKnob rotaryKnobS1;
        private SimAddonControls.RotaryKnob rotaryKnobS2;
        private SimAddonControls.RotaryKnob rotaryKnobS3;
        private SimAddonControls.RotaryKnob rotaryKnobS4;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnUnicom;
        private Label label1;
        private SimAddonControls.RotaryKnob rotaryKnobMode;
        private Label label2;
        private Panel panel1;
        private Label label5;
        private Label label4;
        private Label label3;
    }
}
