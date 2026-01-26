using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SimAddonControls
{
    /// <summary>
    /// Designer personnalisé pour VSStatusStrip
    /// </summary>
    public partial class VSStatusStrip
    {
        /// <summary>
        /// Applique un thème prédéfini
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VSStatusStripTheme Theme
        {
            set
            {
                switch (value)
                {
                    case VSStatusStripTheme.VisualStudioDark:
                        ApplyVisualStudioDarkTheme();
                        break;
                    case VSStatusStripTheme.VisualStudioBlue:
                        ApplyVisualStudioBlueTheme();
                        break;
                    case VSStatusStripTheme.VisualStudioLight:
                        ApplyVisualStudioLightTheme();
                        break;
                }
            }
        }

        /// <summary>
        /// Applique le thème Visual Studio Dark (par défaut)
        /// </summary>
        public void ApplyVisualStudioDarkTheme()
        {
            BackColor = Color.FromArgb(0, 122, 204);
            ForeColor = Color.White;
            BorderColor = Color.FromArgb(0, 100, 180);
            GripColor = Color.FromArgb(70, 70, 74);
        }

        /// <summary>
        /// Applique le thème Visual Studio Blue
        /// </summary>
        public void ApplyVisualStudioBlueTheme()
        {
            BackColor = Color.FromArgb(0, 122, 204);
            ForeColor = Color.White;
            BorderColor = Color.FromArgb(0, 100, 180);
            GripColor = Color.FromArgb(60, 60, 64);
        }

        /// <summary>
        /// Applique le thème Visual Studio Light
        /// </summary>
        public void ApplyVisualStudioLightTheme()
        {
            BackColor = Color.FromArgb(0, 122, 204);
            ForeColor = Color.White;
            BorderColor = Color.FromArgb(0, 100, 180);
            GripColor = Color.FromArgb(200, 200, 200);
        }

        /// <summary>
        /// Applique un thème personnalisé pour correspondre à un formulaire sombre
        /// </summary>
        public void ApplyCustomDarkTheme(Color backColor, Color foreColor)
        {
            BackColor = backColor;
            ForeColor = foreColor;
            
            // Calculer une couleur de bordure légèrement plus sombre
            BorderColor = Color.FromArgb(
                Math.Max(0, backColor.R - 30),
                Math.Max(0, backColor.G - 30),
                Math.Max(0, backColor.B - 30)
            );
            
            GripColor = Color.FromArgb(70, 70, 74);
        }
    }

    /// <summary>
    /// Thèmes prédéfinis pour VSStatusStrip
    /// </summary>
    public enum VSStatusStripTheme
    {
        VisualStudioDark,
        VisualStudioBlue,
        VisualStudioLight
    }
}
