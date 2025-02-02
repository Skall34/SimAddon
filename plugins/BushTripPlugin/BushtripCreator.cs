using SimDataManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BushTripPlugin
{
    public partial class BushtripCreator : Form
    {
        simData data;
        string searched;

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
                int choses = Random.Shared.Next(nbAirports);
                Aeroport a = data.aeroports[choses];
                comboBox1.Items.Clear();
                comboBox1.Items.Add(a);
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No airport database loaded");
            }
        }

        public static async Task<List<Aeroport>> BuildBushtrip(List<Aeroport> airports, Aeroport departure, Aeroport arrival, double maxHop)
        {

            List<Aeroport> result = new List<Aeroport>();
            //remove the start airport from the global list of airports.
            airports.Remove(departure);
            double globalRoute = await NavigationHelper.GetApproxNavRouteAsync(departure.latitude_deg, departure.longitude_deg, arrival.latitude_deg, arrival.longitude_deg);
            double localRoute;
            double ecart;

            if (departure != arrival)
            {
                //build the list of airports within the required range.
                departure.FindAirportsInRange(airports, maxHop);

                //in this short list, find the one closest to the destination.
                Aeroport Next = arrival.FindClosestAirport(departure.AirportsInRange);
                localRoute = await NavigationHelper.GetApproxNavRouteAsync(departure.latitude_deg, departure.longitude_deg, Next.latitude_deg, Next.longitude_deg);
                ecart = Math.Abs((localRoute - globalRoute) % 360);
                while ((ecart > 90) && (Next != null))
                {
                    departure.AirportsInRange.Remove(Next);
                    Next = arrival.FindClosestAirport(departure.AirportsInRange);
                    if (Next != null)
                    {
                        localRoute = await NavigationHelper.GetApproxNavRouteAsync(departure.latitude_deg, departure.longitude_deg, Next.latitude_deg, Next.longitude_deg);
                        ecart = Math.Abs((localRoute - globalRoute) % 360);
                    }
                }

                if (Next != null)
                {
                    //remove it from the short list
                    departure.AirportsInRange.Remove(Next);

                    //if we're arrived, then just add the last point to the result.
                    if (Next == arrival)
                    {
                        result.Add(Next);
                    }
                    else
                    {
                        while (Next != null)
                        {
                            //try to build the rest of the bushtrip starting now from this new point
                            List<Aeroport> nextHops = await BuildBushtrip(airports, Next, arrival, maxHop);
                            if (nextHops.Count > 0)
                            {
                                result.Add(departure);
                                result.AddRange(nextHops);
                                break;
                            }
                            else
                            {   //if this leads to nowhere, then remove this point, and find the next closest one.
                                departure.AirportsInRange.Remove(Next);

                                Next = arrival.FindClosestAirport(departure.AirportsInRange);
                                localRoute = await NavigationHelper.GetApproxNavRouteAsync(departure.latitude_deg, departure.longitude_deg, Next.latitude_deg, Next.longitude_deg);
                                ecart = Math.Abs((localRoute - globalRoute) % 360);
                                while ((ecart > 90) && (Next != null))
                                {
                                    departure.AirportsInRange.Remove(Next);
                                    Next = arrival.FindClosestAirport(departure.AirportsInRange);
                                    if (Next != null)
                                    {
                                        localRoute = await NavigationHelper.GetApproxNavRouteAsync(departure.latitude_deg, departure.longitude_deg, Next.latitude_deg, Next.longitude_deg);
                                        ecart = Math.Abs((localRoute - globalRoute) % 360);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            else
            {
                result.Add(arrival);
            }
            return result;
        }


        private async void btnSearchFlights_Click(object sender, EventArgs e)
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
                if ((start == null) || (end == null))
                {
                    MessageBox.Show("ICAO départ ou arrivée non renseigné !", "SimAddon", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                }
                else
                {
                    List<Aeroport> temp = new List<Aeroport>(data.aeroports);
                    List<Aeroport> trip = await BuildBushtrip(temp, start, end, miles);
                    foreach (Aeroport trip2 in trip)
                    {
                        lbArrivals.Items.Add(trip2);
                    }
                }
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
                int choses = Random.Shared.Next(nbAirports);
                Aeroport a = data.aeroports[choses];
                comboBox2.Items.Clear();
                comboBox2.Items.Add(a);
                comboBox2.SelectedIndex = 0;
            }
            else
            {
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
    }
}
