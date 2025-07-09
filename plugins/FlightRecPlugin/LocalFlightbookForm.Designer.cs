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
            btnOK = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            tbFlightDetails = new System.Windows.Forms.TextBox();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { Date, Departure, Arrival, Immat, comments });
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
            // LocalFlightbookForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(563, 444);
            Controls.Add(splitContainer1);
            Controls.Add(btnClear);
            Controls.Add(btnOK);
            Name = "LocalFlightbookForm";
            Text = "Local flightbook";
            Load += LocalFlightbookForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
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
    }
}