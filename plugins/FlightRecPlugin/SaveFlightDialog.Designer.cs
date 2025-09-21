namespace FlightRecPlugin
{
    partial class SaveFlightDialog
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
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            valDepFuel = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            dtDeparture = new System.Windows.Forms.DateTimePicker();
            label2 = new System.Windows.Forms.Label();
            tbDepICAO = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            valArrFuel = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            dtArrival = new System.Windows.Forms.DateTimePicker();
            label5 = new System.Windows.Forms.Label();
            tbArrICAO = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            valCargo = new System.Windows.Forms.NumericUpDown();
            label8 = new System.Windows.Forms.Label();
            cbImmat = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            groupBox4 = new System.Windows.Forms.GroupBox();
            valNote = new System.Windows.Forms.NumericUpDown();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            cbMission = new System.Windows.Forms.ComboBox();
            tbComments = new System.Windows.Forms.TextBox();
            tbSimPlane = new System.Windows.Forms.TextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)valDepFuel).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)valArrFuel).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)valCargo).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)valNote).BeginInit();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSave.Location = new System.Drawing.Point(464, 364);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(75, 23);
            btnSave.TabIndex = 11;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(383, 364);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(valDepFuel);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(dtDeparture);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(tbDepICAO);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new System.Drawing.Point(12, 97);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(263, 114);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Departure";
            // 
            // valDepFuel
            // 
            valDepFuel.Location = new System.Drawing.Point(51, 77);
            valDepFuel.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            valDepFuel.Name = "valDepFuel";
            valDepFuel.Size = new System.Drawing.Size(200, 23);
            valDepFuel.TabIndex = 4;
            valDepFuel.ValueChanged += valDepFuel_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(11, 79);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(35, 15);
            label3.TabIndex = 4;
            label3.Text = "Fuel :";
            // 
            // dtDeparture
            // 
            dtDeparture.CustomFormat = "";
            dtDeparture.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            dtDeparture.Location = new System.Drawing.Point(51, 49);
            dtDeparture.Name = "dtDeparture";
            dtDeparture.ShowUpDown = true;
            dtDeparture.Size = new System.Drawing.Size(200, 23);
            dtDeparture.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(11, 52);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(39, 15);
            label2.TabIndex = 2;
            label2.Text = "Time :";
            // 
            // tbDepICAO
            // 
            tbDepICAO.Location = new System.Drawing.Point(51, 22);
            tbDepICAO.Name = "tbDepICAO";
            tbDepICAO.Size = new System.Drawing.Size(200, 23);
            tbDepICAO.TabIndex = 2;
            tbDepICAO.TextChanged += tbDepICAO_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(35, 15);
            label1.TabIndex = 0;
            label1.Text = "ICAO";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(valArrFuel);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(dtArrival);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(tbArrICAO);
            groupBox2.Controls.Add(label6);
            groupBox2.Location = new System.Drawing.Point(281, 97);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(264, 114);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Arrival";
            // 
            // valArrFuel
            // 
            valArrFuel.Location = new System.Drawing.Point(55, 77);
            valArrFuel.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            valArrFuel.Name = "valArrFuel";
            valArrFuel.Size = new System.Drawing.Size(200, 23);
            valArrFuel.TabIndex = 7;
            valArrFuel.ValueChanged += valArrFuel_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(8, 79);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(35, 15);
            label4.TabIndex = 10;
            label4.Text = "Fuel :";
            // 
            // dtArrival
            // 
            dtArrival.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            dtArrival.Location = new System.Drawing.Point(55, 49);
            dtArrival.Name = "dtArrival";
            dtArrival.ShowUpDown = true;
            dtArrival.Size = new System.Drawing.Size(200, 23);
            dtArrival.TabIndex = 6;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(8, 52);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(39, 15);
            label5.TabIndex = 8;
            label5.Text = "Time :";
            // 
            // tbArrICAO
            // 
            tbArrICAO.Location = new System.Drawing.Point(55, 22);
            tbArrICAO.Name = "tbArrICAO";
            tbArrICAO.Size = new System.Drawing.Size(200, 23);
            tbArrICAO.TabIndex = 5;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(8, 25);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(35, 15);
            label6.TabIndex = 6;
            label6.Text = "ICAO";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tbSimPlane);
            groupBox3.Controls.Add(valCargo);
            groupBox3.Controls.Add(label8);
            groupBox3.Controls.Add(cbImmat);
            groupBox3.Controls.Add(label7);
            groupBox3.Location = new System.Drawing.Point(11, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(534, 79);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Plane";
            // 
            // valCargo
            // 
            valCargo.Location = new System.Drawing.Point(52, 48);
            valCargo.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            valCargo.Name = "valCargo";
            valCargo.Size = new System.Drawing.Size(200, 23);
            valCargo.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(8, 50);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(39, 15);
            label8.TabIndex = 2;
            label8.Text = "Cargo";
            // 
            // cbImmat
            // 
            cbImmat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbImmat.FormattingEnabled = true;
            cbImmat.Location = new System.Drawing.Point(52, 16);
            cbImmat.Name = "cbImmat";
            cbImmat.Size = new System.Drawing.Size(200, 23);
            cbImmat.TabIndex = 0;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(8, 19);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(42, 15);
            label7.TabIndex = 0;
            label7.Text = "Immat";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(valNote);
            groupBox4.Controls.Add(label10);
            groupBox4.Controls.Add(label9);
            groupBox4.Controls.Add(cbMission);
            groupBox4.Controls.Add(tbComments);
            groupBox4.Location = new System.Drawing.Point(11, 217);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(533, 141);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "Comments";
            // 
            // valNote
            // 
            valNote.Location = new System.Drawing.Point(60, 81);
            valNote.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            valNote.Name = "valNote";
            valNote.Size = new System.Drawing.Size(192, 23);
            valNote.TabIndex = 9;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(6, 83);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(33, 15);
            label10.TabIndex = 4;
            label10.Text = "Note";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(6, 112);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(48, 15);
            label9.TabIndex = 2;
            label9.Text = "Mission";
            // 
            // cbMission
            // 
            cbMission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbMission.FormattingEnabled = true;
            cbMission.Location = new System.Drawing.Point(60, 109);
            cbMission.Name = "cbMission";
            cbMission.Size = new System.Drawing.Size(192, 23);
            cbMission.TabIndex = 10;
            // 
            // tbComments
            // 
            tbComments.Location = new System.Drawing.Point(5, 22);
            tbComments.Multiline = true;
            tbComments.Name = "tbComments";
            tbComments.Size = new System.Drawing.Size(520, 53);
            tbComments.TabIndex = 8;
            // 
            // tbSimPlane
            // 
            tbSimPlane.Location = new System.Drawing.Point(270, 16);
            tbSimPlane.Multiline = true;
            tbSimPlane.Name = "tbSimPlane";
            tbSimPlane.ReadOnly = true;
            tbSimPlane.Size = new System.Drawing.Size(258, 55);
            tbSimPlane.TabIndex = 3;
            // 
            // SaveFlightDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(551, 399);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Name = "SaveFlightDialog";
            Text = "Save flight";
            Load += SaveFlightDialog_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)valDepFuel).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)valArrFuel).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)valCargo).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)valNote).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown valDepFuel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtDeparture;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDepICAO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown valArrFuel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtArrival;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbArrICAO;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown valCargo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbImmat;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbMission;
        private System.Windows.Forms.TextBox tbComments;
        private System.Windows.Forms.NumericUpDown valNote;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbSimPlane;
    }
}