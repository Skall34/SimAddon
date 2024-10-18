using System.Drawing;
using System.Windows.Forms;

namespace FlightRecorder
{
    partial class BugForm
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
            tbDesc = new TextBox();
            btnSend = new Button();
            btnCancel = new Button();
            tbTitle = new TextBox();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // tbDesc
            // 
            tbDesc.AcceptsReturn = true;
            tbDesc.AcceptsTab = true;
            tbDesc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbDesc.Location = new Point(12, 56);
            tbDesc.Multiline = true;
            tbDesc.Name = "tbDesc";
            tbDesc.ScrollBars = ScrollBars.Vertical;
            tbDesc.Size = new Size(369, 194);
            tbDesc.TabIndex = 0;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSend.Location = new Point(306, 254);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 1;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(225, 254);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbTitle
            // 
            tbTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbTitle.Location = new Point(54, 12);
            tbTitle.Name = "tbTitle";
            tbTitle.Size = new Size(327, 23);
            tbTitle.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 4;
            label1.Text = "Titre :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 38);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 5;
            label2.Text = "Description :";
            // 
            // BugForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(393, 281);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tbTitle);
            Controls.Add(btnCancel);
            Controls.Add(btnSend);
            Controls.Add(tbDesc);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BugForm";
            Text = "BugForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbDesc;
        private Button btnSend;
        private Button btnCancel;
        private TextBox tbTitle;
        private Label label1;
        private Label label2;
    }
}