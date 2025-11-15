using flightplan;
using FlightplanPlugin;
using FlightplanPlugin.Properties;
//using System.Text.Json;
using Newtonsoft.Json;
using SimAddonLogger;
using SimAddonPlugin;
using simbrief;
using SimDataManager;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;
using static SimDataManager.SiteConnection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BushTripPlugin
{
    public partial class FlightplanCtrl : UserControl, ISimAddonPluginCtrl
    {
        private simData? data;
        private uint waypointIndex;
        private int lastWaypointIndex;
        private LittleNavmap? flightPlan;
        private bool hidden = false; //hide flight plan in case of fplan file only

        private string filename;
        private double declinaison;

        private string SimBriefDirectory;
        private string SimBriefPdfFileLink;
        private string tmpPDFfile;

        public FlightplanCtrl()
        {
            InitializeComponent();
            waypointIndex = 0;
            lastWaypointIndex = 0;
            restartToolStripMenuItem.Enabled = false;
            exportToolStripMenuItem.Enabled = false;
            getFlightBriefingToolStripMenuItem.Enabled = false;
            tmpPDFfile = string.Empty;

        }

        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;
        public event ISimAddonPluginCtrl.OnShowDialogHandler OnShowDialog;

        public void SetWindowMode(ISimAddonPluginCtrl.WindowMode mode)
        {
            if (mode == ISimAddonPluginCtrl.WindowMode.COMPACT)
            {
                splitContainer1.Panel1Collapsed = true;
            }
            else
            {
                splitContainer1.Panel1Collapsed = false;
            }
        }

        private void UpdateStatus(string message)
        {
            if (OnStatusUpdate != null)
            {
                OnStatusUpdate(this, message);
            }
        }

        public string getName()
        {
            return ("Flight plan");
        }

        public void init(ref simData _data)
        {
            data = _data;
        }

        public void FormClosing(object sender, FormClosingEventArgs e)
        {
            //nothing particular for termination
        }


        public TabPage registerPage()
        {
            //parent.SuspendLayout();
            TabPage pluginPage = new TabPage();
            pluginPage.Text = getName();
            pluginPage.Controls.Add(this);
            this.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Dock = DockStyle.Fill;
            pluginPage.Visible = true;
            return pluginPage;
            //parent.TabPages.Add(pluginPage);
            //parent.ResumeLayout();
        }

        public DialogResult ShowMsgBox(string text, string caption, MessageBoxButtons buttons)
        {
            if (OnShowMsgbox != null)
            {
                return OnShowMsgbox(this, text, caption, buttons);
            }
            else
            {
                //if the caller did not managed this, consider a cancel. (could have been ignore also ?)
                return DialogResult.Cancel;
            }
        }

        public async void updateSituation(situation data)
        {
            try
            {
                double distanceToNextWP = 0;
                double routeToWP = 0;
                double magVariation = data.magVariation;

                if (flightPlan != null)
                {
                    //s'il y a encore au moins un WP avant la fin
                    uint targetWP = waypointIndex + 1;
                    if (waypointIndex < lastWaypointIndex)
                    {
                        targetWP = waypointIndex + 1;
                    }
                    else
                    {
                        targetWP = waypointIndex;
                    }
                    double wpLat = flightPlan.Item.Waypoints[targetWP].Pos.Lat;
                    double wpLon = flightPlan.Item.Waypoints[targetWP].Pos.Lon;

                    distanceToNextWP = NavigationHelper.GetDistance(data.position.Location.Latitude, data.position.Location.Longitude, wpLat, wpLon);
                    routeToWP = await NavigationHelper.GetNavRouteAsync(data.position.Location.Latitude, data.position.Location.Longitude, wpLat, wpLon, magVariation);

                    double ecartRoute = routeToWP - (data.position.HeadingDegreesTrue - magVariation);
                    if (ecartRoute < 0)
                    {
                        ecartRoute = 360 + ecartRoute;
                    }

                    if (waypointIndex < lastWaypointIndex)
                    {
                        tsGlobalStatus.Text = "Next waypoint : Route : " + routeToWP.ToString() + "  Distance : " + distanceToNextWP.ToString();
                    }
                    else
                    {
                        tsGlobalStatus.Text = "Flight plan finished !";
                    }

                    compas1.Headings[0] = (int)ecartRoute;
                    compas1.NumericValue = distanceToNextWP;


                    //si on est a moins de 2 miles du wp, OU si on est a moins de 10 miles du dernier WP.
                    //, on affiche le segment suivant.
                    if ((distanceToNextWP <= 2) || ((distanceToNextWP <= 10) && (waypointIndex == lastWaypointIndex - 1)))
                    {
                        if (waypointIndex < lastWaypointIndex)
                        {
                            waypointIndex++;
                            flightPlan.CurrentStep = waypointIndex;
                            //save the flightplan with the current status, for a potential next reload (not to restart the whole flight)
                            saveFlightPlan();
                            refreshFlightBook();
                        }
                        else
                        {
                            tsGlobalStatus.Text = "Land here !";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }
        }

        public async void refreshFlightBook()
        {
            if (flightPlan != null)
            {

                lvWaypoints.Items.Clear();
                tbComment.Text = "";
                // Vous pouvez maintenant accéder aux propriétés de l'objet flightPlan
                for (int i = 0; i < flightPlan.Item.Waypoints.Count(); i++)
                {
                    LittleNavmapFlightplanWaypoint? wp = flightPlan.Item.Waypoints[i];
                    LittleNavmapFlightplanWaypoint? nextwp = null;

                    if (i + 1 < flightPlan.Item.Waypoints.Count())
                    {
                        nextwp = flightPlan.Item.Waypoints[i + 1];
                    }
                    string toShow = wp.Ident;
                    double route = 0;
                    double distance = 0;
                    if (nextwp != null)
                    {
                        route = await NavigationHelper.GetNavRouteAsync(wp.Pos.Lat, wp.Pos.Lon, nextwp.Pos.Lat, nextwp.Pos.Lon, declinaison);
                        distance = NavigationHelper.GetDistance(wp.Pos.Lat, wp.Pos.Lon, nextwp.Pos.Lat, nextwp.Pos.Lon);
                    }
                    if (wp.Name != string.Empty)
                    {
                        toShow += " , " + wp.Name + " , " + route + " , " + distance;
                    }
                    ListViewItem item = new ListViewItem(new string[] { wp.Ident, wp.Name, route.ToString(), distance.ToString() });
                    item.ImageKey = wp.Type;

                    if (i <= waypointIndex)
                    {
                        if (i < waypointIndex)
                        {
                            //pour les waypoints passés, on surligne en bleu
                            item.BackColor = System.Drawing.Color.LightBlue;
                        }
                        else
                        {
                            //pour le waypoint courant, on surligne en vert
                            item.BackColor = System.Drawing.Color.LightGreen;
                        }

                        //on affiche le commentaire du waypoint
                        if (!string.IsNullOrEmpty(flightPlan.Item.Waypoints[i].Comment))
                        {
                            tbComment.AppendText(flightPlan.Item.Waypoints[i].Name + Environment.NewLine);
                            tbComment.AppendText(flightPlan.Item.Waypoints[i].Comment);
                            if (!flightPlan.Item.Waypoints[i].Comment.EndsWith(Environment.NewLine))
                            {
                                tbComment.AppendText(Environment.NewLine);
                            }
                            tbComment.AppendText("-------------------" + Environment.NewLine);
                        }
                        else
                        {
                            tbComment.AppendText("-------------------" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        item.BackColor = System.Drawing.Color.White;
                    }

                    if ((i <= waypointIndex) || (!hidden))
                    {
                        lvWaypoints.Items.Add(item);
                        lvWaypoints.Items[lvWaypoints.Items.Count - 1].EnsureVisible();
                    }
                }


                //ensure that text is scrolled down in the comments
                tbComment.SelectionStart = tbComment.TextLength;
                tbComment.ScrollToCaret();
            }

        }

        private double computeFlightLength()
        {
            double result = 1;
            if (flightPlan != null)
            {
                for (int i = 0; i <= lastWaypointIndex; i++)
                {
                    LittleNavmapFlightplanWaypoint? wp = flightPlan.Item.Waypoints[i];
                    LittleNavmapFlightplanWaypoint? nextwp = null;

                    if (i + 1 < flightPlan.Item.Waypoints.Count())
                    {
                        nextwp = flightPlan.Item.Waypoints[i + 1];
                    }

                    if (nextwp != null)
                    {
                        result += NavigationHelper.GetDistance(wp.Pos.Lat, wp.Pos.Lon, nextwp.Pos.Lat, nextwp.Pos.Lon);
                    }
                }
            }
            return result;
        }

        private async void useFlightPlan()
        {
            if (flightPlan != null)
            {
                //recupére la declinaison magnétique du premier point du plan de vol
                try
                {
                    declinaison = (double)await NavigationHelper.GetMagneticDeclinaison(flightPlan.Item.Waypoints[0].Pos.Lat, flightPlan.Item.Waypoints[0].Pos.Lon);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("error when getting magnetic declinaison :" + ex.Message);
                    declinaison = 0;
                }
                lastWaypointIndex = flightPlan.Item.Waypoints.Length - 1;
                refreshFlightBook();
                double distance = computeFlightLength();
                lblDistanceTotale.Text = "Total distance :" + distance.ToString() + " miles";
                tsGlobalStatus.Text = "Flight plan loaded";
                restartToolStripMenuItem.Enabled = true;
                exportToolStripMenuItem.Enabled = true;
            }
            else
            {
                Logger.WriteLine("Error when loading flight plan");
            }
        }

        private void ImportFromFile()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Any flight plan file|*.fplan;*.lnmpln;*.xml;*.fms|Bushtrip file|*.fplan|little nav map file|*.lnmpln|simbrief xml file|*.xml|fms file|*.fms";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    filename = filePath;
                    string shortFileName = Path.GetFileName(filePath);
                    Logger.WriteLine("Importing flight plan from file " + shortFileName);
                    waypointIndex = 0;
                    lastWaypointIndex = 0;
                    bool loaded = false;

                    if (filePath.EndsWith(".fplan"))
                    {
                        string json = File.ReadAllText(filePath);
                        flightPlan = JsonConvert.DeserializeObject<LittleNavmap>(json);
                        waypointIndex = flightPlan.CurrentStep;
                        if (flightPlan.Item.Header.FileName != null)
                        {
                            shortFileName = flightPlan.Item.Header.FileName;
                        }
                        Logger.WriteLine("Fichier fplan chargé avec succès !");
                        loaded = true;
                        hidden = true;
                    }

                    if (filePath.EndsWith(".lnmpln"))
                    {
                        //set the save file name to store fileplan & current position;
                        filename = filePath.Replace(".lnmpln", ".fplan");

                        // Remplacez 'FlightPlan' par la classe générée à partir du XSD
                        XmlSerializer serializer = new XmlSerializer(typeof(LittleNavmap));

                        // Lecture du fichier et désérialisation
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            flightPlan = (LittleNavmap)serializer.Deserialize(fileStream);
                        }

                        flightPlan.CurrentStep = waypointIndex;
                        Logger.WriteLine("Fichier LNMPLN chargé avec succès !");
                        loaded = true;
                        hidden = false;
                    }

                    if (filePath.EndsWith(".xml"))
                    {
                        //https://www.simbrief.com/ofp/flightplans/GMTTGMME_PDF_1760896886.pdf

                        //set the save file name to store fileplan & current position;
                        filename = filePath.Replace(".xml", ".fplan");

                        string pdfName = shortFileName.Replace("XML", "PDF").Replace("xml", "pdf");

                        // Remplacez 'FlightPlan' par la classe générée à partir du XSD
                        XmlSerializer serializer = new XmlSerializer(typeof(OFP));

                        //lit le fichier, nettoie le en utilisant simbriefXmlHelper
                        string rawXml = File.ReadAllText(filePath); 
                        string cleanXml = simbrief.SimbriefXmlHelper.Sanitize(rawXml);
                        File.WriteAllText(filePath, cleanXml);

                        // Lecture du fichier et désérialisation
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            OFP simbriefPlan = (OFP)serializer.Deserialize(fileStream);
                            flightPlan = converter.FlightplanFromOFP(simbriefPlan);
                        }
                        flightPlan.CurrentStep = waypointIndex;

                        Logger.WriteLine("Fichier XML chargé avec succès !");
                        loaded = true;
                        hidden = false;

                    }

                    if (filePath.EndsWith(".fms"))
                    {
                        //set the save file name to store fileplan & current position;
                        filename = filePath.Replace(".fms", ".fplan");

                        string fmsRawData = File.ReadAllText(filePath);
                        try
                        {
                            fms fmsData = new fms(fmsRawData);

                            flightPlan = converter.FlightplanFromFMS(fmsData);


                            flightPlan.CurrentStep = waypointIndex;
                            Logger.WriteLine("Fichier FMS chargé avec succès !");
                            loaded = true;
                            hidden = false;

                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine("Erreur dans le fichier FMS " + filePath);
                            Logger.WriteLine(ex.ToString());
                        }
                    }

                    //set the flightplan file name in the header for next reload
                    flightPlan.Item.Header.FileName = shortFileName;

                    //everything is OK, use the flightplan
                    useFlightPlan();

                    //notify the other plugin that a flightplan was loaded if it's not a fplan file
                    if (loaded && !filePath.EndsWith(".fplan"))
                    {
                        if (OnSimEvent != null)
                        {
                            SimEventArg eventArg = new SimEventArg();
                            eventArg.reason = SimEventArg.EventType.SETDESTINATION;
                            int nbWaypoints = flightPlan.Item.Waypoints.Count();
                            eventArg.value = flightPlan.Item.Waypoints[nbWaypoints - 1].Ident;
                            OnSimEvent(this, eventArg);
                        }
                    }


                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.ToString()+" : "+ex.InnerException.Message);
                    ShowMsgBox(ex.Message +" : "+ex.InnerException.Message, "Error during import", MessageBoxButtons.OK);
                }

            }

        }

        private async void btnImportFlightPLan_Click(object sender, EventArgs e)
        {
            ImportFromFile();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void saveFlightPlan()
        {
            if (filename.EndsWith(".fplan"))
            {
                string json = JsonConvert.SerializeObject(flightPlan, Formatting.Indented);
                File.WriteAllText(filename, json);
            }
            if (filename.EndsWith(".csv"))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("Name,Latitude,Longitude"); // Header

                    foreach (LittleNavmapFlightplanWaypoint waypoint in flightPlan.Item.Waypoints)
                    {
                        writer.WriteLine($"{waypoint.Name} ({waypoint.Ident})," +
                            $"{waypoint.Pos.Lat.ToString(CultureInfo.InvariantCulture)}," +
                            $"{waypoint.Pos.Lon.ToString(CultureInfo.InvariantCulture)}"
                            );
                    }
                }

            }
        }

        private void ExportFlightplan()
        {
            if (flightPlan != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = filename;
                saveFileDialog.Filter = "flight plan|*.fplan|csv|*.csv";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog.FileName;
                    saveFlightPlan();
                }
            }

        }

        private void btnSaveFlightPlan_Click(object sender, EventArgs e)
        {
            ExportFlightplan();

        }

        private void BushTripCtrl_Load(object sender, EventArgs e)
        {

        }

        private void lvWaypoints_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (flightPlan != null)
            {
                if ((waypointIndex == lastWaypointIndex) || (!hidden))
                {
                    waypointIndex = (uint)lvWaypoints.SelectedIndices[0];
                }
                else
                {
                    waypointIndex = (uint)lastWaypointIndex;
                }
                refreshFlightBook();
            }
        }

        private void lblDistanceTotale_Click(object sender, EventArgs e)
        {

        }

        private void RestartFlightPlan()
        {
            if (flightPlan != null)
            {
                //reset waypoint index;
                waypointIndex = 0;
                flightPlan.CurrentStep = waypointIndex;
                //save the current status in the flight plan
                saveFlightPlan();
                refreshFlightBook();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            RestartFlightPlan();
        }

        private LittleNavmap createFlightPlan(List<Aeroport> trip)
        {

            LittleNavmap fp = new LittleNavmap();
            LittleNavmapFlightplan lfp = new LittleNavmapFlightplan();
            lfp.SimData = "simaddon";
            lfp.NavData = "simaddon";
            lfp.Header = new LittleNavmapFlightplanHeader();
            lfp.Header.CreationDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            lfp.Header.FileVersion = "1.0";
            lfp.Header.ProgramName = "simaddon";

            lfp.Waypoints = new LittleNavmapFlightplanWaypoint[trip.Count];
            for (int i = 0; i < trip.Count; i++)
            {
                lfp.Waypoints[i] = new LittleNavmapFlightplanWaypoint();
                lfp.Waypoints[i].Name = trip[i].name;
                lfp.Waypoints[i].Ident = trip[i].ident;
                lfp.Waypoints[i].Type = "AIRPORT";
                lfp.Waypoints[i].Pos = new LittleNavmapFlightplanWaypointPos();
                lfp.Waypoints[i].Pos.Lon = trip[i].longitude_deg;
                lfp.Waypoints[i].Pos.LonSpecified = true;
                lfp.Waypoints[i].Pos.Lat = trip[i].latitude_deg;
                lfp.Waypoints[i].Pos.LatSpecified = true;
                lfp.Waypoints[i].Pos.AltSpecified = false;
            }

            fp.Item = lfp;
            fp.CurrentStep = 0;
            waypointIndex = 0;
            return fp;
        }

        private void CreateTrip()
        {
            bool isTopMost = false;
            Form parentForm = (Form)this.TopLevelControl;
            //en cas de "always on top"
            if (parentForm.TopMost)
            {
                //temporairement desactive la always on top
                isTopMost = true;
                parentForm.TopMost = false;
            }


            BushtripCreator creator = new BushtripCreator(this, data);
            DialogResult result = creator.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<Aeroport> trip = creator.Trip;
                if (trip.Count > 0)
                {
                    flightPlan = createFlightPlan(trip);
                    filename = "bushtrip_" + trip[0].ident + "-" + trip[trip.Count - 1].ident + ".fplan";
                    //saveFlightPlan();
                    useFlightPlan();

                    //notify the other plugin that a flightplan was loaded if it's not a fplan file
                    if (OnSimEvent != null)
                    {
                        SimEventArg eventArg = new SimEventArg();
                        eventArg.reason = SimEventArg.EventType.SETDESTINATION;
                        int nbWaypoints = flightPlan.Item.Waypoints.Count();
                        eventArg.value = flightPlan.Item.Waypoints[nbWaypoints - 1].Ident;
                        OnSimEvent(this, eventArg);
                    }
                }
                else
                {
                    ShowMsgBox("No airport in the trip, aborting", "Error", MessageBoxButtons.OK);
                }

            }

            if (isTopMost)
            {
                //reactive le always on top
                parentForm.TopMost = true;
            }

        }

        private void btnCreateTrip_Click(object sender, EventArgs e)
        {
            //CreateTrip();
            contextMenuStrip1.Show(btnCreateTrip, 0, btnCreateTrip.Height);
        }

        void ISimAddonPluginCtrl.SetExecutionFolder(string path)
        {
            throw new NotImplementedException();
        }

        public void ManageSimEvent(object sender, SimEventArg eventArg)
        {

        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTrip();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportFlightplan();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestartFlightPlan();
        }

        private bool ImportFromSimbrief()
        {
            bool finalResult = false;
            if (Settings.Default.SimbriefUsername == string.Empty)
            {
                if (OnShowDialog != null)
                {
                    //show a messagebox to ask for simbrief username
                    SettingsForm settingsForm = new SettingsForm();
                    settingsForm.SimbriefUserName = Settings.Default.SimbriefUsername;
                    settingsForm.pdfStorageFolder = Settings.Default.PdfFolder;
                    DialogResult result = OnShowDialog(this, settingsForm);
                    if (result == DialogResult.OK)
                    {
                        Settings.Default.SimbriefUsername = settingsForm.SimbriefUserName;
                        Settings.Default.PdfFolder = settingsForm.pdfStorageFolder;
                        Settings.Default.Save();
                    }
                    else
                    {
                        return finalResult;
                    }
                }
                else
                {
                    //no way to ask for simbrief username, abort
                    return finalResult;
                }
            }
            //here we have a simbrief username, we can proceed
            //https://www.simbrief.com/api/xml.fetcher.php?username={username}

            //download the xml file from simbrief
            //make a web request to https://www.simbrief.com/api/xml.fetcher.php?username={username}
            string url = "https://www.simbrief.com/api/xml.fetcher.php?username=" + Settings.Default.SimbriefUsername;
            using (var client = new System.Net.WebClient())
            {
                try
                {
                    string xmlContent = client.DownloadString(url);
                    //save the xml content to a temporary file
                    string tempFile = Path.GetTempFileName() + ".xml";
                    File.WriteAllText(tempFile, xmlContent);
                    //import the flightplan from the temporary file
                    filename = tempFile.Replace(".xml", ".fplan");
                    waypointIndex = 0;
                    lastWaypointIndex = 0;
                    XmlSerializer serializer = new XmlSerializer(typeof(OFP));
                    // Lecture du fichier et désérialisation
                    using (FileStream fileStream = new FileStream(tempFile, FileMode.Open))
                    {
                        OFP simbriefPlan = (OFP)serializer.Deserialize(fileStream);
                        //save the simbrief files info
                        SimBriefDirectory = simbriefPlan.files.directory;
                        SimBriefPdfFileLink = simbriefPlan.files.pdf.link;

                        flightPlan = converter.FlightplanFromOFP(simbriefPlan);
                    }
                    flightPlan.CurrentStep = waypointIndex;
                    Logger.WriteLine("Fichier XML Simbrief chargé avec succès !");
                    useFlightPlan();
                    //notify the other plugin that a flightplan was loaded
                    if (OnSimEvent != null)
                    {
                        SimEventArg eventArg = new SimEventArg();
                        eventArg.reason = SimEventArg.EventType.SETDESTINATION;
                        int nbWaypoints = flightPlan.Item.Waypoints.Count();
                        eventArg.value = flightPlan.Item.Waypoints[nbWaypoints - 1].Ident;
                        OnSimEvent(this, eventArg);
                    }
                    getFlightBriefingToolStripMenuItem.Enabled = true;
                    finalResult = true;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Error when downloading flightplan from Simbrief : " + ex.Message);
                    ShowMsgBox("Error when downloading flightplan from Simbrief : " + ex.Message, "Error", MessageBoxButtons.OK);
                }
            }
            return finalResult;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show a messagebox to ask for simbrief username
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.SimbriefUserName = Settings.Default.SimbriefUsername;
            settingsForm.pdfStorageFolder = Settings.Default.PdfFolder;

            if (OnShowDialog != null)
            {
                DialogResult result = OnShowDialog(this, settingsForm);
                if (result == DialogResult.OK)
                {
                    Settings.Default.SimbriefUsername = settingsForm.SimbriefUserName;
                    Settings.Default.PdfFolder = settingsForm.pdfStorageFolder;
                    Settings.Default.Save();
                }
                return;
            }
        }

        private void getLastFlightplanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool importOK = ImportFromSimbrief();
            if (importOK)
            {
                getFlightBriefingToolStripMenuItem.Enabled = true;
            }
            else
            {
                //disable the get briefing menu
                getFlightBriefingToolStripMenuItem.Enabled = false;
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportFromFile();
        }

        private void getFlightBriefingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //download the pdf file from simbrief if necessary
            if (tmpPDFfile != string.Empty)
            {
                //open the pdf file
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = tmpPDFfile,
                    UseShellExecute = true
                });
            }
            else
            {
                if (SimBriefPdfFileLink != string.Empty)
                {
                    using (var client = new System.Net.WebClient())
                    {
                        try
                        {
                            string pdfFileName = Path.Combine(SimBriefDirectory, Path.GetFileName(SimBriefPdfFileLink));
                            //download the pdf file to a temporary location
                            //save it in the temp folder
                            string saveFolder = Settings.Default.PdfFolder;
                            if (saveFolder == string.Empty)
                            {
                                saveFolder = Path.GetTempPath();
                            }

                            tmpPDFfile = Path.Combine(saveFolder, SimBriefPdfFileLink);
                            client.DownloadFile(pdfFileName, tmpPDFfile);
                            //open the pdf file
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = tmpPDFfile,
                                UseShellExecute = true
                            });
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine("Error when downloading flight briefing from Simbrief : " + ex.Message);
                            ShowMsgBox("Error when downloading flight briefing from Simbrief : " + ex.Message, "Error", MessageBoxButtons.OK);
                        }
                    }
                }
            }
        }

        private void createNewFlightplanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string simbriefUrl = "https://dispatch.simbrief.com/options/new";
            // Open the default browser to the login URL
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = simbriefUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Simbrief: could not open browser for new flightplan: " + ex.Message);
            }
        }
    }
}
