using System;
using System.Drawing;
using System.Windows.Forms;


namespace SimAddonControls
{
    public partial class RotaryKnob : UserControl
    {
        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 100;

        private int increment = 1;
        public int Increment
        {
            get => increment;
            set => increment = value>0?value:1; // Empêche 0 ou négatif
        }

        private int value = 0;
        public int Value
        {
            get => value;
            set
            {
                int newValue = Math.Max(Minimum, Math.Min(Maximum, value));
                if (this.value != newValue)
                {
                    this.value = newValue;
                    Invalidate(); // Redessiner
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ValueChanged;

        public RotaryKnob()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Size = new Size(100, 100);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Value += e.Delta > 0 ? Increment : -Increment;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int radius = Math.Min(Width, Height) / 2 - 5;
            Point center = new Point(Width / 2, Height / 2);

            // Fond du bouton
            g.FillEllipse(Brushes.Gray, center.X - radius, center.Y - radius, radius * 2, radius * 2);
            g.DrawEllipse(Pens.Black, center.X - radius, center.Y - radius, radius * 2, radius * 2);

            // Calcul de l’angle (ex: 135° à 45°)
            float angle = 270f * (Value - Minimum) / (Maximum - Minimum) - 135f;
            double rad = angle * Math.PI / 180;

            Point needle = new Point(
                center.X + (int)(radius * 0.8 * Math.Cos(rad)),
                center.Y + (int)(radius * 0.8 * Math.Sin(rad))
            );

            // Aiguille
            using (Pen pen = new Pen(Color.Red, 3))
            {
                g.DrawLine(pen, center, needle);
            }
        }
    }
}

