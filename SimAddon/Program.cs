using System;
using System.Threading;
using System.Windows.Forms;

namespace SimAddon
{
    internal static class Program
    {
        private static Mutex singleInstanceMutex;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Vérification instance unique
            bool createdNew = false;
            singleInstanceMutex = new Mutex(true, "SimAddonSingleInstanceMutex", out createdNew);
            
            if (!createdNew)
            {
                MessageBox.Show("Another instance is already running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Sortie propre de l'application
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //ApplicationConfiguration.Initialize();
            
            try
            {
                Application.Run(new Form1());
            }
            finally
            {
                // Libérer le mutex à la fermeture
                if (singleInstanceMutex != null)
                {
                    singleInstanceMutex.ReleaseMutex();
                    singleInstanceMutex.Dispose();
                }
            }
        }
    }
}