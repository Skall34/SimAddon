using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SimAddonControls
{
    /// <summary>
    /// StatusStrip personnalisé avec un style Visual Studio
    /// </summary>
    public partial class VSStatusStrip : StatusStrip
    {
        // Couleurs personnalisables
        private Color _backColor = Color.FromArgb(0, 122, 204);
        private Color _foreColor = Color.White;
        private Color _borderColor = Color.FromArgb(0, 100, 180);
        private Color _gripColor = Color.FromArgb(70, 70, 74);

        [Category("Appearance")]
        [Description("Couleur de fond du StatusStrip")]
        public new Color BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                base.BackColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Couleur du texte par défaut")]
        public new Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                base.ForeColor = value;
                UpdateItemsColors();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Couleur de la bordure supérieure")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Couleur de la poignée de redimensionnement")]
        public Color GripColor
        {
            get => _gripColor;
            set { _gripColor = value; Invalidate(); }
        }

        public VSStatusStrip()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            // Configuration par défaut
            this.BackColor = _backColor;
            this.ForeColor = _foreColor;
            this.Renderer = new VSStatusStripRenderer(this);
        }

        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            base.OnItemAdded(e);
            UpdateItemColor(e.Item);
        }

        private void UpdateItemsColors()
        {
            foreach (ToolStripItem item in Items)
            {
                UpdateItemColor(item);
            }
        }

        private void UpdateItemColor(ToolStripItem item)
        {
            if (item != null)
            {
                item.ForeColor = _foreColor;
                item.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// Renderer personnalisé pour le VSStatusStrip
        /// </summary>
        private class VSStatusStripRenderer : ToolStripProfessionalRenderer
        {
            private VSStatusStrip _statusStrip;

            public VSStatusStripRenderer(VSStatusStrip statusStrip)
            {
                _statusStrip = statusStrip;
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                // Dessiner le fond
                using (SolidBrush brush = new SolidBrush(_statusStrip.BackColor))
                {
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                }

                // Dessiner la bordure supérieure
                using (Pen pen = new Pen(_statusStrip.BorderColor, 1))
                {
                    e.Graphics.DrawLine(pen, 0, 0, e.ToolStrip.Width, 0);
                }
            }

            protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
            {
                // Dessiner la poignée de redimensionnement personnalisée
                if (e.GripStyle == ToolStripGripStyle.Visible)
                {
                    using (SolidBrush brush = new SolidBrush(_statusStrip.GripColor))
                    {
                        int gripWidth = 3;
                        int gripHeight = 3;
                        int spacing = 4;
                        
                        // Dessiner 3 rangées de 3 points
                        for (int row = 0; row < 3; row++)
                        {
                            for (int col = 0; col < 3; col++)
                            {
                                int x = e.GripBounds.Left + (col * spacing) + 2;
                                int y = e.GripBounds.Top + (row * spacing) + 2;
                                e.Graphics.FillRectangle(brush, x, y, gripWidth, gripHeight);
                            }
                        }
                    }
                }
                else
                {

                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                // Utiliser la couleur de texte personnalisée
                e.TextColor = _statusStrip.ForeColor;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            {
                // Dessiner le séparateur avec la couleur de bordure
                if (!e.Vertical)
                {
                    using (Pen pen = new Pen(_statusStrip.BorderColor))
                    {
                        int y = e.Item.Height / 2;
                        e.Graphics.DrawLine(pen, 
                            e.Item.ContentRectangle.Left, 
                            y, 
                            e.Item.ContentRectangle.Right, 
                            y);
                    }
                }
                else
                {
                    using (Pen pen = new Pen(_statusStrip.BorderColor))
                    {
                        int x = e.Item.Width / 2;
                        e.Graphics.DrawLine(pen, 
                            x, 
                            e.Item.ContentRectangle.Top, 
                            x, 
                            e.Item.ContentRectangle.Bottom);
                    }
                }
            }
        }
    }
}
