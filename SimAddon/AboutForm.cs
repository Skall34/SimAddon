using System;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace SimAddon
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            LoadVersionInfo();
        }

        private void LoadVersionInfo()
        {
            // Récupérer la version de l'application
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                Version version = assembly.GetName().Version;
                if (version != null)
                {
                    lblVersion.Text = $"Version {version.ToString(3)}";
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://github.com/Skall34/SimAddon",
                    UseShellExecute = true
                });
            }
            catch
            {
                // Ignorer les erreurs silencieusement
            }
        }
    }
}
