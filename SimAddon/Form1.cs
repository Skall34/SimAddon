using SimAddon.Properties;
//using FSUIPC;
using System;
using System.Collections.Generic;
//using System.Collections.Immutable;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using SimDataManager;
using static SimDataManager.simData;
using SimAddonPlugin;
using SimAddonLogger;

namespace SimAddon
{


    public partial class Form1 : Form
    {
        private bool autostart = false;

        PluginsMgr plugsMgr;
      
        private simData _simData;

        Version version;

        public static bool isAutoStart()
        {
            // Récupérer les paramètres de la ligne de commande
            string[] args = Environment.GetCommandLineArgs();

            // Vérifier si "-auto" est présent dans les arguments
            foreach (string arg in args)
            {
                if (arg.Equals("-auto", StringComparison.OrdinalIgnoreCase))
                {
                    return true; // Retourne true si "-auto" est trouvé
                }
            }

            return false; // Retourne false si "-auto" n'est pas trouvé
        }

        //private bool modifiedFuel;
        public Form1()
        {
            InitializeComponent();

            plugsMgr= new PluginsMgr();
            plugsMgr.LoadPluginsFromFolder("plugins",tabControl1);
            autostart = isAutoStart();

            //initialize the trace mechanism
            Logger.init();

            Logger.WriteLine("Starting SimAddon");
            if (autostart)
            {
                Logger.WriteLine("Autostarted");
            }

            // Get the version information of your application
            Assembly assembly = Assembly.GetEntryAssembly();
            if (null != assembly)
            {
                version = assembly.GetName().Version;
                if (null == version)
                {
                    version = new Version("unknown");
                }
                // Set the form's title to include the version number
                this.Text = $"SimAddon - Version {version.ToString(3)}";
                if (autostart)
                {
                    this.Text += " autostarted";
                }
                Logger.WriteLine($"Version : {version.ToString(3)}");
            }
            else
            {
                version = new Version("unknown");
            }

            _simData = new simData();

            this.Cursor = Cursors.WaitCursor;


        }


        // appelé chaque 1s par le timer de connection
        private void TimerConnection_Tick(object sender, EventArgs e)
        {
            // Try to open the connection
            try
            {
                //if ((aeroports.Count > 0) && (avions.Count > 0) && (missions.Count > 0))
                //{

                //essaie d'ouvrir la connection. Si ça échoue, une exception sera envoyée
                _simData.OpenConnection();

                Logger.WriteLine("Connected to simulator");
                //si on arrive ici, la connection est bien ouverte, arrete le timer de connection.
                this.timerConnection.Stop();

                //read the value that normally don't change (except change of plane, or cargo)
                //_simData.ReadStaticValues();

                //demarre le timer principal, qui lit les infos du simu 2x par 1s.
                this.timerMain.Start();

                // met à jour le status de connection dans la barre de statut.
                ConfigureForm();

            }
            catch
            {
                // No connection found. Don't need to do anything, just keep trying
                if (autostart)
                {
                    //if flight recorder was started automatically by the simulator, then exit when simulator is not there anymore.
                    System.Windows.Forms.Application.Exit();
                }
            }
        }


