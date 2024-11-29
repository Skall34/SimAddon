
namespace ATISPlugin
{
    partial class ATISCtrl
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
            splitContainer1 = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            tbATISText = new TextBox();
            cbICAO = new ComboBox();
            btnRequest = new Button();
            ledBulb1 = new SimAddonControls.LedBulb();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = Color.FromArgb(64, 64, 64);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new Size(534, 714);
            splitContainer1.SplitterDistance = 211;
            splitContainer1.SplitterWidth = 6;
            splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tableLayoutPanel1.Controls.Add(tbATISText, 0, 1);
            tableLayoutPanel1.Controls.Add(cbICAO, 1, 0);
            tableLayoutPanel1.Controls.Add(btnRequest, 2, 0);
            tableLayoutPanel1.Controls.Add(ledBulb1, 2, 2);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Size = new Size(534, 211);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tbATISText
            // 
            tbATISText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(tbATISText, 3);
            tbATISText.Location = new Point(4, 44);
            tbATISText.Margin = new Padding(4);
            tbATISText.Multiline = true;
            tbATISText.Name = "tbATISText";
            tbATISText.Size = new Size(526, 133);
            tbATISText.TabIndex = 0;
            // 
            // cbICAO
            // 
            cbICAO.Dock = DockStyle.Fill;
            cbICAO.FormattingEnabled = true;
            cbICAO.Location = new Point(184, 4);
            cbICAO.Margin = new Padding(4);
            cbICAO.Name = "cbICAO";
            cbICAO.Size = new Size(256, 29);
            cbICAO.TabIndex = 1;
            cbICAO.SelectedIndexChanged += cbICAO_SelectedIndexChanged;
            // 
            // btnRequest
            // 
            btnRequest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRequest.ForeColor = Color.Black;
            btnRequest.Location = new Point(448, 4);
            btnRequest.Margin = new Padding(4);
            btnRequest.Name = "btnRequest";
            btnRequest.Size = new Size(82, 32);
            btnRequest.TabIndex = 0;
            btnRequest.Text = "Request";
            btnRequest.UseVisualStyleBackColor = true;
            btnRequest.Click += button1_Click;
            // 
            // ledBulb1
            // 
            ledBulb1.Dock = DockStyle.Fill;
            ledBulb1.Location = new Point(448, 185);
            ledBulb1.Margin = new Padding(4);
            ledBulb1.Name = "ledBulb1";
            ledBulb1.On = false;
            ledBulb1.Size = new Size(82, 22);
            ledBulb1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.ForeColor = Color.Black;
            label1.Location = new Point(4, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(172, 40);
            label1.TabIndex = 4;
            label1.Text = "Available ATIS in range";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.ForeColor = Color.Black;
            label2.Location = new Point(183, 181);
            label2.Name = "label2";
            label2.Size = new Size(258, 30);
            label2.TabIndex = 5;
            label2.Text = "Avionics";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ATISCtrl
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "ATISCtrl";
            Size = new Size(534, 714);
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }



        #endregion

        private SplitContainer splitContainer1;
        private Button btnRequest;
        private ComboBox cbICAO;
        private SimAddonControls.LedBulb ledBulb1;
        private ComboBox cbVoices;
        private TextBox tbATISText;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label2;
    }
}
