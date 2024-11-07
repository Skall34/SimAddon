
namespace MeteoPlugin
{
    partial class MeteoCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeteoCtrl));
            label1 = new Label();
            btnRequest = new Button();
            tbMETAR = new TextBox();
            statusStrip1 = new StatusStrip();
            pictureBox1 = new PictureBox();
            lblDecodedMETAR = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            cbICAO = new ComboBox();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(5);
            label1.Size = new Size(85, 25);
            label1.TabIndex = 0;
            label1.Text = "Airport ICAO";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRequest
            // 
            btnRequest.Location = new Point(253, 3);
            btnRequest.Name = "btnRequest";
            btnRequest.Size = new Size(75, 23);
            btnRequest.TabIndex = 2;
            btnRequest.Text = "Request";
            btnRequest.UseVisualStyleBackColor = true;
            btnRequest.Click += btnRequest_Click;
            // 
            // tbMETAR
            // 
            tbMETAR.Dock = DockStyle.Fill;
            tbMETAR.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbMETAR.Location = new Point(5, 104);
            tbMETAR.Multiline = true;
            tbMETAR.Name = "tbMETAR";
            tbMETAR.ReadOnly = true;
            tbMETAR.Size = new Size(496, 54);
            tbMETAR.TabIndex = 7;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 560);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(512, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 9;
            statusStrip1.Text = "statusStrip1";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(5, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(496, 54);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // lblDecodedMETAR
            // 
            lblDecodedMETAR.AutoSize = true;
            lblDecodedMETAR.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDecodedMETAR.ForeColor = Color.Black;
            lblDecodedMETAR.Location = new Point(10, 10);
            lblDecodedMETAR.Margin = new Padding(10);
            lblDecodedMETAR.Name = "lblDecodedMETAR";
            lblDecodedMETAR.Size = new Size(19, 21);
            lblDecodedMETAR.TabIndex = 12;
            lblDecodedMETAR.Text = "...";
            lblDecodedMETAR.Click += lblDecodedMETAR_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(tbMETAR, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 3);
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(506, 554);
            tableLayoutPanel1.TabIndex = 13;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(btnRequest, 2, 0);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(cbICAO, 1, 0);
            tableLayoutPanel2.Location = new Point(5, 67);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(496, 29);
            tableLayoutPanel2.TabIndex = 11;
            tableLayoutPanel2.Paint += tableLayoutPanel2_Paint;
            // 
            // cbICAO
            // 
            cbICAO.FormattingEnabled = true;
            cbICAO.Location = new Point(153, 3);
            cbICAO.Name = "cbICAO";
            cbICAO.Size = new Size(94, 23);
            cbICAO.TabIndex = 3;
            cbICAO.SelectedIndexChanged += cbICAO_SelectedIndexChanged;
            cbICAO.KeyPress += cbICAO_KeyPress;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightGray;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(lblDecodedMETAR);
            panel1.Dock = DockStyle.Fill;
            panel1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel1.Location = new Point(12, 173);
            panel1.Margin = new Padding(10);
            panel1.Name = "panel1";
            panel1.Size = new Size(482, 369);
            panel1.TabIndex = 12;
            // 
            // MeteoCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Teal;
            Controls.Add(tableLayoutPanel1);
            Controls.Add(statusStrip1);
            Name = "MeteoCtrl";
            Size = new Size(512, 582);
            Load += MeteoCtrl_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private Label label1;
        private Button btnRequest;
        private TextBox tbMETAR;
        private StatusStrip statusStrip1;
        private PictureBox pictureBox1;
        private Label lblDecodedMETAR;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
        private ComboBox cbICAO;
    }
}
