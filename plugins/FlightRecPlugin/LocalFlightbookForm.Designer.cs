namespace FlightRecPlugin
{
    partial class LocalFlightbookForm
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
            components = new System.ComponentModel.Container();
            listView1 = new System.Windows.Forms.ListView();
            Date = new System.Windows.Forms.ColumnHeader();
            Departure = new System.Windows.Forms.ColumnHeader();
            Arrival = new System.Windows.Forms.ColumnHeader();
            Immat = new System.Windows.Forms.ColumnHeader();
            comments = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            pushToServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            btnOK = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            tbFlightDetails = new System.Windows.Forms.TextBox();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            lblLocalFlightbookSize = new System.Windows.Forms.Label();
            extractFlightDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { Date, Departure, Arrival, Immat, comments });
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(539, 286);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // Date
            // 
            Date.Text = "Date (start)";
            Date.Width = 120;
            // 
            // Departure
            // 
            Departure.Text = "Departure";
            Departure.Width = 80;
            // 
            // Arrival
            // 
            Arrival.Text = "Arrival";
            Arrival.Width = 80;
            // 
            // Immat
            // 
            Immat.Text = "Immatriculation";
            Immat.Width = 100;
            // 
            // comments
            // 
            comments.Text = "Comments";
            comments.Width = 150;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { extractFlightDataToolStripMenuItem, pushToServerToolStripMenuItem, toolStripSeparator1, deleteToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(181, 98);
            // 
            // pushToServerToolStripMenuItem
            // 
            pushToServerToolStripMenuItem.Name = "pushToServerToolStripMenuItem";
            pushToServerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            pushToServerToolStripMenuItem.Text = "Push to server";
            pushToServerToolStripMenuItem.Click += pushToServerToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // btnOK
            // 
            btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOK.Location = new System.Drawing.Point(476, 409);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnClear
            // 
            btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnClear.Location = new System.Drawing.Point(12, 409);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(75, 23);
            btnClear.TabIndex = 2;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // tbFlightDetails
            // 
            tbFlightDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            tbFlightDetails.Location = new System.Drawing.Point(0, 0);
            tbFlightDetails.Multiline = true;
            tbFlightDetails.Name = "tbFlightDetails";
            tbFlightDetails.Size = new System.Drawing.Size(539, 101);
            tbFlightDetails.TabIndex = 3;
            tbFlightDetails.TextChanged += tbFlightDetails_TextChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(12, 12);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tbFlightDetails);
            splitContainer1.Size = new System.Drawing.Size(539, 391);
            splitContainer1.SplitterDistance = 286;
            splitContainer1.TabIndex = 4;
            // 
            // lblLocalFlightbookSize
            // 
            lblLocalFlightbookSize.AutoSize = true;
            lblLocalFlightbookSize.Location = new System.Drawing.Point(93, 413);
            lblLocalFlightbookSize.Name = "lblLocalFlightbookSize";
            lblLocalFlightbookSize.Size = new System.Drawing.Size(115, 15);
            lblLocalFlightbookSize.TabIndex = 5;
            lblLocalFlightbookSize.Text = "Local flightbook size";
            // 
            // extractFlightDataToolStripMenuItem
            // 
            extractFlightDataToolStripMenuItem.Name = "extractFlightDataToolStripMenuItem";
            extractFlightDataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            extractFlightDataToolStripMenuItem.Text = "Extract flight data";
            extractFlightDataToolStripMenuItem.Click += extractFlightDataToolStripMenuItem_Click;
            // 
            // LocalFlightbookForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(563, 444);
            Controls.Add(lblLocalFlightbookSize);
            Controls.Add(splitContainer1);
            Controls.Add(btnClear);
            Controls.Add(btnOK);
            Name = "LocalFlightbookForm";
            Text = "Local flightbook";
            Load += LocalFlightbookForm_Load;
            contextMenuStrip1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Departure;
        private System.Windows.Forms.ColumnHeader Arrival;
        private System.Windows.Forms.ColumnHeader Immat;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ColumnHeader comments;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox tbFlightDetails;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pushToServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Label lblLocalFlightbookSize;
        private System.Windows.Forms.ToolStripMenuItem extractFlightDataToolStripMenuItem;
    }
}