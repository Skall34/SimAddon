
using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Reflection;

namespace SimAddon
{


    public partial class Form1 : Form
    {
        private bool autostart = false;

        PluginsMgr plugsMgr;

        private simData _simData;
        private situation currentStatus;
        FormWindowState LastWindowState = FormWindowState.Normal;

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

            plugsMgr = new PluginsMgr();
            plugsMgr.LoadPluginsFromFolder("plugins", tabControl1);
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
                AssemblyName assemblyName = assembly.GetName();
                version = assembly.GetName().Version;
                if (null == version)
                {
                    version = new Version("unknown");
                }
                // Set the form's title to include the version number
                Text = $"SimAddon - Version {version.ToString(3)}";
                if (autostart)
                {
                    Text += " autostarted";
                }
                Logger.WriteLine($"Version : {version.ToString(3)}");
            }
            else
            {
                version = new Version("unknown");
            }

            //create the object to get the dat from sim and the structure to push situation update to plugins
            _simData = new simData(Properties.Settings.Default.GSheetAPIUrl);
            currentStatus = new situation();

            this.Cursor = Cursors.WaitCursor;

            LastWindowState = WindowState;
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
                UpdateLabelConnectionStatus();

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
                UpdateLabelConnectionStatus();
                //rafraichis les données venant du simu
                _simData.Refresh();

                currentStatus.magVariation = _simData.GetMagVariation();
                currentStatus.readyToFly = _simData.GetReadyToFly();
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
                currentStatus.MasterAvionicsOn = (0 != _simData.GetAvionicsMaster());
                currentStatus.MasterBatteryOn = (0 != _simData.GetBatteryMaster());

                //byte ViewMode = _simData.GetViewMode();
                //lblConnectionStatus.Text = "viewMode " + ViewMode;
            }
            catch (Exception ex)
            {
                //log the excepction
                Logger.WriteLine("Communication with FSUIPC Failed\n\n" + ex.Message);
                // An error occured. Tell the user and stop this timer.
                this.timerMain.Stop();
                // Update the connection status
                UpdateLabelConnectionStatus();

                //send a last update to the plugins.
                currentStatus.MasterAvionicsOn = false;
                currentStatus.MasterBatteryOn = false;

                // re-start the connection timer
                this.timerConnection.Start();
            }

            //send the update to the plugins.
            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    plugin.updateSituation(currentStatus);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }

            }
        }

        // Configures the status label depending on if we're connected or not 
        private void UpdateLabelConnectionStatus()
        {
            string statusText = string.Empty;
            //si la connection vers le simu est OK
            if (_simData.isConnected)
            {
                statusText = "Connected";
                this.lblConnectionStatus.ForeColor = Color.Green;
                if (_simData.GetReadyToFly())
                {
                    statusText += " / Ready";
                }
                else
                {
                    statusText += " Waiting for flight load...";
                }
            }
            else
            {
                statusText = "Disconnected. Looking for Simulator...";
                this.lblConnectionStatus.ForeColor = Color.Red;

            }
            this.lblConnectionStatus.Text = statusText;
        }

        // Form is closing so stop all the timers and close FSUIPC Connection
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            FormClosingEventArgs e2;
            if (autostart)
            {
                //this can't be canceled
                e2 = new FormClosingEventArgs(CloseReason.ApplicationExitCall, false);
            }
            else
            {
                //this is normal user closing
                e2 = new FormClosingEventArgs(CloseReason.UserClosing, false);
            }

            //request for termination on all plugins
            //if one replies false, then cancel termination.
            bool isCanceled = false;
            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    plugin.FormClosing(sender, e2);
                    isCanceled |= e2.Cancel;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }
            }
            e.Cancel = isCanceled;

            if (!isCanceled)
            {
                _simData.CloseConnection();
                //stop and flush the traces 
                Logger.Dispose();
            }
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
                    plugin.OnStatusUpdate += Plugin_OnStatusUpdate;
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
            UpdateLabelConnectionStatus();

            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    plugin.init(ref _simData);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }
            }
            Cursor = Cursors.Default;

            //demarre le timer de connection (fait un essai de connexion toutes les 1000ms)
            this.timerConnection.Start();

        }

        //write the message the status bar
        private void Plugin_OnStatusUpdate(object sender, string statusMessage)
        {
            this.lblPluginStatus.Text = statusMessage;
            this.lblPluginStatus.ForeColor = Color.Green;
        }

        private void submitBugToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState != LastWindowState)
            {
                LastWindowState = WindowState;
                if (WindowState == FormWindowState.Maximized)
                {
                    foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
                    {
                        try
                        {
                            plugin.SetWindowMode(ISimAddonPluginCtrl.WindowMode.FULL);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine(ex.Message);
                        }
                    }
                }

                if (WindowState == FormWindowState.Normal)
                {
                    foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
                    {
                        try
                        {
                            plugin.SetWindowMode(ISimAddonPluginCtrl.WindowMode.COMPACT);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine(ex.Message);
                        }
                    }

                }

            }
        }
    }
}
