using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SimAddonControls
{
    /// <summary>
    /// Designer personnalisé pour VSTabControl
    /// </summary>
    [Designer(typeof(ParentControlDesigner))]
    public partial class VSTabControl
    {
        /// <summary>
        /// Applique un thème prédéfini
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public VSTabControlTheme Theme
        {
            set
            {
                switch (value)
                {
                    case VSTabControlTheme.VisualStudioDark:
                        ApplyVisualStudioDarkTheme();
                        break;
                    case VSTabControlTheme.VisualStudioBlue:
                        ApplyVisualStudioBlueTheme();
                        break;
                    case VSTabControlTheme.VisualStudioLight:
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
            TabBackColor = Color.FromArgb(45, 45, 48);
            TabSelectedBackColor = Color.MidnightBlue;
            TabHoverBackColor = Color.DarkGray;
            TabTextColor = Color.White;
            TabSelectedTextColor = Color.White;
            AccentColor = Color.FromArgb(0, 122, 204);
            TabPageBackColor = Color.FromArgb(37, 37, 38);
            BackColor = Color.FromArgb(37, 37, 38); // Même couleur que TabPageBackColor
        }

        /// <summary>
        /// Applique le thème Visual Studio Blue
        /// </summary>
        public void ApplyVisualStudioBlueTheme()
        {
            TabBackColor = Color.FromArgb(41, 57, 85);
            TabSelectedBackColor = Color.FromArgb(0, 122, 204);
            TabHoverBackColor = Color.FromArgb(51, 67, 95);
            TabTextColor = Color.White;
            TabSelectedTextColor = Color.White;
            AccentColor = Color.FromArgb(0, 122, 204);
            TabPageBackColor = Color.FromArgb(37, 37, 38);
            BackColor = Color.FromArgb(37, 37, 38); // Même couleur que TabPageBackColor
        }

        /// <summary>
        /// Applique le thème Visual Studio Light
        /// </summary>
        public void ApplyVisualStudioLightTheme()
        {
            TabBackColor = Color.FromArgb(245, 245, 245);
            TabSelectedBackColor = Color.White;
            TabHoverBackColor = Color.FromArgb(200, 200, 200);
            TabTextColor = Color.Black;
            TabSelectedTextColor = Color.Black;
            AccentColor = Color.FromArgb(0, 122, 204);
            TabPageBackColor = Color.White;
            BackColor = Color.White; // Même couleur que TabPageBackColor
        }
    }

    /// <summary>
    /// Thèmes prédéfinis pour VSTabControl
    /// </summary>
    public enum VSTabControlTheme
    {
        VisualStudioDark,
        VisualStudioBlue,
        VisualStudioLight
    }
}
