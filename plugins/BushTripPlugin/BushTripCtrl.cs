using System.Windows.Forms;
using System.Xml.Serialization;
using SimDataManager;
using SimAddonPlugin;
//using System.Text.Json;
using System;
using Newtonsoft.Json;
using SimAddonLogger;
using SimAddonControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Intrinsics.Arm;
using System.Globalization;
using simbrief;
using flightplan;

namespace BushTripPlugin
{
    public partial class BushTripCtrl : UserControl, ISimAddonPluginCtrl
    {
        private simData? data;
        private uint waypointIndex;
        private int lastWaypointIndex;
        private LittleNavmap? flightPlan;

        private string filename;
        private double declinaison;

        public BushTripCtrl()
        {
            InitializeComponent();
            waypointIndex = 0;
            lastWaypointIndex = 0;
            btnReset.Enabled = false;
            btnSaveFlightPlan.Enabled = false;
        }

        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;

        //event ISimAddonPluginCtrl.UpdateStatusHandler ISimAddonPluginCtrl.OnStatusUpdate
        //{
        //    add
        //    {
        //        updateStatusHandler = value;
        //    }

        //    remove
        //    {
        //        updateStatusHandler = null;
        //    }
        //}

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
            return ("Bushtrip");
        }

        public void init(ref simData _data)
        {
            data = _data;
        }

        public void FormClosing(object sender, FormClosingEventArgs e)
        {
            //nothing particular for termination
        }

        public void registerPage(TabControl parent)
        {
            parent.SuspendLayout();
            TabPage pluginPage = new TabPage();
            pluginPage.Text = getName();
            pluginPage.Controls.Add(this);
            this.Dock = DockStyle.Fill;
            pluginPage.Visible = true;
            parent.TabPages.Add(pluginPage);
            parent.ResumeLayout();
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
                for (int i = 0; i <= waypointIndex; i++)
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
                        toShow += " , " + wp.Name + " , " + route + " , " + distance; ;
                    }
                    ListViewItem item = new ListViewItem(new string[] { wp.Ident, wp.Name, route.ToString(), distance.ToString() });
                    item.ImageKey = wp.Type;

                    lvWaypoints.Items.Add(item);
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
                //ensure that text is scrolled down
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
                btnReset.Enabled = true;
                btnSaveFlightPlan.Enabled = true;
            }
            else
            {
                Logger.WriteLine("Error when loading flight plan");
            }
        }

        private async void btnImportFlightPLan_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Any flight plan file|*.fplan;*.lnmpln;*.xml;*.fms|Bushtrip file|*.fplan|little nav map file|*.lnmpln|simbrief xml file|*.xml|fms file|*.fms";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    filename = filePath;
                    waypointIndex = 0;
                    lastWaypointIndex = 0;
                    if (filePath.EndsWith(".fplan"))
                    {
                        string json = File.ReadAllText(filePath);
                        flightPlan = JsonConvert.DeserializeObject<LittleNavmap>(json);
                        waypointIndex = flightPlan.CurrentStep;
                        Logger.WriteLine("Fichier fplan chargé avec succès !");
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
                        Logger.WriteLine("Fichier XML chargé avec succès !");
                    }

                    if (filePath.EndsWith(".xml"))
                    {
                        //set the save file name to store fileplan & current position;
                        filename = filePath.Replace(".xml", ".fplan");

                        // Remplacez 'FlightPlan' par la classe générée à partir du XSD
                        XmlSerializer serializer = new XmlSerializer(typeof(OFP));

                        // Lecture du fichier et désérialisation
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            OFP simbriefPlan = (OFP)serializer.Deserialize(fileStream);
                            flightPlan = converter.FlightplanFromOFP(simbriefPlan);
                        }
                        flightPlan.CurrentStep = waypointIndex;
                        Logger.WriteLine("Fichier XML chargé avec succès !");
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
                            Logger.WriteLine("Fichier XML chargé avec succès !");
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine("Erreur dans le fichier FMS "+filePath);
                            Logger.WriteLine(ex.ToString());
                        }
                    }


                    //everything is OK, use the flightplan
                    useFlightPlan();


                }catch(Exception ex)
                {
                    Logger.WriteLine(ex.ToString());
                    MessageBox.Show(ex.ToString(),"Error during import");
                }

            }

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void saveFlightPlan()
        {
            if (filename.EndsWith(".json"))
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

        private void btnSaveFlightPlan_Click(object sender, EventArgs e)
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

        private void BushTripCtrl_Load(object sender, EventArgs e)
        {

        }

        private void lvWaypoints_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (flightPlan != null)
            {
                if (waypointIndex == lastWaypointIndex)
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

        private void button1_Click(object sender, EventArgs e)
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

        private LittleNavmap createFlightPlan(List<Aeroport> trip)
        {
            
            LittleNavmap fp = new LittleNavmap();
            LittleNavmapFlightplan lfp = new LittleNavmapFlightplan();
            lfp.SimData = "simaddon";
            lfp.NavData = "simaddon";
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

        private void btnCreateTrip_Click(object sender, EventArgs e)
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


            BushtripCreator creator = new BushtripCreator(data);
            DialogResult result =  creator.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<Aeroport> trip = creator.Trip;
                if (trip.Count > 0)
                {
                    flightPlan = createFlightPlan(trip);
                    filename = "bushtrip_" + trip[0].ident + "-" + trip[trip.Count - 1].ident + ".fplan";
                    //saveFlightPlan();
                    useFlightPlan();
                }
                else
                {
                    MessageBox.Show("Nothing to export");
                }

            }
            
            if (isTopMost)
            {
                //reactive le always on top
                parentForm.TopMost = true;
            }
        }

        void ISimAddonPluginCtrl.SetExecutionFolder(string path)
        {
            throw new NotImplementedException();
        }
    }
}
