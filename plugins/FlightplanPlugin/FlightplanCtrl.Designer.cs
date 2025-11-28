namespace BushTripPlugin
{
    partial class FlightplanCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlightplanCtrl));
            statusStrip1 = new StatusStrip();
            tsGlobalStatus = new ToolStripStatusLabel();
            splitContainer1 = new SplitContainer();
            contextMenuStrip1 = new ContextMenuStrip(components);
            createToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            importToolStripMenuItem = new ToolStripMenuItem();
            exportToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            simbriefToolStripMenuItem = new ToolStripMenuItem();
            createNewFlightplanToolStripMenuItem = new ToolStripMenuItem();
            getLastFlightplanToolStripMenuItem = new ToolStripMenuItem();
            getFlightBriefingToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            restartToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            lblDistanceTotale = new Label();
            lvWaypoints = new ListView();
            ColWaypoint = new ColumnHeader();
            ColName = new ColumnHeader();
            ColRoute = new ColumnHeader();
            ColDistance = new ColumnHeader();
            imageList1 = new ImageList(components);
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            splitContainer2 = new SplitContainer();
            compas1 = new SimAddonControls.Compas();
            splitContainer3 = new SplitContainer();
            tbComment = new TextBox();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
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
            tsGlobalStatus.BackColor = Color.FromArgb(64, 64, 64);
            tsGlobalStatus.Name = "tsGlobalStatus";
            tsGlobalStatus.Size = new Size(99, 17);
            tsGlobalStatus.Text = "Load a flight plan";
            tsGlobalStatus.Click += toolStripStatusLabel1_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = Color.FromArgb(64, 64, 64);
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = Color.FromArgb(64, 64, 64);
            splitContainer1.Panel1.ContextMenuStrip = contextMenuStrip1;
            splitContainer1.Panel1.Controls.Add(lblDistanceTotale);
            splitContainer1.Panel1.Controls.Add(lvWaypoints);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel1);
            splitContainer1.Size = new Size(521, 658);
            splitContainer1.SplitterDistance = 237;
            splitContainer1.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { createToolStripMenuItem, toolStripSeparator1, importToolStripMenuItem, exportToolStripMenuItem, toolStripSeparator4, simbriefToolStripMenuItem, toolStripSeparator2, restartToolStripMenuItem, toolStripSeparator3, settingsToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(119, 160);
            // 
            // createToolStripMenuItem
            // 
            createToolStripMenuItem.Name = "createToolStripMenuItem";
            createToolStripMenuItem.Size = new Size(118, 22);
            createToolStripMenuItem.Text = "Create";
            createToolStripMenuItem.Click += createToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(115, 6);
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.Size = new Size(118, 22);
            importToolStripMenuItem.Text = "Import";
            importToolStripMenuItem.Click += importToolStripMenuItem_Click;
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Enabled = false;
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(118, 22);
            exportToolStripMenuItem.Text = "Export";
            exportToolStripMenuItem.Click += exportToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(115, 6);
            // 
            // simbriefToolStripMenuItem
            // 
            simbriefToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { createNewFlightplanToolStripMenuItem, getLastFlightplanToolStripMenuItem, getFlightBriefingToolStripMenuItem });
            simbriefToolStripMenuItem.Name = "simbriefToolStripMenuItem";
            simbriefToolStripMenuItem.Size = new Size(118, 22);
            simbriefToolStripMenuItem.Text = "Simbrief";
            // 
            // createNewFlightplanToolStripMenuItem
            // 
            createNewFlightplanToolStripMenuItem.Name = "createNewFlightplanToolStripMenuItem";
            createNewFlightplanToolStripMenuItem.Size = new Size(187, 22);
            createNewFlightplanToolStripMenuItem.Text = "Create new flightplan";
            createNewFlightplanToolStripMenuItem.Click += createNewFlightplanToolStripMenuItem_Click;
            // 
            // getLastFlightplanToolStripMenuItem
            // 
            getLastFlightplanToolStripMenuItem.Name = "getLastFlightplanToolStripMenuItem";
            getLastFlightplanToolStripMenuItem.Size = new Size(187, 22);
            getLastFlightplanToolStripMenuItem.Text = "Get last flightplan";
            getLastFlightplanToolStripMenuItem.Click += getLastFlightplanToolStripMenuItem_Click;
            // 
            // getFlightBriefingToolStripMenuItem
            // 
            getFlightBriefingToolStripMenuItem.Enabled = false;
            getFlightBriefingToolStripMenuItem.Name = "getFlightBriefingToolStripMenuItem";
            getFlightBriefingToolStripMenuItem.Size = new Size(187, 22);
            getFlightBriefingToolStripMenuItem.Text = "Get flight briefing";
            getFlightBriefingToolStripMenuItem.Click += getFlightBriefingToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(115, 6);
            // 
            // restartToolStripMenuItem
            // 
            restartToolStripMenuItem.Enabled = false;
            restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            restartToolStripMenuItem.Size = new Size(118, 22);
            restartToolStripMenuItem.Text = "Restart";
            restartToolStripMenuItem.Click += restartToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(115, 6);
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(118, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // lblDistanceTotale
            // 
            lblDistanceTotale.AutoSize = true;
            lblDistanceTotale.Location = new Point(0, 0);
            lblDistanceTotale.Name = "lblDistanceTotale";
            lblDistanceTotale.Size = new Size(99, 15);
            lblDistanceTotale.TabIndex = 5;
            lblDistanceTotale.Text = "Load a flight plan";
            lblDistanceTotale.Click += lblDistanceTotale_Click;
            // 
            // lvWaypoints
            // 
            lvWaypoints.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvWaypoints.AutoArrange = false;
            lvWaypoints.Columns.AddRange(new ColumnHeader[] { ColWaypoint, ColName, ColRoute, ColDistance });
            lvWaypoints.Font = new Font("Segoe Script", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lvWaypoints.FullRowSelect = true;
            lvWaypoints.GridLines = true;
            lvWaypoints.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvWaypoints.Location = new Point(3, 26);
            lvWaypoints.MultiSelect = false;
            lvWaypoints.Name = "lvWaypoints";
            lvWaypoints.Size = new Size(512, 208);
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
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74.66411F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.3358917F));
            tableLayoutPanel1.Controls.Add(panel1, 1, 0);
            tableLayoutPanel1.Controls.Add(splitContainer3, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(521, 417);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(64, 64, 64);
            panel1.Controls.Add(splitContainer2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(392, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(126, 411);
            panel1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(compas1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.BackgroundImage = (Image)resources.GetObject("splitContainer2.Panel2.BackgroundImage");
            splitContainer2.Panel2.BackgroundImageLayout = ImageLayout.Zoom;
            splitContainer2.Size = new Size(126, 411);
            splitContainer2.SplitterDistance = 203;
            splitContainer2.TabIndex = 2;
            // 
            // compas1
            // 
            compas1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            compas1.BackColor = Color.FromArgb(64, 64, 64);
            compas1.Headings = new int[]
    {
    0
    };
            compas1.LabelText = "Compas";
            compas1.Location = new Point(3, 3);
            compas1.Name = "compas1";
            compas1.NbNeedles = 1;
            compas1.NeedleImages = new Image[]
    {
    (Image)resources.GetObject("compas1.NeedleImages")
    };
            compas1.NumericValue = 0D;
            compas1.RectangleSize = new Size(80, 20);
            compas1.Size = new Size(117, 186);
            compas1.TabIndex = 1;
            compas1.Unit = "NM";
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(3, 3);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(tbComment);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(webView21);
            splitContainer3.Size = new Size(383, 411);
            splitContainer3.SplitterDistance = 153;
            splitContainer3.TabIndex = 3;
            // 
            // tbComment
            // 
            tbComment.Dock = DockStyle.Fill;
            tbComment.Font = new Font("Segoe Script", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbComment.Location = new Point(0, 0);
            tbComment.Multiline = true;
            tbComment.Name = "tbComment";
            tbComment.ScrollBars = ScrollBars.Both;
            tbComment.Size = new Size(383, 153);
            tbComment.TabIndex = 0;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(0, 0);
            webView21.Name = "webView21";
            webView21.Size = new Size(383, 254);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // FlightplanCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(128, 64, 0);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            ForeColor = Color.White;
            Name = "FlightplanCtrl";
            Size = new Size(521, 680);
            Load += BushTripCtrl_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel1.PerformLayout();
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
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
        private ImageList imageList1;
        private TableLayoutPanel tableLayoutPanel1;
        private SimAddonControls.Compas compas1;
        private Panel panel1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem createToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem restartToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem simbriefToolStripMenuItem;
        private ToolStripMenuItem getLastFlightplanToolStripMenuItem;
        private ToolStripMenuItem getFlightBriefingToolStripMenuItem;
        private SplitContainer splitContainer2;
        private ToolStripMenuItem createNewFlightplanToolStripMenuItem;
        private SplitContainer splitContainer3;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
    }
}
