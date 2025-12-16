namespace FlightplanPlugin
{
    partial class SettingsForm
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
            btnOK = new Button();
            btnCancel = new Button();
            lblSimbriefUser = new Label();
            tbSimbriefUsername = new TextBox();
            label1 = new Label();
            tbPdfFolder = new TextBox();
            btnBrowsePdfFolder = new Button();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.Location = new Point(406, 76);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(325, 76);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblSimbriefUser
            // 
            lblSimbriefUser.AutoSize = true;
            lblSimbriefUser.Location = new Point(12, 9);
            lblSimbriefUser.Name = "lblSimbriefUser";
            lblSimbriefUser.Size = new Size(114, 15);
            lblSimbriefUser.TabIndex = 2;
            lblSimbriefUser.Text = "simbrief user name :";
            // 
            // tbSimbriefUsername
            // 
            tbSimbriefUsername.Location = new Point(132, 6);
            tbSimbriefUsername.Name = "tbSimbriefUsername";
            tbSimbriefUsername.Size = new Size(183, 23);
            tbSimbriefUsername.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 41);
            label1.Name = "label1";
            label1.Size = new Size(112, 15);
            label1.TabIndex = 4;
            label1.Text = "Flight brief storage :";
            // 
            // tbPdfFolder
            // 
            tbPdfFolder.Location = new Point(130, 38);
            tbPdfFolder.Name = "tbPdfFolder";
            tbPdfFolder.Size = new Size(308, 23);
            tbPdfFolder.TabIndex = 5;
            // 
            // btnBrowsePdfFolder
            // 
            btnBrowsePdfFolder.Location = new Point(444, 38);
            btnBrowsePdfFolder.Name = "btnBrowsePdfFolder";
            btnBrowsePdfFolder.Size = new Size(39, 23);
            btnBrowsePdfFolder.TabIndex = 6;
            btnBrowsePdfFolder.Text = "...";
            btnBrowsePdfFolder.UseVisualStyleBackColor = true;
            btnBrowsePdfFolder.Click += btnBrowsePdfFolder_Click;
            // 
            // SettingsForm
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(493, 111);
            Controls.Add(btnBrowsePdfFolder);
            Controls.Add(tbPdfFolder);
            Controls.Add(label1);
            Controls.Add(tbSimbriefUsername);
            Controls.Add(lblSimbriefUser);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Simbrief settings";
            Load += SettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOK;
        private Button btnCancel;
        private Label lblSimbriefUser;
        private TextBox tbSimbriefUsername;
        private Label label1;
        private TextBox tbPdfFolder;
        private Button btnBrowsePdfFolder;
    }
}