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
            tbComment = new TextBox();
            lblDistanceTotale = new Label();
            btnSaveFlightPlan = new Button();
            btnReset = new Button();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
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
            statusStrip1.Location = new Point(0, 468);
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
            splitContainer1.Panel2.Controls.Add(tbComment);
            splitContainer1.Size = new Size(515, 409);
            splitContainer1.SplitterDistance = 177;
            splitContainer1.TabIndex = 3;
            // 
            // lvWaypoints
            // 
            lvWaypoints.AutoArrange = false;
            lvWaypoints.Columns.AddRange(new ColumnHeader[] { ColWaypoint, ColName, ColRoute, ColDistance });
            lvWaypoints.Dock = DockStyle.Fill;
            lvWaypoints.FullRowSelect = true;
            lvWaypoints.GridLines = true;
            lvWaypoints.Location = new Point(0, 0);
            lvWaypoints.Name = "lvWaypoints";
            lvWaypoints.Size = new Size(515, 177);
            lvWaypoints.SmallImageList = imageList1;
            lvWaypoints.TabIndex = 2;
            lvWaypoints.UseCompatibleStateImageBehavior = false;
            lvWaypoints.View = View.Details;
            lvWaypoints.MouseDoubleClick += lvWaypoints_MouseDoubleClick;
            // 
            // ColWaypoint
            // 
            ColWaypoint.Text = "Waypoint";
            ColWaypoint.Width = 80;
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
            ColDistance.Text = "Distance";
            ColDistance.TextAlign = HorizontalAlignment.Center;
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
            // tbComment
            // 
            tbComment.Dock = DockStyle.Fill;
            tbComment.Location = new Point(0, 0);
            tbComment.Multiline = true;
            tbComment.Name = "tbComment";
            tbComment.Size = new Size(515, 228);
            tbComment.TabIndex = 0;
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
            Name = "BushTripCtrl";
            Size = new Size(521, 490);
            Load += BushTripCtrl_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
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
    }
}