        // This method runs 2 times per second (every 500ms). This is set on the timerMain properties.
        private async void TimerMain_Tick(object sender, EventArgs e)
        {
            try
            {

                //rafraichis les données venant du simu
                _simData.Refresh();

                //tell the plugins to update
                situation currentStatus=new situation();
                currentStatus.airSpeed = _simData.GetAirSpeed();
                currentStatus.crashedFlag = _simData.GetCrashedFlag();
                currentStatus.currentFuel = _simData.GetFuelWeight();
                currentStatus.flapsAvailableFlag = _simData.GetFlapsAvailableFlag();
                currentStatus.flapsPosition = _simData.GetFlapsPosition();
                currentStatus.gearIsUp = _simData.GetIsGearUp();
                currentStatus.gearRetractableFlag = _simData.GetGearRetractableFlag();
                currentStatus.isAtLeastOneEngineFiring = _simData.IsAtLeastOneEngineFiring();
                currentStatus.landingVerticalSpeed = _simData.GetLandingVerticalSpeed();
                currentStatus.offRunwayCrashed = _simData.GetOffRunwayCrashed();
                currentStatus.onGround = _simData.GetOnground();
                currentStatus.overSpeedWarning = _simData.GetOverspeedWarning();
                currentStatus.payload = _simData.GetPayload();
                currentStatus.planeWeight = _simData.GetPlaneWeight();                    
                currentStatus.stallWarning = _simData.GetStallWarning();
                currentStatus.position = _simData.GetPosition();

                foreach(ISimAddonPluginCtrl plugin in plugsMgr.plugins)
                {
                    try
                    {
                        plugin.updateSituation(currentStatus);
                    }catch(Exception ex)
                    {
                        Logger.WriteLine(ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                //log the excepction
                Logger.WriteLine("Communication with FSUIPC Failed\n\n" + ex.Message);
                // An error occured. Tell the user and stop this timer.
                this.timerMain.Stop();
                // Update the connection status
                ConfigureForm();
                // re-start the connection timer
                this.timerConnection.Start();
            }
        }

        // Configures the status label depending on if we're connected or not 
        private void ConfigureForm()
        {
            //si la connection vers le simu est OK
            if (_simData.isConnected)
            {
                this.lblConnectionStatus.Text = "Connected";
                this.lblConnectionStatus.ForeColor = Color.Green;
            }
            else
            {
                this.lblConnectionStatus.Text = "Disconnected. Looking for Flight Simulator...";
                this.lblConnectionStatus.ForeColor = Color.Red;
            }
        }

        // Form is closing so stop all the timers and close FSUIPC Connection
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            //string message = "Confirm close ACARS ?";
            //if (this.btnSubmit.Enabled == true)
            //{
            //    //Le vol n'a PAS été envoyé
            //    message += "\r\n !!! Le vol n'a pas été envoyé !!!";
            //}
            //DialogResult res = DialogResult.Cancel;

            //if (!autostart)
            //{
            //    res = MessageBox.Show(message, "Flight Recorder", MessageBoxButtons.OKCancel);
            //}
            //else
            //{
            //    //si il y a eu un vol, ET que les moteurs sont arretés, envoie le vol vers la google sheet
            //    if ((this.btnSubmit.Enabled == true) && (!atLeastOneEngineFiring))
            //    {
            //        saveFlight();
            //    }
            //}



            //if ((res == DialogResult.OK) || (autostart))
            //{
            //    if (atLeastOneEngineFiring)
            //    {
            //        this.Cursor = Cursors.WaitCursor;
            //        this.lblConnectionStatus.Text = "Freeing plane...";
            //        this.lblConnectionStatus.ForeColor = Color.Green;
            //        // Libère l'avion sur le fichier en cas de fermeture de l'acars avant la fin du vol
            //        // on ne le fait que si un moteur tourne encore ==> vol interrompu avant la fin
            //        UpdatePlaneStatus(0);
            //        cbImmat.Enabled = true;
            //        //tbEndICAO.Enabled = true;
            //        System.Threading.Thread.Sleep(2000);
            //        this.Cursor = Cursors.Default;
            //    }
            //    //arrete les timers.
            //    this.timerConnection.Stop();
            //    this.timerMain.Stop();
            //    //ferme la connection vers le simu
            //    FSUIPCConnection.Close();

            //    //stop and flush the traces 
            //    Logger.Dispose();
            //}
            //else
            //{
            //    // Si l'utilisateur clique sur Annuler, annule la fermeture de la fenêtre.
            //    e.Cancel = true;
            //}
        }

        private void Form1_Activated(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //initialise l'object qui sert à capter les données qui viennent du simu
            Logger.WriteLine("initialize the connection to the simulator");
            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    plugin.registerPage(tabControl1);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }
            }

            await _simData.loadDataFromSheet();
            //met à jour l'etat de connection au simu dans la barre de statut
            Logger.WriteLine("update form status");
            ConfigureForm();

            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins) {
                try
                {
                    plugin.init(ref _simData);
                }catch(Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }
            }
            Cursor = Cursors.Default;

            //demarre le timer de connection (fait un essai de connexion toutes les 1000ms)
            this.timerConnection.Start();

        }

        private void submitBugToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

    }
}
