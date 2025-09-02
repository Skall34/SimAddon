namespace SimAddonControls
{
    public partial class Compas : UserControl
    {
        public string LabelText { get; set; } = "Compas";

        private int _nbNeedles = 0;
        public int NbNeedles
        {
            get
            {
                return _nbNeedles;
            }
            set
            {
                _nbNeedles = value;
                _headings=new int[NbNeedles];
                _needleImages=new Image[NbNeedles];
                Invalidate();
            }
        } // Heading in degrees

        private int[] _headings ;
        public int[] Headings { 
            get {
                return _headings;
            } 
            set {
                _headings = value;
                Invalidate();
            }
        } // Heading in degrees

        private double _numericvalue = 0;
        public double NumericValue { get
            {
                return _numericvalue;
            }
            set { 
                _numericvalue = value;
                Invalidate();
            } 
        } // Distance in nautical miles

        private string _unit = "NM";
        public string Unit { get { 
                return _unit;
            } set { 
                _unit = value;
                Invalidate();
            }
        }

        private Image[] _needleImages = null;

        public Image[] NeedleImages { 
            get {
                return _needleImages;
            } set 
            {
                _needleImages = value;
                Invalidate();
            }
        }

        private Size _rectanglesize = new Size(60, 20);

        public Size RectangleSize { get { 
                return _rectanglesize;
            }
            set { 
                _rectanglesize = value;
                Invalidate();
            }
        }

        public Compas()
        {
            InitializeComponent();
        }

        private void paintNeedle(Graphics g,int centerX,int centerY,int radius,Image image,int hdg)
        {
            float needleAngle = hdg - 90; // Offset by 90 degrees to make 0 point upwards
            g.TranslateTransform(centerX, centerY);
            g.RotateTransform(needleAngle);

            // Calculate maximum length and scale factor for the needle
            int maxNeedleLength = (int)(radius * 1.8);
            float scaleFactor = Math.Min((float)maxNeedleLength / image.Width, 1.0f);

            // Calculate scaled dimensions
            int needleWidth = (int)(image.Width * scaleFactor);
            int needleHeight = (int)(image.Height * scaleFactor);

            // Draw the scaled image centered on the dial
            g.DrawImage(image, -needleWidth / 2, -needleHeight / 2, needleWidth, needleHeight);

            // Reset transformation
            g.ResetTransform();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int centerX = Width / 2;
            int centerY = Height / 2;
            int radius = Math.Min(centerX, centerY) - 10;

            // Draw white outer circle with a width of 5 pixels
            int outerRadius = radius + 5;
            g.DrawEllipse(new Pen(Color.White, 5), centerX - outerRadius, centerY - outerRadius, outerRadius * 2, outerRadius * 2);

            // Draw black circular dial
            g.FillEllipse(Brushes.Black, centerX - radius, centerY - radius, radius * 2, radius * 2);

            // Draw white graduations every 10 degrees
            for (int i = 0; i < 360; i += 10)
            {

                double angle = Math.PI * i / 180.0;
                int innerX = centerX + (int)(radius * 0.8 * Math.Cos(angle));
                int innerY = centerY + (int)(radius * 0.8 * Math.Sin(angle));
                if (i%90==0)
                {
                    innerX = centerX + (int)(radius * 0.6* Math.Cos(angle));
                    innerY = centerY + (int)(radius * 0.6 * Math.Sin(angle));
                }
                int outerX = centerX + (int)(radius * Math.Cos(angle));
                int outerY = centerY + (int)(radius * Math.Sin(angle));
                g.DrawLine(Pens.White, innerX, innerY, outerX, outerY);
            }


            // Draw fixed-size rectangle and right-aligned distance text
            string distanceText = $"{NumericValue:F1} {Unit}";
            Font font = new Font("Arial", 10);
            SizeF textSize = g.MeasureString(distanceText, font);
            Rectangle textRect = new Rectangle(
                (int)(centerX - textSize.Width / 2 - 2),
                (int)(centerY + radius * 0.5f - textSize.Height / 2 - 2),
                (int)textSize.Width + 4,
                (int)textSize.Height + 4);

            g.DrawRectangle(Pens.White, textRect);


            // Calculate position for right-aligned text within the rectangle
            float textX = textRect.Right - textSize.Width - 2;
            float textY = textRect.Top + (textRect.Height - textSize.Height) / 2;
            g.DrawString(distanceText, font, Brushes.White, textX, textY);


            // Draw the label text below the dial
            if (!string.IsNullOrEmpty(LabelText))
            {
                Font labelFont = new Font("Arial", 10);
                SizeF labelSize = g.MeasureString(LabelText, labelFont);

                Rectangle LabelRect = new Rectangle(
                    (int)(centerX - labelSize.Width / 2-2),
                    (int)(centerY - radius * 0.5f - labelSize.Height / 2 - 2),
                    (int)labelSize.Width + 4,
                    (int)labelSize.Height + 4);

                g.DrawRectangle(Pens.White, LabelRect);

                float labelX = centerX - labelSize.Width / 2;
                float labelY = LabelRect.Top + (LabelRect.Height - labelSize.Height) / 2;
                g.DrawString(LabelText, labelFont, Brushes.White, labelX, labelY);
            }

            for (int i = 0; i < NbNeedles; i++)
            {
                // Draw the needle as an image
                if (NeedleImages != null)
                {
                    paintNeedle(g, centerX, centerY, radius, NeedleImages[i], Headings[i]);
                }
                else
                {
                    // Draw heading needle
                    double needleAngle = Math.PI * (Headings[i] - 90) / 180.0; // Offset by 90 degrees
                    int needleX = centerX + (int)(radius * 0.8 * Math.Cos(needleAngle));
                    int needleY = centerY + (int)(radius * 0.8 * Math.Sin(needleAngle));
                    g.DrawLine(new Pen(Color.Red, 2), centerX, centerY, needleX, needleY);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate(); // Redraw on resize
        }
    }
}
