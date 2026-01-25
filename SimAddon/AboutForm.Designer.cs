using System.Drawing;
using System.Windows.Forms;

namespace SimAddon
{
    partial class AboutForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle;
        private Label lblVersion;
        private Label lblPoweredBy;
        private LinkLabel linkLabelGitHub;
        private Button btnOK;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblVersion = new Label();
            lblPoweredBy = new Label();
            linkLabelGitHub = new LinkLabel();
            btnOK = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(118, 26);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "SimAddon";
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblVersion.ForeColor = Color.LightGray;
            lblVersion.Location = new Point(20, 52);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(92, 17);
            lblVersion.TabIndex = 2;
            lblVersion.Text = "Version 1.0.0";
            // 
            // lblPoweredBy
            // 
            lblPoweredBy.AutoSize = true;
            lblPoweredBy.Font = new Font("Arial", 10F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblPoweredBy.ForeColor = Color.White;
            lblPoweredBy.Location = new Point(20, 110);
            lblPoweredBy.Name = "lblPoweredBy";
            lblPoweredBy.Size = new Size(160, 16);
            lblPoweredBy.TabIndex = 3;
            lblPoweredBy.Text = "Propulsée par JFK && PB";
            // 
            // linkLabelGitHub
            // 
            linkLabelGitHub.AutoSize = true;
            linkLabelGitHub.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabelGitHub.LinkColor = Color.FromArgb(0, 122, 204);
            linkLabelGitHub.Location = new Point(20, 140);
            linkLabelGitHub.Name = "linkLabelGitHub";
            linkLabelGitHub.Size = new Size(208, 15);
            linkLabelGitHub.TabIndex = 4;
            linkLabelGitHub.TabStop = true;
            linkLabelGitHub.Text = "https://github.com/Skall34/SimAddon";
            linkLabelGitHub.LinkClicked += linkLabel_LinkClicked;
            // 
            // btnOK
            // 
            btnOK.BackColor = Color.FromArgb(0, 122, 204);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Font = new Font("Arial", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOK.ForeColor = Color.White;
            btnOK.Location = new Point(148, 168);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(80, 30);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = false;
            btnOK.Click += btnOK_Click;
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(242, 210);
            Controls.Add(btnOK);
            Controls.Add(linkLabelGitHub);
            Controls.Add(lblPoweredBy);
            Controls.Add(lblVersion);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "About SimAddon";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
