namespace Calculator
{
    partial class Converter
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            tbInput = new TextBox();
            lblInput = new Label();
            cbSrcUnits = new ComboBox();
            lblArrow = new Label();
            lblResult = new Label();
            cbDstUnits = new ComboBox();
            SuspendLayout();
            // 
            // tbInput
            // 
            tbInput.Location = new Point(65, 3);
            tbInput.Name = "tbInput";
            tbInput.Size = new Size(89, 23);
            tbInput.TabIndex = 0;
            tbInput.Text = "0";
            tbInput.TextChanged += tbInput_TextChanged;
            // 
            // lblInput
            // 
            lblInput.AutoSize = true;
            lblInput.Location = new Point(4, 6);
            lblInput.Name = "lblInput";
            lblInput.Size = new Size(43, 15);
            lblInput.TabIndex = 1;
            lblInput.Text = "Source";
            // 
            // cbSrcUnits
            // 
            cbSrcUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSrcUnits.FormattingEnabled = true;
            cbSrcUnits.Location = new Point(160, 3);
            cbSrcUnits.Name = "cbSrcUnits";
            cbSrcUnits.Size = new Size(121, 23);
            cbSrcUnits.TabIndex = 2;
            cbSrcUnits.SelectedIndexChanged += cbSrcUnits_SelectedIndexChanged;
            // 
            // lblArrow
            // 
            lblArrow.AutoSize = true;
            lblArrow.Location = new Point(287, 6);
            lblArrow.Name = "lblArrow";
            lblArrow.Size = new Size(23, 15);
            lblArrow.TabIndex = 3;
            lblArrow.Text = "=>";
            // 
            // lblResult
            // 
            lblResult.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblResult.AutoSize = true;
            lblResult.Location = new Point(308, 6);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(16, 15);
            lblResult.TabIndex = 4;
            lblResult.Text = "...";
            // 
            // cbDstUnits
            // 
            cbDstUnits.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbDstUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDstUnits.FormattingEnabled = true;
            cbDstUnits.Location = new Point(401, 3);
            cbDstUnits.Name = "cbDstUnits";
            cbDstUnits.Size = new Size(121, 23);
            cbDstUnits.TabIndex = 5;
            cbDstUnits.SelectedIndexChanged += cbDstUnits_SelectedIndexChanged;
            // 
            // Converter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cbDstUnits);
            Controls.Add(lblResult);
            Controls.Add(lblArrow);
            Controls.Add(cbSrcUnits);
            Controls.Add(lblInput);
            Controls.Add(tbInput);
            Name = "Converter";
            Size = new Size(525, 30);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbInput;
        private Label lblInput;
        private ComboBox cbSrcUnits;
        private Label lblArrow;
        private Label lblResult;
        private ComboBox cbDstUnits;
    }
}
