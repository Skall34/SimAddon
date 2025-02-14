using FSUIPC;
using Microsoft.VisualBasic.Logging;
using SimAddonLogger;
using SimDataManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BushTripPlugin
{
    public partial class BushtripCreator : Form
    {
        simData data;
        string searched;

        bool cancel = false;

        //public Aeroport Departure { get; set; }
        //public Aeroport Arrival { get; set; }

        public List<Aeroport> Trip { get; set; }

        public BushtripCreator(simData _data)
        {
            InitializeComponent();
            data = _data;
            searched = string.Empty;
            lblGndSpeed.Text = trackBar1.Value.ToString();
            Trip = new List<Aeroport>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool OK = true;
            if (checkMultiHop.Checked)
            {
                foreach (var item in lbArrivals.Items)
                {
                    Trip.Add((Aeroport)item);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                if (comboBox1.SelectedItem != null)
                {
                    Trip.Add((Aeroport)comboBox1.SelectedItem);
                }
                else
                {
                    MessageBox.Show("Please select a departure");
                    OK = false;
                }
                if (lbArrivals.SelectedItem != null)
                {
                    Trip.Add((Aeroport)lbArrivals.SelectedItem);
                }
                else
                {
                    MessageBox.Show("Please select a destination");
                    OK = false;
                }

                if (OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lblGndSpeed.Text = trackBar1.Value.ToString();
        }

        private void BushtripCreator_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRandomDeparture_Click(object sender, EventArgs e)
        {
            if (data != null)
            {
                int nbAirports = data.aeroports.Count;
                if (nbAirports > 0)
                {
                    int choses = Random.Shared.Next(nbAirports);
                    Aeroport a = data.aeroports[choses];
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add(a);
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    Logger.WriteLine("No Airport database loaded");
                    MessageBox.Show("No airport database loaded");
                }
            }
            else
            {
                Logger.WriteLine("No Airport database loaded");
                MessageBox.Show("No airport database loaded");
            }
        }

        private  double Heuristic(Aeroport a, Aeroport b)
        {
            return a.DistanceTo(b); // Distance en ligne droite
        }
        private  List<Aeroport> ReconstructPath(Dictionary<Aeroport, Aeroport> cameFrom, Aeroport current)
        {
            var path = new List<Aeroport> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

        // A* algorithm implementé via chatgpt !
        public async Task<List<Aeroport>> BuildBushtrip3(List<Aeroport> airports, Aeroport start, Aeroport goal, double maxHop, int depth = 0)
        {

            var openSet = new SortedSet<(double F, Aeroport Airport)>(Comparer<(double, Aeroport)>.Create((a, b) => a.Item1.CompareTo(b.Item1)));
            var cameFrom = new Dictionary<Aeroport, Aeroport>();
            var gScore = new Dictionary<Aeroport, double>();
            var fScore = new Dictionary<Aeroport, double>();

            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);
            openSet.Add((fScore[start], start));

                while (openSet.Count > 0)
                {
                    Aeroport current = openSet.First().Airport;
                    openSet.Remove(openSet.First());

                    if (current == goal)
                        return ReconstructPath(cameFrom, current);

                    current.FindAirportsInRange(airports, maxHop);
                    progressBar1.Value = 0;
                    progressBar1.Maximum = current.AirportsInRange.Count;

                    if (cancel)
                    {
                        break;
                    }

                    foreach (var neighbor in current.AirportsInRange)
                    {
                        double tentativeGScore = gScore[current] + current.DistanceTo(neighbor);
                        progressBar1.Value++;
                        if (progressBar1.Value >= progressBar1.Maximum)
                        {
                            progressBar1.Value = progressBar1.Minimum;
                        }

                        if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                        {
                            cameFrom[neighbor] = current;
                            gScore[neighbor] = tentativeGScore;
                            fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);

                            if (!openSet.Any(n => n.Airport == neighbor))
                                openSet.Add((fScore[neighbor], neighbor));
                        }
                    }
                }

            return null; // Aucun chemin trouvé
        }

        private async void buildMultiHopBushTrip(Aeroport start, Aeroport end, double miles)
        {
            CancellationTokenSource s_cts = new CancellationTokenSource();
            btnSearchFlights.Enabled = false;
            Cursor = Cursors.WaitCursor;
            if ((start == null) || (end == null))
            {
                MessageBox.Show("ICAO départ ou arrivée non renseigné !", "SimAddon", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
            }
            else
            {
                List<Aeroport> temp = new List<Aeroport>(data.aeroports);
                List<Aeroport> trip;
                    trip = await BuildBushtrip3(temp, start, end, miles);
                if (trip != null)
                {
                    foreach (Aeroport etape in trip)
                    {
                        lbArrivals.Items.Add(etape);
                    }
                }

            }
            Cursor = Cursors.Default;
            btnSearchFlights.Enabled = true;
        }

        private void btnSearchFlights_Click(object sender, EventArgs e)
        {
            lbArrivals.Items.Clear();
            DateTime flightTime = dateTimePicker1.Value;
            double speed = trackBar1.Value;
            double hours = flightTime.Hour + (double)flightTime.Minute / 60;
            double miles = hours * speed;
            Aeroport start = (Aeroport)comboBox1.SelectedItem;
            Aeroport end = (Aeroport)comboBox2.SelectedItem;

            if (checkMultiHop.Checked)
            {
                buildMultiHopBushTrip(start, end, miles);
            }
            else
            {

                if (start != null)
                {
                    foreach (Aeroport a in data.aeroports)
                    {
                        double distance = a.DistanceTo(start.latitude_deg, start.longitude_deg);
                        if ((distance < miles * 1.1) && (distance > miles * 0.9))
                        {
                            lbArrivals.Items.Add(a);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("ICAO départ non renseigné !", "SimAddon", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnRandomArrival_Click(object sender, EventArgs e)
        {
            if (data != null)
            {
                int nbAirports = data.aeroports.Count;
                if (nbAirports > 0)
                {
                    int choses = Random.Shared.Next(nbAirports);
                    Aeroport a = data.aeroports[choses];
                    comboBox2.Items.Clear();
                    comboBox2.Items.Add(a);
                    comboBox2.SelectedIndex = 0;
                }
                else
                {
                    Logger.WriteLine("No airport database loaded");
                    MessageBox.Show("No airport database loaded");
                }
            }
            else
            {
                Logger.WriteLine("No airport database loaded");
                MessageBox.Show("No airport database loaded");
            }

        }

        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back)
            {
                ComboBox comboBox = sender as ComboBox;
                searched = comboBox.Text.ToLower();
                if (data != null)
                {
                    // search = comboBox1.Text;
                    List<Aeroport> possible = data.aeroports.FindAll(a => (a.fullName.ToLower().Contains(searched)));
                    comboBox.Items.Clear();
                    if (possible.Count > 0)
                    {
                        comboBox.Items.AddRange(possible.ToArray());
                        comboBox.DroppedDown = true;
                    }
                    else
                    {
                    }
                    comboBox.SelectionStart = comboBox.Text.Length;
                }
                else
                {
                    MessageBox.Show("No airport database loaded");
                }
            }
        }

        private void checkMultiHop_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = checkMultiHop.Checked;
            btnRandomArrival.Enabled = checkMultiHop.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            cancel = false;
            lbArrivals.Items.Clear();
            DateTime flightTime = dateTimePicker1.Value;
            double speed = trackBar1.Value;
            double hours = flightTime.Hour + (double)flightTime.Minute / 60;
            double miles = hours * speed;
            Aeroport start = (Aeroport)comboBox1.SelectedItem;
            Aeroport end = (Aeroport)comboBox2.SelectedItem;

            if (checkMultiHop.Checked)
            {
                buildMultiHopBushTrip(start, end, miles);
            }
            else
            {
                if (start != null)
                {
                    foreach (Aeroport a in data.aeroports)
                    {
                        double distance = a.DistanceTo(start.latitude_deg, start.longitude_deg);
                        if ((distance < miles * 1.1) && (distance > miles * 0.9))
                        {
                            lbArrivals.Items.Add(a);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("ICAO départ non renseigné !", "SimAddon", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
        }
    }
}
