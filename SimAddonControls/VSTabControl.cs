using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SimAddonControls
{
    /// <summary>
    /// TabControl personnalisé avec un style Visual Studio
    /// </summary>
    public partial class VSTabControl : TabControl
    {
        // Couleurs personnalisables
        private Color _tabBackColor = Color.FromArgb(45, 45, 48);
        private Color _tabSelectedBackColor = Color.MidnightBlue;
        private Color _tabHoverBackColor = Color.DarkGray;
        private Color _tabTextColor = Color.White;
        private Color _tabSelectedTextColor = Color.White;
        private Color _accentColor = Color.FromArgb(0, 122, 204);
        private Color _tabPageBackColor = Color.FromArgb(37, 37, 38);

        [Category("Appearance")]
        [Description("Couleur de fond des onglets non sélectionnés")]
        public Color TabBackColor
        {
            get => _tabBackColor;
            set { _tabBackColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Couleur de fond de l'onglet sélectionné")]
        public Color TabSelectedBackColor
        {
            get => _tabSelectedBackColor;
            set { _tabSelectedBackColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Couleur de fond de l'onglet au survol")]
        public Color TabHoverBackColor
        {
            get => _tabHoverBackColor;
            set { _tabHoverBackColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Couleur du texte des onglets")]
        public Color TabTextColor
        {
            get => _tabTextColor;
            set { _tabTextColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Couleur du texte de l'onglet sélectionné")]
        public Color TabSelectedTextColor
        {
            get => _tabSelectedTextColor;
            set { _tabSelectedTextColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Couleur de la ligne d'accent sur l'onglet sélectionné")]
        public Color AccentColor
        {
            get => _accentColor;
            set { _accentColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Couleur de fond du contenu des TabPages")]
        public Color TabPageBackColor
        {
            get => _tabPageBackColor;
            set
            {
                _tabPageBackColor = value;
                UpdateTabPagesBackColor();
            }
        }

        public VSTabControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            
            DrawMode = TabDrawMode.OwnerDrawFixed;
            ItemSize = new Size(80, 25);
            SizeMode = TabSizeMode.Fixed;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control is TabPage tabPage)
            {
                tabPage.BackColor = _tabPageBackColor;
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= TabCount) return;

            Graphics g = e.Graphics;
            TabPage tabPage = TabPages[e.Index];
            Rectangle tabBounds = GetTabRect(e.Index);

            // Déterminer si l'onglet est sélectionné
            bool isSelected = (e.Index == SelectedIndex);

            // Déterminer si la souris survole l'onglet
            Point mousePos = PointToClient(Cursor.Position);
            bool isHovered = tabBounds.Contains(mousePos);

            // Dessiner le fond de l'onglet
            Color backColor = isSelected ? _tabSelectedBackColor : (isHovered ? _tabHoverBackColor : _tabBackColor);
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, tabBounds);
            }

            // Dessiner une ligne d'accent en haut de l'onglet sélectionné
            if (isSelected)
            {
                using (Pen accentPen = new Pen(_accentColor, 3))
                {
                    g.DrawLine(accentPen, tabBounds.Left, tabBounds.Top + 1, tabBounds.Right, tabBounds.Top + 1);
                }
            }

            // Dessiner le texte centré
            Color textColor = isSelected ? _tabSelectedTextColor : _tabTextColor;
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(tabPage.Text, Font, textBrush, tabBounds, sf);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Dessiner l'arrière-plan de la zone des onglets
            Rectangle tabAreaRect = new Rectangle(0, 0, Width, ItemSize.Height + 4);
            using (SolidBrush backBrush = new SolidBrush(_tabBackColor))
            {
                e.Graphics.FillRectangle(backBrush, tabAreaRect);
            }

            // Redessiner tous les onglets
            for (int i = 0; i < TabCount; i++)
            {
                Rectangle tabRect = GetTabRect(i);
                DrawItemEventArgs args = new DrawItemEventArgs(
                    e.Graphics, Font, tabRect, i,
                    i == SelectedIndex ? DrawItemState.Selected : DrawItemState.None);
                OnDrawItem(args);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            // Redessiner pour l'effet hover
            Invalidate();
        }

        private void UpdateTabPagesBackColor()
        {
            foreach (TabPage page in TabPages)
            {
                page.BackColor = _tabPageBackColor;
            }
        }
    }
}
