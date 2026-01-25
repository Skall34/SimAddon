using SimAddon.Properties;
using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SimAddonPlugin.ISimAddonPluginCtrl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimAddon
{
    public partial class Form1 : Form
    {
        private LoadingForm splashScreen;
        private bool autostart = false;
        private bool hasBeenConnected = false;
        private bool autoHide = false;
        private System.Windows.Forms.Timer timerZulu;
        private UpdateChecker updateChecker;

        PluginsMgr plugsMgr;
        private PluginsSettings pluginsSettings;

        private simData _simData;
        private situation currentStatus;
        FormWindowState LastWindowState = FormWindowState.Normal;

        Version version;
        Collection<TabPage> pluginTabs;

        // Auto-hide border feature
        private double transparencyLevel = 1.0;
        private System.Windows.Forms.Timer mouseCheckTimer;
        private bool isMouseOverForm = false;

        // Window dragging support
        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Window resizing support
        private bool isResizing = false;
        private Point resizeStartPoint;
        private Size resizeStartSize;
        private const int RESIZE_BORDER = 10;

        Dictionary<string, string> flightRecords;
        string departureAirport = "";
        string arrivalAirport = "";
        List<string> screenshots;

        DateTime flightStartTime;
        DateTime flightEndTime;


        // Méthode pour vérifier la présence de l'argument "-auto"
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

        // helper thread-safe pour mettre à jour le splash
        private void SetSplashProgress(int value, string status)
        {
            if (splashScreen == null) return;
            int v = Math.Max(0, Math.Min(100, value));
            if (splashScreen.IsHandleCreated && splashScreen.InvokeRequired)
            {
                splashScreen.Invoke((Action)(() => splashScreen.updateProgress(v, status)));
            }
            else
            {
                try { splashScreen.updateProgress(v, status); } catch { }
            }
        }

        private void WriteWindowTitle()
        {
            // Get the version information of your application
            Assembly? assembly = Assembly.GetEntryAssembly();
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
            Text += " on " + Properties.Settings.Default.FlyingNetwork;
        }

        //private bool modifiedFuel;
        public Form1()
        {
            pluginTabs = new Collection<TabPage>();
            pluginsSettings = new PluginsSettings();
            pluginsSettings.loadFromJsonFile("plugins.json");

            // Initialiser le vérificateur de mises à jour
            updateChecker = new UpdateChecker();

            InitializeComponent();

            // create and show splash as early as possible
            splashScreen = new LoadingForm();
            splashScreen.StartPosition = FormStartPosition.CenterScreen;
            splashScreen.TopMost = true;
            splashScreen.Show();
            // set initial progress and ensure UI updates
            SetSplashProgress(5, "Initializing...");
            Application.DoEvents();

            splashScreen.Progress = 0;

            splashScreen.Show();

            splashScreen.BringToFront();

            splashScreen.Refresh();

            splashScreen.Update();

            splashScreen.TopMost = true;

            splashScreen.Invalidate();

            splashScreen.Update();

            splashScreen.Refresh();

            // Dans Form1.Designer.cs ou dans le constructeur de Form1
            this.timerZulu = new System.Windows.Forms.Timer();
            this.timerZulu.Interval = 1000; // 1 seconde
            this.timerZulu.Tick += new System.EventHandler(this.timerZulu_Tick);

            // Timer pour vérifier la position de la souris
            this.mouseCheckTimer = new System.Windows.Forms.Timer();
            this.mouseCheckTimer.Interval = 100; // Vérifier toutes les 100ms
            this.mouseCheckTimer.Tick += new System.EventHandler(this.mouseCheckTimer_Tick);

            // Activer le déplacement de la fenêtre via le MenuStrip
            this.menuStrip1.MouseDown += new MouseEventHandler(this.menuStrip1_MouseDown);
            this.menuStrip1.MouseMove += new MouseEventHandler(this.menuStrip1_MouseMove);
            this.menuStrip1.MouseUp += new MouseEventHandler(this.menuStrip1_MouseUp);

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

            autoHide = Properties.Settings.Default.AutoHide;
            // Charger le niveau de transparence
            transparencyLevel = Properties.Settings.Default.TransparentWindow;
            if (transparencyLevel < 1.0)
            {
                ToggleTransparency(transparencyLevel);
            }
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;

            SetSplashProgress(10, "Searching for plugins...");
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

            // Vérifier les mises à jour de manière asynchrone sans bloquer l'UI
            _ = CheckForUpdatesAsync();

            //create the object to get the dat from sim and the structure to push situation update to plugins
            _simData = new simData(Properties.Settings.Default.GSheetAPIUrl);
            _simData.useFlyingNetwork(Properties.Settings.Default.FlyingNetwork);
            if (Settings.Default.FlyingNetwork == FlyingNetwork.VATSIM.ToString())
            {
                vATSIMToolStripMenuItem1.Checked = true;
            }
            else if (Settings.Default.FlyingNetwork == FlyingNetwork.IVAO.ToString())
            {
                iVAOToolStripMenuItem1.Checked = true;
            }

            WriteWindowTitle();

            currentStatus = new situation();
            currentStatus.counter = 0;

            this.Cursor = Cursors.WaitCursor;

            //this will hold the list of screenshots taken during the flight
            screenshots = new List<string>();
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
                hasBeenConnected = true;
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
                if (autostart && hasBeenConnected)
                {
                    Logger.WriteLine("Simulator not found, exiting...");
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
                currentStatus.counter++;
                //si le counter arrive à 1000, le remet à 0 pour éviter un overflow
                if (currentStatus.counter >= 1000)
                {
                    currentStatus.counter = 0;
                }

                //remplis la structure currentStatus avec les données venant du simu
                currentStatus.timestamp = _simData.GetSimDateTimeUTC();
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
                currentStatus.totalFuelFlow = _simData.GetFuelFlow();
                currentStatus.engine1ManifoldPressure = _simData.GetEngine1ManifoldPressure();
                currentStatus.engine1RPM = _simData.GetEngine1RPM();

                currentStatus.verticalSpeed = _simData.GetVerticalSpeed();
                currentStatus.verticalAcceleration = _simData.GetVerticalAcceleration();

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
            if (_simData.isConnectedToSim)
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
                Logger.WriteLine("Autostarted, closing without confirmation.");
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
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
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

        private void timerZulu_Tick(object sender, EventArgs e)
        {
            // Format HH:mm:ss 'Z' pour l'heure Zulu
            toolStripHeureZulu.Text = DateTime.UtcNow.ToString("HH:mm:ss 'Z'");

        }

        private void mouseCheckTimer_Tick(object sender, EventArgs e)
        {
            if (transparencyLevel >= 1.0) return;

            // Obtenir la position de la souris par rapport à l'écran
            Point mousePos = Control.MousePosition;

            // Vérifier si la souris est sur la fenêtre
            bool mouseIsOver = this.Bounds.Contains(mousePos);

            // Si l'état a changé
            if (mouseIsOver != isMouseOverForm)
            {
                isMouseOverForm = mouseIsOver;

                // Sauvegarder TopMost avant de changer l'opacité
                bool wasTopMost = this.TopMost;

                // Changer l'opacité de la fenêtre
                this.Opacity = mouseIsOver ? 1.0 : transparencyLevel;

                // Restaurer TopMost si nécessaire
                if (this.TopMost != wasTopMost)
                {
                    this.TopMost = wasTopMost;
                }
            }
        }

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        private void menuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void menuStrip1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void ToggleTransparency(double opacity)
        {
            // S'assurer que l'opacité est dans les limites valides
            transparencyLevel = Math.Max(0.2, Math.Min(1.0, opacity));

            if (transparencyLevel < 1.0)
            {
                // Démarrer le timer de vérification
                mouseCheckTimer.Start();

                // Vérifier immédiatement l'état de la souris
                Point mousePos = Control.MousePosition;
                isMouseOverForm = this.Bounds.Contains(mousePos);

                // Appliquer l'opacité appropriée
                if (!isMouseOverForm)
                {
                    // Sauvegarder TopMost avant de changer l'opacité
                    bool wasTopMost = this.TopMost;

                    this.Opacity = transparencyLevel;

                    // Restaurer TopMost
                    if (!this.TopMost && wasTopMost)
                    {
                        this.TopMost = wasTopMost;
                    }
                }
            }
            else
            {
                // Arrêter le timer
                mouseCheckTimer.Stop();

                // Sauvegarder TopMost avant de restaurer l'opacité
                bool wasTopMost = this.TopMost;

                // Restaurer l'opacité normale
                this.Opacity = 1.0;
                isMouseOverForm = false;

                // Restaurer TopMost
                if (!this.TopMost && wasTopMost)
                {
                    this.TopMost = wasTopMost;
                }
            }
        }


        private async Task<bool> connectToSite()
        {
            bool result = false;
            //check connection to data server
            if ((Settings.Default.SessionToken == "") || (!await _simData.checkSession(Settings.Default.SessionToken)))
            {
                string sessionToken = await _simData.loginToSite();
                if (sessionToken == "")
                {
                    Logger.WriteLine("Login failed");
                }
                else
                {
                    Logger.WriteLine("Login OK");
                    Settings.Default.SessionToken = sessionToken;
                    Settings.Default.Save();
                    result = true;
                }
            }
            else
            {
                Logger.WriteLine("Already logged in to data server");
                result = true;
            }
            return result;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.timerZulu.Start();
            //initialise l'object qui sert à capter les données qui viennent du simu
            Logger.WriteLine("initialize the connection to the simulator");

            // show splash progress while loading UI/plugins/data
            SetSplashProgress(10, "Loading plugins...");

            // Configure TabControl appearance like Visual Studio
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.ItemSize = new Size(80, 20);
            tabControl1.DrawItem += TabControl1_DrawItem;

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
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
                }
            }
            tabControl1.ResumeLayout(true);
            //save the plugins settings to the file
            pluginsSettings.saveToJsonFile("plugins.json");

            SetSplashProgress(30, "Connecting to site...");
            //connect to the web site to check session token, or get a new one if needed
            Logger.WriteLine("Connecting to data server...");
            bool loggedin = await connectToSite();
            SetSplashProgress(40, "Loading data from server...");
            Plugin_OnStatusUpdate(this, "Loading data from server...");
            Logger.WriteLine("Loading data from server...");
            await _simData.loadDataFromSheet();
            //met à jour l'etat de connection au simu dans la barre de statut
            Plugin_OnStatusUpdate(this, "Data loaded.");
            Logger.WriteLine("Data loaded from server.");
            UpdateLabelConnectionStatus();

            SetSplashProgress(50, "Initializing plugins...");
            int nblugins = plugsMgr.plugins.Count;
            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    SetSplashProgress(nblugins > 0 ? 50 + (45 / nblugins) : 95, "Initializing plugin " + plugin.getName() + "...");
                    plugin.init(ref _simData);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
                }
            }
            Cursor = Cursors.Default;

            SetSplashProgress(98, "Starting...");

            //demarre le timer de connection (fait un essai de connexion toutes les 1000ms)
            this.timerConnection.Start();

            SetSplashProgress(100, "done");

            // close/hide splash
            try
            {
                if (splashScreen != null)
                {
                    splashScreen.Close();
                    splashScreen.Dispose();
                    splashScreen = null;
                }
            }
            catch { }

        }

        private void showTab(string v)
        {
            //verify if the tab is already shown
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == v)
                {
                    return;
                }
            }
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
            //save the plugins settings to the file
            pluginsSettings.saveToJsonFile("plugins.json");
        }

        private void hideTab(string v)
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Text == v)
                {
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
            //show the message box above the current window
            if (wasWindowTopMost)
            {

                DialogResult result = MessageBox.Show(this, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                //restore the topmost as it was before the popup.
                this.TopMost = wasWindowTopMost;
                return result;
            }
            else
            {
                DialogResult result = MessageBox.Show(this, text, caption, buttons);
                return result;
            }
        }

        private DialogResult Plugin_OnShowDialog(object sender, Form dialog)
        {
            //if the window is top most, disable the topmost before showing the dialog.
            bool wasWindowTopMost = this.TopMost;
            if (this.TopMost)
            {
                this.TopMost = false;
            }

            //open the dialog above the current window
            dialog.StartPosition = FormStartPosition.CenterParent;

            DialogResult result = dialog.ShowDialog();
            //restore the topmost as it was before the popup.
            this.TopMost = wasWindowTopMost;
            return result;
        }

        private void Plugin_OnSimEvent(SimAddonPlugin.ISimAddonPluginCtrl sender, SimEventArg eventArg)
        {
            Logger.WriteLine("Event received from plugin : " + sender.getName() + " " + eventArg.reason.ToString() + " value=" + eventArg.value);
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

            try
            {
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
                    case SimEventArg.EventType.CRASHING:
                        //what to do in application if flight recorder detected a crash ?
                        SetCrash(sender, eventArg.value);
                        break;
                    case SimEventArg.EventType.SETAIRCRAFT:
                        //what to do in application if flight recorder detected a change of aircraft ?
                        SetAircraft(sender, eventArg.value);
                        break;
                    case SimEventArg.EventType.SETDEPARTURE:
                        //what to do in application if flight recorder detected a change of departure ?
                        SetDeparture(sender, eventArg.value);
                        break;
                    case SimEventArg.EventType.ENDOFFLIGHT:
                        //what to do in application if flight recorder detected end of flight ?
                        SetEndOfFlight(sender, eventArg.value);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Error handling event from plugin " + sender.getName() + " : " + ex.Message);
            }

        }

        private void SetEndOfFlight(ISimAddonPluginCtrl sender, string value)
        {
            arrivalAirport = value;
            flightEndTime = DateTime.Now;
        }

        private void SetDeparture(ISimAddonPluginCtrl sender, string value)
        {
            departureAirport = value;
        }

        private void SetAircraft(ISimAddonPluginCtrl sender, string value)
        {
        }

        private void SetCrash(ISimAddonPluginCtrl sender, string value)
        {
        }

        public void SetEngineStart()
        {
            //what to do in application if flight recorder detected an engine start ?
            if (autoHide)
            {
                LastWindowState = this.WindowState;
                this.WindowState = FormWindowState.Minimized;
            }
            flightRecords = new Dictionary<string, string>();
            flightStartTime = DateTime.Now;

            generateFlightReportToolStripMenuItem1.Enabled = true;
        }

        public void SetEngineStop()
        {
            //what to do in application if flight recorder detected an engine stop ?
            if (autoHide)
            {
                this.WindowState = LastWindowState;
            }
            flightEndTime = DateTime.Now;

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
            arrivalAirport = destination;
        }

        //write the message the status bar
        private void Plugin_OnStatusUpdate(object sender, string statusMessage)
        {
            this.lblPluginStatus.Text = statusMessage;
            this.lblPluginStatus.ForeColor = Color.Green;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

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

        /// <summary>
        /// Vérifie asynchronement si une nouvelle version est disponible
        /// </summary>
        private async Task CheckForUpdatesAsync()
        {
            try
            {
                // Donner un peu de temps à l'interface de se charger
                await Task.Delay(2000);

                UpdateChecker.ReleaseInfo releaseInfo = await updateChecker.CheckForUpdatesAsync(includePrerelease: false);

                if (releaseInfo != null)
                {
                    // Afficher la boîte de dialogue sur le thread UI
                    if (this.InvokeRequired)
                    {
                        this.Invoke((Action)(() =>
                        {
                            if (UpdateChecker.ShowUpdateDialog(releaseInfo, this))
                            {
                                UpdateChecker.OpenDownloadPage(releaseInfo);
                            }
                        }));
                    }
                    else
                    {
                        if (UpdateChecker.ShowUpdateDialog(releaseInfo, this))
                        {
                            UpdateChecker.OpenDownloadPage(releaseInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error checking for updates: {ex.Message}");
            }
        }

        private void loginToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            connectToSite();
        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _simData.logoutFromSite();
            Settings.Default.SessionToken = "";
            Settings.Default.Save();
        }

        private async void checkSessionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool sessionValid = await _simData.checkSession(Settings.Default.SessionToken);
            if (sessionValid)
            {
                Plugin_OnShowMsgbox(this, "Session is valid.", "Session Check", MessageBoxButtons.OK);
            }
            else
            {
                Plugin_OnShowMsgbox(this, "Session is not valid.", "Session Check", MessageBoxButtons.OK);
            }
        }

        private void screenshotToolStripMenuItem1_Click(object sender, EventArgs e)
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

                // Save the screenshot as a png file with the date and time as filename

                string fileName = $"Screenshot_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png";
                string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SimAddon");
                if (!Directory.Exists(documentsPath))
                {
                    Directory.CreateDirectory(documentsPath);
                }
                string filePath = Path.Combine(documentsPath, fileName);
                bitmap.Save(filePath, ImageFormat.Png);
                screenshots.Add(filePath);

                // Load the screenshot into the clipboard
                Clipboard.SetImage(bitmap);
                Logger.WriteLine($"Screenshot saved to {filePath}");

            }
        }

        private void GenerateReport(REPORTFORMAT format)
        {

            if (flightRecords == null)
            {
                //show a message box saying there is no flight data to save
                Plugin_OnShowMsgbox(this, "No flight data", "There is no flight data to save.", MessageBoxButtons.OK);

                Logger.WriteLine("No flight data to save.");
                return;
            }

            //create a filename from the departure and arrival airport, and the current date/time
            string filename = $"FlightRecord_{departureAirport}_{arrivalAirport}_{DateTime.UtcNow.ToString("yyyyMMdd_HHmm")}";
            switch (format)
            {
                case REPORTFORMAT.MD:
                    filename += ".md";
                    break;
                case REPORTFORMAT.HTML:
                    filename += ".html";
                    break;
                case REPORTFORMAT.JSON:
                    filename += ".json";
                    break;
                default:
                    filename += ".html";
                    break;
            }

            //the file is saved in the users documents folder
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //create a simaddon folder in the documents folder if it doesn't exist
            documentsPath = Path.Combine(documentsPath, "SimAddon");

            //open a folder chooser dialog to select the folder to save the flight record
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the folder to save the flight record";
            folderBrowserDialog.SelectedPath = documentsPath;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                documentsPath = folderBrowserDialog.SelectedPath;



                if (!Directory.Exists(documentsPath))
                {
                    Directory.CreateDirectory(documentsPath);
                }
                //create a subfolder with the departure and arrival airport and current date
                string subfolder = $"{departureAirport}_{arrivalAirport}_{DateTime.UtcNow.ToString("yyyyMMdd")}";
                documentsPath = Path.Combine(documentsPath, subfolder);
                if (!Directory.Exists(documentsPath))
                {
                    Directory.CreateDirectory(documentsPath);
                }
                filename = Path.Combine(documentsPath, filename);
                //save the flight record to the file
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    //create a string title for the report
                    string title = $"Flight Record from {departureAirport} to {arrivalAirport}";

                    //write the report header
                    sw.WriteLine(ReportBuilder.GenerateReportHeader(title, format));
                    //write each flight record
                    sw.WriteLine(ReportBuilder.GenerateReport(plugsMgr.plugins, format));


                    Logger.WriteLine("Flight record saved to " + filename);

                    //also save the list of screenshots taken during the flight
                    if (screenshots.Count > 0)
                    {
                        //move the screenshots to the same folder as the flight record
                        foreach (string screenshot in screenshots)
                        {
                            try
                            {
                                string destFile = Path.Combine(documentsPath, Path.GetFileName(screenshot));
                                File.Move(screenshot, destFile);
                                Logger.WriteLine("Screenshot moved to " + destFile);
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteLine("Error moving screenshot " + screenshot + " : " + ex.Message);
                            }
                        }
                        Plugin_OnShowMsgbox(this, $"Flight report and screenshots saved to {documentsPath}", "Flight Report Saved", MessageBoxButtons.OK);
                    }
                    else
                    {
                        Logger.WriteLine("No screenshots taken during the flight.");
                    }

                    //if there is a setting for screenshots folder, then copy the flight record there too
                    string screenshotsFolder = Properties.Settings.Default.ScreenshotsFolder;
                    if (!string.IsNullOrEmpty(screenshotsFolder))
                    {
                        try
                        {
                            //make sure the folder exist
                            if (Directory.Exists(screenshotsFolder))
                            {
                                //find all the files in the screenshots folder
                                var files = Directory.EnumerateFiles(screenshotsFolder).ToList();
                                //for each file, check if file was created during the flight
                                foreach (string file in files)
                                {
                                    try
                                    {
                                        DateTime creationTime = File.GetCreationTime(file);
                                        //if (creationTime >= flightStartTime && creationTime <= flightEndTime)
                                        if (creationTime >= flightStartTime)
                                        {
                                            //copy the file to the destination folder with retry logic
                                            string destFile = Path.Combine(documentsPath, Path.GetFileName(file));
                                            bool copySuccess = false;
                                            int maxRetries = 3;

                                            for (int retry = 0; retry < maxRetries && !copySuccess; retry++)
                                            {
                                                try
                                                {
                                                    if (retry > 0)
                                                    {
                                                        // Wait a bit before retrying
                                                        System.Threading.Thread.Sleep(500);
                                                    }

                                                    File.Copy(file, destFile, true);
                                                    copySuccess = true;
                                                    Logger.WriteLine($"Screenshot copied from Steam folder : {destFile}");
                                                }
                                                catch (IOException ioEx) when (retry < maxRetries - 1)
                                                {
                                                    Logger.WriteLine($"Retry {retry + 1}/{maxRetries} for file {file}: {ioEx.Message}");
                                                }
                                            }

                                            if (!copySuccess)
                                            {
                                                Logger.WriteLine($"Failed to copy screenshot after {maxRetries} attempts: {file}");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.WriteLine($"Error processing screenshot file {file}: {ex.Message}");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine("Error copying screenshots from Steam folder : " + ex.Message);
                        }

                    }
                    //write the report footer
                    sw.WriteLine(ReportBuilder.GenerateReportFooter(format));

                    //opne the destination folder in file explorer
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = documentsPath,
                        UseShellExecute = true, // This is necessary to open the URL in the default browser
                        Verb = "open" // This is necessary to open the folder in the file explorer
                    });
                }
            }

        }

        private void generateFlightReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GenerateReport(REPORTFORMAT.HTML);
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //show the settings form
            using (SimaddonSettingsForm settingsForm = new SimaddonSettingsForm())
            {
                //set the current settings
                settingsForm.AlwaysOnTop = Properties.Settings.Default.AlwaysOnTop;
                settingsForm.AutoHide = this.autoHide;
                settingsForm.TransparencyLevel = this.transparencyLevel;
                settingsForm.screenshotFolder = Properties.Settings.Default.ScreenshotsFolder;
                settingsForm.SiteUrl = Properties.Settings.Default.GSheetAPIUrl;

                //set the visible plugins
                Dictionary<string, bool> visiblePlugins = new Dictionary<string, bool>();
                foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
                {
                    bool isVisible = false;
                    foreach (TabPage tab in tabControl1.TabPages)
                    {
                        if (tab.Text == plugin.getName())
                        {
                            isVisible = true;
                        }
                    }
                    visiblePlugins[plugin.getName()] = isVisible;
                }
                settingsForm.VisiblePlugins = visiblePlugins;
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    //apply the new settings
                    this.TopMost = settingsForm.AlwaysOnTop;
                    this.autoHide = settingsForm.AutoHide;
                    ToggleTransparency(settingsForm.TransparencyLevel);
                    Properties.Settings.Default.AutoHide = settingsForm.AutoHide;
                    Properties.Settings.Default.AlwaysOnTop = settingsForm.AlwaysOnTop;
                    Properties.Settings.Default.TransparentWindow = settingsForm.TransparencyLevel;
                    Properties.Settings.Default.ScreenshotsFolder = settingsForm.screenshotFolder;
                    Properties.Settings.Default.Save();
                    //update the visible plugins
                    foreach (var pluginVisibility in settingsForm.VisiblePlugins)
                    {
                        if (pluginVisibility.Value)
                        {
                            //show the tab
                            showTab(pluginVisibility.Key);
                        }
                        else
                        {
                            //hide the tab
                            hideTab(pluginVisibility.Key);
                        }
                    }
                }
            }
        }

        private void tracesFolderToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void vATSIMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (vATSIMToolStripMenuItem1.Checked)
            {
                iVAOToolStripMenuItem1.Checked = false;
                _simData.useFlyingNetwork(FlyingNetwork.VATSIM);
                Settings.Default.FlyingNetwork = FlyingNetwork.VATSIM.ToString();
                Settings.Default.Save();
            }
            WriteWindowTitle();

            SimEventArg eventArg = new SimEventArg();
            eventArg.reason = SimEventArg.EventType.CHANGENETWORK;
            //push the event to all the plugins
            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    plugin.ManageSimEvent(this, eventArg);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
                }
            }
        }

        private void iVAOToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (iVAOToolStripMenuItem1.Checked)
            {
                vATSIMToolStripMenuItem1.Checked = false;
                _simData.useFlyingNetwork(FlyingNetwork.IVAO);
                Settings.Default.FlyingNetwork = FlyingNetwork.IVAO.ToString();
                Settings.Default.Save();
            }
            WriteWindowTitle();
            SimEventArg eventArg = new SimEventArg();
            eventArg.reason = SimEventArg.EventType.CHANGENETWORK;
            //push the event to all the plugins
            foreach (ISimAddonPluginCtrl plugin in plugsMgr.plugins)
            {
                try
                {
                    plugin.ManageSimEvent(this, eventArg);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(plugin.getName() + " : " + ex.Message);
                }
            }
        }

        private void openWebSiteToolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void documentationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //open the documentation web page starting the default browser
            try
            {
                string url = "https://github.com/Skall34/SimAddon/wiki";
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // This is necessary to open the URL in the default browser
                });

            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error opening documentation web site: {ex.Message}");
                Plugin_OnShowMsgbox(this, "Error", "Unable to open the documentation web site.", MessageBoxButtons.OK);
            }
        }

        private void skyboundsAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //https://skybound-chronicles-226172438126.us-west1.run.app/
            try
            {
                //url using gemini 2.5 flash lite
                //string url = "https://skybound-chronicles-2-5-flash-lite-226172438126.us-west1.run.app/";
                //url using gemini 3 pro
                string url = "https://skybound-chronicles-226172438126.us-west1.run.app/";

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // This is necessary to open the URL in the default browser
                });
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error opening Skybounds AI web site: {ex.Message}");
                Plugin_OnShowMsgbox(this, "Error", "Unable to open the Skybounds AI web site.", MessageBoxButtons.OK);
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close the application
            this.Close();
        }

        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            if (tabControl == null) return;

            Graphics g = e.Graphics;
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabBounds = tabControl.GetTabRect(e.Index);

            // Couleurs style Visual Studio
            Color tabBackColor = Color.DimGray;        // Fond sombre
            Color tabSelectedBackColor = Color.MidnightBlue; // Bleu VS
            Color tabHoverBackColor = Color.DarkGray;   // Survol
            Color tabTextColor = Color.White;
            Color tabSelectedTextColor = Color.White;

            // Déterminer si l'onglet est sélectionné
            bool isSelected = (e.Index == tabControl.SelectedIndex);

            // Déterminer si la souris survole l'onglet
            Point mousePos = tabControl.PointToClient(Cursor.Position);
            bool isHovered = tabBounds.Contains(mousePos);

            // Dessiner le fond de l'onglet
            using (SolidBrush brush = new SolidBrush(isSelected ? tabSelectedBackColor : (isHovered ? tabHoverBackColor : tabBackColor)))
            {
                g.FillRectangle(brush, tabBounds);
            }

            // Dessiner une ligne en haut de l'onglet sélectionné (accent)
            if (isSelected)
            {
                using (Pen accentPen = new Pen(Color.FromArgb(0, 122, 204), 2))
                {
                    g.DrawLine(accentPen, tabBounds.Left, tabBounds.Top, tabBounds.Right, tabBounds.Top);
                }
            }

            // Dessiner le texte centré
            using (SolidBrush textBrush = new SolidBrush(isSelected ? tabSelectedTextColor : tabTextColor))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(tabPage.Text, tabControl.Font, textBrush, tabBounds, sf);
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripButton btn)
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                    btn.Text = "1";
                    btn.Tag = "maximize";
                }
                else
                {
                    this.WindowState = FormWindowState.Maximized;
                    btn.Text = "2";
                    btn.Tag = "restore";
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutSimAddonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutForm aboutForm = new AboutForm())
            {
                aboutForm.Icon = this.Icon;
                Plugin_OnShowDialog(this, aboutForm);
            }
        }
    }
}
