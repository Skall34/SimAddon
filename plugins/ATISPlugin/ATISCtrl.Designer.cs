
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
            components = new System.ComponentModel.Container();
            splitContainer1 = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            tbATISText = new TextBox();
            cbICAO = new ComboBox();
            btnRequest = new Button();
            label1 = new Label();
            panel1 = new Panel();
            splitContainer2 = new SplitContainer();
            lvControllers = new ListView();
            columnType = new ColumnHeader();
            columnRating = new ColumnHeader();
            columnCallsign = new ColumnHeader();
            columnFreq = new ColumnHeader();
            tbController = new TextBox();
            UpdateVATSIMTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(0);
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
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new Size(534, 714);
            splitContainer1.SplitterDistance = 355;
            splitContainer1.SplitterWidth = 6;
            splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.Controls.Add(tbATISText, 0, 1);
            tableLayoutPanel1.Controls.Add(cbICAO, 1, 0);
            tableLayoutPanel1.Controls.Add(btnRequest, 2, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.Size = new Size(534, 355);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tbATISText
            // 
            tbATISText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.SetColumnSpan(tbATISText, 3);
            tbATISText.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbATISText.Location = new Point(4, 44);
            tbATISText.Margin = new Padding(4);
            tbATISText.Multiline = true;
            tbATISText.Name = "tbATISText";
            tbATISText.ScrollBars = ScrollBars.Vertical;
            tbATISText.Size = new Size(526, 243);
            tbATISText.TabIndex = 0;
            // 
            // cbICAO
            // 
            cbICAO.Dock = DockStyle.Fill;
            cbICAO.FormattingEnabled = true;
            cbICAO.Location = new Point(184, 4);
            cbICAO.Margin = new Padding(4);
            cbICAO.Name = "cbICAO";
            cbICAO.Size = new Size(226, 29);
            cbICAO.TabIndex = 1;
            cbICAO.SelectedIndexChanged += cbICAO_SelectedIndexChanged;
            cbICAO.KeyPress += cbICAO_KeyPress;
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
            // panel1
            // 
            panel1.BackgroundImage = Properties.Resources.Virtual_Air_Traffic_Simulation_Network__logo_;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Location = new Point(0, 291);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(180, 64);
            panel1.TabIndex = 6;
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
            splitContainer2.Panel1.Controls.Add(lvControllers);
            splitContainer2.Panel1.Paint += splitContainer2_Panel1_Paint;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(tbController);
            splitContainer2.Size = new Size(534, 353);
            splitContainer2.SplitterDistance = 216;
            splitContainer2.TabIndex = 1;
            // 
            // lvControllers
            // 
            lvControllers.Columns.AddRange(new ColumnHeader[] { columnType, columnRating, columnCallsign, columnFreq });
            lvControllers.Dock = DockStyle.Fill;
            lvControllers.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lvControllers.FullRowSelect = true;
            lvControllers.GridLines = true;
            lvControllers.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvControllers.Location = new Point(0, 0);
            lvControllers.MultiSelect = false;
            lvControllers.Name = "lvControllers";
            lvControllers.Size = new Size(534, 216);
            lvControllers.TabIndex = 0;
            lvControllers.UseCompatibleStateImageBehavior = false;
            lvControllers.View = View.Details;
            lvControllers.SelectedIndexChanged += lvControllers_SelectedIndexChanged;
            // 
            // columnType
            // 
            columnType.Text = "Facility";
            columnType.Width = 120;
            // 
            // columnRating
            // 
            columnRating.Text = "Rating";
            columnRating.TextAlign = HorizontalAlignment.Center;
            columnRating.Width = 120;
            // 
            // columnCallsign
            // 
            columnCallsign.Text = "Callsign";
            columnCallsign.Width = 120;
            // 
            // columnFreq
            // 
            columnFreq.Text = "Frequency";
            columnFreq.TextAlign = HorizontalAlignment.Center;
            columnFreq.Width = 120;
            // 
            // tbController
            // 
            tbController.Dock = DockStyle.Fill;
            tbController.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbController.Location = new Point(0, 0);
            tbController.Multiline = true;
            tbController.Name = "tbController";
            tbController.ScrollBars = ScrollBars.Vertical;
            tbController.Size = new Size(534, 133);
            tbController.TabIndex = 0;
            // 
            // UpdateVATSIMTimer
            // 
            UpdateVATSIMTimer.Enabled = true;
            UpdateVATSIMTimer.Interval = 300000;
            UpdateVATSIMTimer.Tick += UpdateVATSIMTimer_Tick;
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
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }



        #endregion

        private SplitContainer splitContainer1;
        private Button btnRequest;
        private ComboBox cbICAO;
        private ComboBox cbVoices;
        private TextBox tbATISText;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer UpdateVATSIMTimer;
        private ListView lvControllers;
        private ColumnHeader columnType;
        private ColumnHeader columnRating;
        private ColumnHeader columnFreq;
        private ColumnHeader columnCallsign;
        private SplitContainer splitContainer2;
        private TextBox tbController;
        private Panel panel1;
    }
}
