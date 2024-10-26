
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeteoCtrl));
            label1 = new Label();
            tbICAO = new TextBox();
            btnRequest = new Button();
            label2 = new Label();
            tbDecodedMETAR = new TextBox();
            pictureBox1 = new PictureBox();
            tbMETAR = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 74);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 0;
            label1.Text = "Airport ICAO";
            // 
            // tbICAO
            // 
            tbICAO.Location = new Point(129, 71);
            tbICAO.Name = "tbICAO";
            tbICAO.Size = new Size(100, 23);
            tbICAO.TabIndex = 1;
            // 
            // btnRequest
            // 
            btnRequest.Location = new Point(235, 71);
            btnRequest.Name = "btnRequest";
            btnRequest.Size = new Size(75, 23);
            btnRequest.TabIndex = 2;
            btnRequest.Text = "Request";
            btnRequest.UseVisualStyleBackColor = true;
            btnRequest.Click += btnRequest_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 109);
            label2.Name = "label2";
            label2.Size = new Size(53, 15);
            label2.TabIndex = 3;
            label2.Text = "METAR : ";
            // 
            // tbDecodedMETAR
            // 
            tbDecodedMETAR.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbDecodedMETAR.Location = new Point(15, 196);
            tbDecodedMETAR.Multiline = true;
            tbDecodedMETAR.Name = "tbDecodedMETAR";
            tbDecodedMETAR.ReadOnly = true;
            tbDecodedMETAR.Size = new Size(771, 43);
            tbDecodedMETAR.TabIndex = 4;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(794, 62);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // tbMETAR
            // 
            tbMETAR.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbMETAR.Location = new Point(15, 138);
            tbMETAR.Multiline = true;
            tbMETAR.Name = "tbMETAR";
            tbMETAR.ReadOnly = true;
            tbMETAR.Size = new Size(771, 52);
            tbMETAR.TabIndex = 7;
            // 
            // MeteoCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Teal;
            Controls.Add(tbMETAR);
            Controls.Add(pictureBox1);
            Controls.Add(tbDecodedMETAR);
            Controls.Add(label2);
            Controls.Add(btnRequest);
            Controls.Add(tbICAO);
            Controls.Add(label1);
            Name = "MeteoCtrl";
            Size = new Size(800, 450);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private Label label1;
        private TextBox tbICAO;
        private Button btnRequest;
        private Label label2;
        private TextBox tbDecodedMETAR;
        private PictureBox pictureBox1;
        private TextBox tbMETAR;
    }
}
