
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeteoCtrl));
            label1 = new Label();
            btnRequest = new Button();
            tbMETAR = new TextBox();
            pictureBox1 = new PictureBox();
            lblDecodedMETAR = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            cbICAO = new ComboBox();
            panel1 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            compas1 = new SimAddonControls.Compas();
            tableLayoutPanel4 = new TableLayoutPanel();
            lbAirportInfo = new ListBox();
            panel2 = new Panel();
            ttAeroport = new ToolTip(components);
            VariableWindTimer = new System.Windows.Forms.Timer(components);
            VariableWindAnimation = new System.Windows.Forms.Timer(components);
            splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
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
            btnRequest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRequest.Location = new Point(637, 3);
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
            tbMETAR.Location = new Point(12, 104);
            tbMETAR.Margin = new Padding(10, 3, 10, 3);
            tbMETAR.Multiline = true;
            tbMETAR.Name = "tbMETAR";
            tbMETAR.ReadOnly = true;
            tbMETAR.Size = new Size(701, 44);
            tbMETAR.TabIndex = 7;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(5, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(715, 54);
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
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(tbMETAR, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(725, 406);
            tableLayoutPanel1.TabIndex = 13;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 21F));
            tableLayoutPanel2.Controls.Add(btnRequest, 2, 0);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(cbICAO, 1, 0);
            tableLayoutPanel2.Location = new Point(5, 67);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(715, 29);
            tableLayoutPanel2.TabIndex = 11;
            // 
            // cbICAO
            // 
            cbICAO.Dock = DockStyle.Fill;
            cbICAO.FormattingEnabled = true;
            cbICAO.Location = new Point(153, 3);
            cbICAO.MinimumSize = new Size(250, 0);
            cbICAO.Name = "cbICAO";
            cbICAO.Size = new Size(250, 23);
            cbICAO.TabIndex = 3;
            cbICAO.SelectedIndexChanged += cbICAO_SelectedIndexChanged;
            cbICAO.KeyPress += cbICAO_KeyPress;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BackColor = Color.LightGray;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(lblDecodedMETAR);
            panel1.Dock = DockStyle.Fill;
            panel1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panel1.Location = new Point(12, 156);
            panel1.Margin = new Padding(10, 3, 10, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(701, 245);
            panel1.TabIndex = 12;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(compas1, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(725, 281);
            tableLayoutPanel3.TabIndex = 13;
            tableLayoutPanel3.Paint += tableLayoutPanel3_Paint;
            // 
            // compas1
            // 
            compas1.Dock = DockStyle.Fill;
            compas1.Headings = new int[]
    {
    0,
    0
    };
            compas1.LabelText = "Compas";
            compas1.Location = new Point(3, 3);
            compas1.Name = "compas1";
            compas1.NbNeedles = 2;
            compas1.NeedleImages = new Image[]
    {
    (Image)resources.GetObject("compas1.NeedleImages"),
    (Image)resources.GetObject("compas1.NeedleImages1")
    };
            compas1.NumericValue = 0D;
            compas1.RectangleSize = new Size(60, 20);
            compas1.Size = new Size(356, 275);
            compas1.TabIndex = 0;
            compas1.Unit = "NM";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(lbAirportInfo, 0, 0);
            tableLayoutPanel4.Controls.Add(panel2, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(365, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel4.Size = new Size(357, 275);
            tableLayoutPanel4.TabIndex = 1;
            // 
            // lbAirportInfo
            // 
            lbAirportInfo.BackColor = Color.FromArgb(224, 224, 224);
            lbAirportInfo.Dock = DockStyle.Fill;
            lbAirportInfo.DrawMode = DrawMode.OwnerDrawFixed;
            lbAirportInfo.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbAirportInfo.FormattingEnabled = true;
            lbAirportInfo.HorizontalScrollbar = true;
            lbAirportInfo.ItemHeight = 34;
            lbAirportInfo.Items.AddRange(new object[] { "Loading airport database" });
            lbAirportInfo.Location = new Point(3, 3);
            lbAirportInfo.Name = "lbAirportInfo";
            lbAirportInfo.Size = new Size(351, 239);
            lbAirportInfo.TabIndex = 1;
            lbAirportInfo.DrawItem += lbAirportInfo_DrawItem;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.Teal;
            panel2.Location = new Point(0, 245);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(357, 30);
            panel2.TabIndex = 2;
            // 
            // VariableWindTimer
            // 
            VariableWindTimer.Interval = 5000;
            VariableWindTimer.Tick += VariableWindTimer_Tick;
            // 
            // VariableWindAnimation
            // 
            VariableWindAnimation.Tick += VariableWindAnimation_Tick;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
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
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel3);
            splitContainer1.Size = new Size(725, 691);
            splitContainer1.SplitterDistance = 406;
            splitContainer1.TabIndex = 14;
            // 
            // MeteoCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Teal;
            Controls.Add(splitContainer1);
            Name = "MeteoCtrl";
            Size = new Size(725, 691);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }



        #endregion

        private Label label1;
        private Button btnRequest;
        private TextBox tbMETAR;
        private PictureBox pictureBox1;
        private Label lblDecodedMETAR;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
        private ComboBox cbICAO;
        private ToolTip ttAeroport;
        private TableLayoutPanel tableLayoutPanel3;
        private SimAddonControls.Compas compas1;
        private ListBox lbAirportInfo;
        private System.Windows.Forms.Timer VariableWindTimer;
        private System.Windows.Forms.Timer VariableWindAnimation;
        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel panel2;
    }
}
