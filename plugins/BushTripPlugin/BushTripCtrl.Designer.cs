namespace BushTripPlugin
{
    partial class BushTripCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BushTripCtrl));
            btnImportFlightPLan = new Button();
            statusStrip1 = new StatusStrip();
            tsGlobalStatus = new ToolStripStatusLabel();
            splitContainer1 = new SplitContainer();
            lvWaypoints = new ListView();
            ColWaypoint = new ColumnHeader();
            ColName = new ColumnHeader();
            ColRoute = new ColumnHeader();
            ColDistance = new ColumnHeader();
            imageList1 = new ImageList(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            tbComment = new TextBox();
            panel1 = new Panel();
            label1 = new Label();
            ledBulb1 = new SimAddonControls.LedBulb();
            compas1 = new SimAddonControls.Compas();
            lblDistanceTotale = new Label();
            btnSaveFlightPlan = new Button();
            btnReset = new Button();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnImportFlightPLan
            // 
            btnImportFlightPLan.Location = new Point(3, 12);
            btnImportFlightPLan.Name = "btnImportFlightPLan";
            btnImportFlightPLan.Size = new Size(108, 38);
            btnImportFlightPLan.TabIndex = 0;
            btnImportFlightPLan.Text = "Import flight plan";
            btnImportFlightPLan.UseVisualStyleBackColor = true;
            btnImportFlightPLan.Click += btnImportFlightPLan_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { tsGlobalStatus });
            statusStrip1.Location = new Point(0, 658);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(521, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // tsGlobalStatus
            // 
            tsGlobalStatus.Name = "tsGlobalStatus";
            tsGlobalStatus.Size = new Size(99, 17);
            tsGlobalStatus.Text = "Load a flight plan";
            tsGlobalStatus.Click += toolStripStatusLabel1_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(3, 56);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lvWaypoints);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel1);
            splitContainer1.Size = new Size(515, 599);
            splitContainer1.SplitterDistance = 259;
            splitContainer1.TabIndex = 3;
            // 
            // lvWaypoints
            // 
            lvWaypoints.AutoArrange = false;
            lvWaypoints.Columns.AddRange(new ColumnHeader[] { ColWaypoint, ColName, ColRoute, ColDistance });
            lvWaypoints.Dock = DockStyle.Fill;
            lvWaypoints.Font = new Font("Segoe Script", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lvWaypoints.FullRowSelect = true;
            lvWaypoints.GridLines = true;
            lvWaypoints.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvWaypoints.Location = new Point(0, 0);
            lvWaypoints.MultiSelect = false;
            lvWaypoints.Name = "lvWaypoints";
            lvWaypoints.Size = new Size(515, 259);
            lvWaypoints.SmallImageList = imageList1;
            lvWaypoints.TabIndex = 2;
            lvWaypoints.UseCompatibleStateImageBehavior = false;
            lvWaypoints.View = View.Details;
            lvWaypoints.MouseDoubleClick += lvWaypoints_MouseDoubleClick;
            // 
            // ColWaypoint
            // 
            ColWaypoint.Text = "Waypoint";
            ColWaypoint.Width = 120;
            // 
            // ColName
            // 
            ColName.Text = "Name         ";
            ColName.TextAlign = HorizontalAlignment.Center;
            ColName.Width = 200;
            // 
            // ColRoute
            // 
            ColRoute.Text = "Route";
            ColRoute.TextAlign = HorizontalAlignment.Center;
            // 
            // ColDistance
            // 
            ColDistance.Text = "Distance (nm)";
            ColDistance.TextAlign = HorizontalAlignment.Center;
            ColDistance.Width = 120;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "AIRPORT");
            imageList1.Images.SetKeyName(1, "NDB");
            imageList1.Images.SetKeyName(2, "USER");
            imageList1.Images.SetKeyName(3, "VOR");
            imageList1.Images.SetKeyName(4, "VORDME");
            imageList1.Images.SetKeyName(5, "WAYPOINT");
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(tbComment, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(515, 336);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tbComment
            // 
            tbComment.Dock = DockStyle.Fill;
            tbComment.Font = new Font("Segoe Script", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbComment.Location = new Point(3, 3);
            tbComment.Multiline = true;
            tbComment.Name = "tbComment";
            tbComment.ScrollBars = ScrollBars.Both;
            tbComment.Size = new Size(354, 330);
            tbComment.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(ledBulb1);
            panel1.Controls.Add(compas1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(363, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(149, 330);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(39, 305);
            label1.Name = "label1";
            label1.Size = new Size(52, 15);
            label1.TabIndex = 3;
            label1.Text = "Avionics";
            // 
            // ledBulb1
            // 
            ledBulb1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ledBulb1.Location = new Point(3, 300);
            ledBulb1.Name = "ledBulb1";
            ledBulb1.On = false;
            ledBulb1.Size = new Size(25, 25);
            ledBulb1.TabIndex = 2;
            // 
            // compas1
            // 
            compas1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            compas1.Headings = new int[]
    {
    0
    };
            compas1.LabelText = "Compas";
            compas1.Location = new Point(0, 0);
            compas1.Name = "compas1";
            compas1.NbNeedles = 1;
            compas1.NeedleImages = new Image[]
    {
    (Image)resources.GetObject("compas1.NeedleImages")
    };
            compas1.NumericValue = 0D;
            compas1.RectangleSize = new Size(80, 20);
            compas1.Size = new Size(139, 142);
            compas1.TabIndex = 1;
            compas1.Unit = "NM";
            // 
            // lblDistanceTotale
            // 
            lblDistanceTotale.AutoSize = true;
            lblDistanceTotale.Location = new Point(231, 24);
            lblDistanceTotale.Name = "lblDistanceTotale";
            lblDistanceTotale.Size = new Size(99, 15);
            lblDistanceTotale.TabIndex = 5;
            lblDistanceTotale.Text = "Load a flight plan";
            lblDistanceTotale.Click += lblDistanceTotale_Click;
            // 
            // btnSaveFlightPlan
            // 
            btnSaveFlightPlan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveFlightPlan.Location = new Point(410, 12);
            btnSaveFlightPlan.Name = "btnSaveFlightPlan";
            btnSaveFlightPlan.Size = new Size(108, 38);
            btnSaveFlightPlan.TabIndex = 6;
            btnSaveFlightPlan.Text = "Export flight plan";
            btnSaveFlightPlan.UseVisualStyleBackColor = true;
            btnSaveFlightPlan.Click += btnSaveFlightPlan_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(117, 12);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(108, 38);
            btnReset.TabIndex = 7;
            btnReset.Text = "Restart trip";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += button1_Click;
            // 
            // BushTripCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(128, 64, 0);
            Controls.Add(btnReset);
            Controls.Add(btnSaveFlightPlan);
            Controls.Add(lblDistanceTotale);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            Controls.Add(btnImportFlightPLan);
            ForeColor = Color.White;
            Name = "BushTripCtrl";
            Size = new Size(521, 680);
            Load += BushTripCtrl_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnImportFlightPLan;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tsGlobalStatus;
        private SplitContainer splitContainer1;
        private TextBox tbComment;
        private Label lblDistanceTotale;
        private ListView lvWaypoints;
        private ColumnHeader ColWaypoint;
        private ColumnHeader ColRoute;
        private ColumnHeader ColDistance;
        private ColumnHeader ColName;
        private Button btnSaveFlightPlan;
        private ImageList imageList1;
        private Button btnReset;
        private TableLayoutPanel tableLayoutPanel1;
        private SimAddonControls.Compas compas1;
        private Panel panel1;
        private Label label1;
        private SimAddonControls.LedBulb ledBulb1;
    }
}
