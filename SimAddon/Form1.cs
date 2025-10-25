﻿using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Reflection;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimAddon
{



    public partial class Form1 : Form
    {
        private bool autostart = false;
        private bool autoHide = false;
        private System.Windows.Forms.Timer timerZulu;

        PluginsMgr plugsMgr;
        private PluginsSettings pluginsSettings;

        private simData _simData;
        private situation currentStatus;
        FormWindowState LastWindowState = FormWindowState.Normal;

        Version version;
        Collection<TabPage> pluginTabs;


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
            pluginTabs = new Collection<TabPage>();
            pluginsSettings = new PluginsSettings();
            pluginsSettings.loadFromJsonFile("plugins.json");

            InitializeComponent();

            // Dans Form1.Designer.cs ou dans le constructeur de Form1
            this.timerZulu = new System.Windows.Forms.Timer();
            this.timerZulu.Interval = 1000; // 1 seconde
            this.timerZulu.Tick += new System.EventHandler(this.timerZulu_Tick);

            this.StartPosition = FormStartPosition.Manual;
            Point startlocation = new Point();
            startlocation.X = Properties.Settings.Default.xpos;
            startlocation.Y = Properties.Settings.Default.ypos;
            if (startlocation.X == -32000 || startlocation.Y == -32000)
            {
                //ifthe position is not set, then use the default position
                startlocation.X = 0;
                startlocation.Y = 0;
            }
            this.Location = startlocation;

            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
            alwaysOnTopToolStripMenuItem.Checked = Properties.Settings.Default.AlwaysOnTop;

            autoHide = Properties.Settings.Default.AutoHide;
            autoHideToolStripMenuItem.Checked = autoHide;

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

                currentStatus.COM1Frequency = _simData.GetCOM1();
                currentStatus.COM1StdbyFrequency = _simData.GetCOM1Stdby();
                currentStatus.squawkCode = _simData.GetSquawk();
                currentStatus.squawkMode = _simData.GetSquawkMode(); // 0 = off, 1 = standby, 2 = on, test=3

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
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
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
                    statusText += " / Ready to fly";
                }
                else
                {
                    statusText += " / Not ready to fly...";
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
                    Logger.WriteLine(plugin.getName() +" : " + ex.Message);
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
        private void timerZulu_Tick(object sender, EventArgs e)
        {
            // Format HH:mm:ss 'Z' pour l'heure Zulu
            toolStripHeureZulu.Text = DateTime.UtcNow.ToString("HH:mm:ss 'Z'");

        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            this.timerZulu.Start();
            //initialise l'object qui sert à capter les données qui viennent du simu
            Logger.WriteLine("initialize the connection to the simulator");
            tabControl1.SuspendLayout();
            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    plugin.OnStatusUpdate += Plugin_OnStatusUpdate;
                    plugin.OnSimEvent += Plugin_OnSimEvent;
                    plugin.OnShowMsgbox += Plugin_OnShowMsgbox;
                    plugin.OnShowDialog += Plugin_OnShowDialog;

                    TabPage pluginpage = plugin.registerPage();

                    pluginTabs.Add(pluginpage);
                    //find if there is a plugin setting for this plugin
                    string pluginName = plugin.getName();
                    PluginSettings pluginSetting;
                    if (pluginsSettings.Plugins.ContainsKey(pluginName))
                    {
                        pluginSetting = pluginsSettings.Plugins[plugin.getName()];
                        //if the plugin is not visible, then don't add it to the tab control
                        if (pluginSetting.Visible)
                        {
                            //if there is a setting for this plugin, then add it to the tab control                           
                            tabControl1.TabPages.Add(pluginpage);
                        }
                    }
                    else
                    {
                        //if there is no setting for this plugin, then add it with default visibility
                        pluginSetting = new PluginSettings { Visible = true };
                        pluginsSettings.Plugins[plugin.getName()] = pluginSetting;
                        tabControl1.TabPages.Add(pluginpage);
                    }

                    //add a checkable menu item to the context menu of the tab control
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(plugin.getName(), null, (o, v) => { showHideTab(plugin.getName()); });
                    menuItem.CheckOnClick = true;
                    menuItem.Checked = pluginSetting.Visible; // by default, the plugin is enabled
                    menuItem.Enabled = true;
                    //add the menu item to the context menu of the tab control
                    contextMenuStrip1.Items.Add(menuItem);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
                }
            }
            tabControl1.ResumeLayout(true);
            //save the plugins settings to the file
            pluginsSettings.saveToJsonFile("plugins.json");

            Plugin_OnStatusUpdate(this, "Loading data from server...");
            Logger.WriteLine("Loading data from server...");
            await _simData.loadDataFromSheet();
            //met à jour l'etat de connection au simu dans la barre de statut
            Plugin_OnStatusUpdate(this, "Data loaded.");
            Logger.WriteLine("Data loaded from server.");
            UpdateLabelConnectionStatus();

            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    plugin.init(ref _simData);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
                }
            }
            Cursor = Cursors.Default;

            //demarre le timer de connection (fait un essai de connexion toutes les 1000ms)
            this.timerConnection.Start();

            //// After initialization, check reservation for configured callsign
            //try
            //{
            //    Logger.WriteLine("CheckReservation: starting");
            //    // call and wait so popup appears during startup
            //    await CheckReservationAsync();
            //    Logger.WriteLine("CheckReservation: finished");
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteLine("CheckReservationAsync scheduling failed: " + ex.Message);
            //}
        }


        private void showHideTab(string v)
        {
            bool found = false;
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == v)
                {
                    //toggle the visibility of the tab
                    tabControl1.TabPages.Remove(tab);
                    //update the plugin settings to hide the tab
                    if (pluginsSettings.Plugins.ContainsKey(v))
                    {
                        pluginsSettings.Plugins[v].Visible = false;
                    }
                    else
                    {
                        pluginsSettings.Plugins.Add(v, new PluginSettings { Visible = false });
                    }
                    found = true;
                }
            }
            //if not found, then add the tab
            if (!found)
            {
                //find the plugin with the name v
                foreach (TabPage plugin in pluginTabs)
                {
                    if (plugin.Text == v)
                    {
                        tabControl1.TabPages.Add(plugin);
                        //update the plugin settings to show the tab
                        if (pluginsSettings.Plugins.ContainsKey(v))
                        {
                            pluginsSettings.Plugins[v].Visible = true;
                        }
                        else
                        {
                            pluginsSettings.Plugins.Add(v, new PluginSettings { Visible = true });
                        }
                    }
                }
            }
            //save the plugins settings to the file
            pluginsSettings.saveToJsonFile("plugins.json");
        }

        private DialogResult Plugin_OnShowMsgbox(object sender, string text, string caption, MessageBoxButtons buttons)
        {
            //if the window is top most, disable the topmost before showing the message box.
            bool wasWindowTopMost = this.TopMost;

            if (this.TopMost)
            {
                this.TopMost = false;
            }
            DialogResult result = MessageBox.Show(text, caption, buttons);

            //restore the topmost as it was before the popup.
            this.TopMost = wasWindowTopMost;
            return result;
        }

        private DialogResult Plugin_OnShowDialog(object sender, Form dialog)
        {
            //if the window is top most, disable the topmost before showing the dialog.
            bool wasWindowTopMost = this.TopMost;
            if (this.TopMost)
            {
                this.TopMost = false;
            }
            DialogResult result = dialog.ShowDialog();
            //restore the topmost as it was before the popup.
            this.TopMost = wasWindowTopMost;
            return result;
        }

        private void Plugin_OnSimEvent(SimAddonPlugin.ISimAddonPluginCtrl sender, SimEventArg eventArg)
        {
            Logger.WriteLine("Event received from plugin : " + sender.getName() + " " + eventArg.reason.ToString() + " value=" + eventArg.value);
            switch (eventArg.reason)
            {
                case SimEventArg.EventType.ENGINESTART:
                    SetEngineStart();
                    break;
                case SimEventArg.EventType.ENGINESTOP:
                    SetEngineStop();
                    break;
                case SimEventArg.EventType.TAKEOFF:
                    SetTakeoff();
                    break;
                case SimEventArg.EventType.LANDING:
                    SetLanding();
                    break;
                case SimEventArg.EventType.SETCALLSIGN:
                    SetCallsign(sender, eventArg.value);
                    break;
                case SimEventArg.EventType.SETDESTINATION:
                    SetDestination(sender, eventArg.value);
                    break;
                default:
                    break;
            }

            //push the event to all the plugins
            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    //don't send the event to the plugin that sent it
                    if (plugin != sender)
                    {
                        plugin.ManageSimEvent(sender, eventArg);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
                }
            }
        }

        public void SetEngineStart()
        {
            //what to do in application if flight recorder detected an engine start ?
            if (autoHide)
            {
                LastWindowState = this.WindowState;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        public void SetEngineStop()
        {
            //what to do in application if flight recorder detected an engine stop ?
            if (autoHide)
            {
                this.WindowState = LastWindowState;
            }
        }

        public void SetTakeoff()
        {
            //what to do in application if flight recorder detected a takeoff ?
//            if (autoHide)
//            {
//                LastWindowState = this.WindowState;
//                this.WindowState = FormWindowState.Minimized;
//            }
        }

        public void SetLanding()
        {
            //what to do in application if flight recorder detected a landing ?
//            if (autoHide)
//            {
//                this.WindowState = LastWindowState;
//            }
        }

        public void SetCallsign(object sender, string callsign)
        {

        }

        public void SetDestination(object sender, string destination)
        {
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

        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TopMost == false)
            {
                this.TopMost = alwaysOnTopToolStripMenuItem.Checked = true;
                this.TopMost = true;
            }
            else
            {
                this.TopMost = alwaysOnTopToolStripMenuItem.Checked = false;
                this.TopMost = false;
            }
            Properties.Settings.Default.AlwaysOnTop = this.TopMost;
            Properties.Settings.Default.Save();
        }

        private void autoHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoHide = !autoHide;
            autoHideToolStripMenuItem.Checked = autoHide;
            Properties.Settings.Default.AutoHide = autoHide;
            Properties.Settings.Default.Save();
        }

        private void screenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the primary screen
            var primaryScreen = Screen.PrimaryScreen;

            // Get the bounds of the primary screen
            var bounds = primaryScreen.Bounds;

            // Create a bitmap with the size of the primary screen
            using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                // Draw the screen into the bitmap
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
                }

                // Save the screenshot (e.g., as a PNG file)
                string filePath = "screenshot.png";
                bitmap.Save(filePath, ImageFormat.Png);

                // Load the screenshot into the clipboard
                Clipboard.SetImage(bitmap);

                Logger.WriteLine($"Screenshot saved to {filePath}");
                Console.WriteLine($"Screenshot saved to {filePath}");
                Plugin_OnShowMsgbox(this, $"Screenshot copied to clipboard", "Screenshort copied", MessageBoxButtons.OK);
            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            // Save the position of the window when it is moved, but not when minimized
            if (this.WindowState != FormWindowState.Minimized)
            {
                Properties.Settings.Default.xpos = this.Location.X;
                Properties.Settings.Default.ypos = this.Location.Y;
                Properties.Settings.Default.Save();
            }
        }

        private void openWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open the web site starting the default browser
            try
            {
                //start the default web browser to view the url stored in Properties.Settings.Default.GSheetAPIUrl
                string url = Properties.Settings.Default.GSheetAPIUrl;
                if (string.IsNullOrEmpty(url))
                {
                    Logger.WriteLine("No URL configured in settings. Please set the URL in the settings.");
                    Plugin_OnShowMsgbox(this, "Error", "No URL configured in settings. Please set the URL in the settings.", MessageBoxButtons.OK);
                    return;
                }
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // This is necessary to open the URL in the default browser
                });
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error opening web site: {ex.Message}");
                Plugin_OnShowMsgbox(this, "Error", "Unable to open the web site. Please check the URL.", MessageBoxButtons.OK);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void traceFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the application name
            string appName = Assembly.GetEntryAssembly().GetName().Name;

            // Get the path to the user's AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the AppData path with the folder name
            string fullPath = Path.Combine(appDataPath, appName);

            //open fullPath in the file explorer
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = fullPath,
                UseShellExecute = true, // This is necessary to open the URL in the default browser
                Verb = "open" // This is necessary to open the folder in the file explorer
            });
        }
    }
}
