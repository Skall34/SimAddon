using System.Windows.Forms;
using System.Xml.Serialization;
using SimDataManager;
using SimAddonPlugin;
//using System.Text.Json;
using System;
using Newtonsoft.Json;
using SimAddonLogger;

namespace BushTripPlugin
{
    public partial class BushTripCtrl : UserControl, ISimAddonPluginCtrl
    {
        private simData? data;
        private uint waypointIndex;
        private LittleNavmap? flightPlan;

        private string filename;
        private double declinaison;

        public BushTripCtrl()
        {
            InitializeComponent();
            waypointIndex = 0;
            btnReset.Enabled = false;
            btnSaveFlightPlan.Enabled = false;
        }

        public string getName()
        {
            return ("Bushtrip");
        }

        public void init(ref simData _data)
        {
            data = _data;
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
            double distanceToNextWP = 0;
            double routeToWP = 0;
            double magVariation = data.magVariation;

            if (flightPlan != null)
            {
                //s'il y a encore au moins un WP avant la fin
                if (waypointIndex+1 < flightPlan.Item.Waypoints.Count())
                {
                    double wpLat = flightPlan.Item.Waypoints[waypointIndex + 1].Pos.Lat;
                    double wpLon = flightPlan.Item.Waypoints[waypointIndex + 1].Pos.Lon;

                    distanceToNextWP = NavigationHelper.GetDistance(data.position.Location.Latitude, data.position.Location.Longitude, wpLat, wpLon);

                    routeToWP = await NavigationHelper.GetNavRouteAsync(data.position.Location.Latitude, data.position.Location.Longitude, wpLat, wpLon, magVariation);

                    tsGlobalStatus.Text = "Next waypoint : Route : " + routeToWP.ToString() + "  Distance : " + distanceToNextWP.ToString();
                }
                else
                {
                    tsGlobalStatus.Text = "Flight plan finished !";
                }

                //si on est a moins de 3 miles du wp, on affiche le segment suivant.
                if (distanceToNextWP < 3)
                {
                    if (waypointIndex < flightPlan.Item.Waypoints.Count()-1)
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
                    tbComment.Text += flightPlan.Item.Waypoints[i].Comment + Environment.NewLine + "-------------------" + Environment.NewLine;
                }
            }

        }

        private double computeFlightLength()
        {
            double result = 1;
            if (flightPlan != null)
            {
                for (int i = 0; i < flightPlan.Item.Waypoints.Count(); i++)
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

        private async void btnImportFlightPLan_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "flight plan file|*.fplan|little nav map file|*.lnmpln";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                filename = filePath;
                waypointIndex = 0;

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
                //recupére la declinaison magnétique du premier point du plan de vol
                try
                {
                    declinaison = (double)await NavigationHelper.GetMagneticDeclinaison(flightPlan.Item.Waypoints[0].Pos.Lat, flightPlan.Item.Waypoints[0].Pos.Lon);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("error when getting magnetic declinaison :" + ex.Message);
                }

                refreshFlightBook();
                double distance = computeFlightLength();
                lblDistanceTotale.Text = "Total distance :" + distance.ToString() + " miles";
                tsGlobalStatus.Text = "Flight plan loaded";
                btnReset.Enabled = true;
                btnSaveFlightPlan.Enabled = true;

            }

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void saveFlightPlan()
        {
            string json = JsonConvert.SerializeObject(flightPlan, Formatting.Indented);
            File.WriteAllText(filename, json);
        }

        private void btnSaveFlightPlan_Click(object sender, EventArgs e)
        {
            if (flightPlan != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = filename;
                saveFileDialog.Filter = "flight plan|*.fplan";
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
                waypointIndex = (uint)flightPlan.Item.Waypoints.Count() - 1;
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
    }
}
