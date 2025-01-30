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

        public Aeroport Departure { get; set; }
        public Aeroport Arrival { get; set; }

        public BushtripCreator(simData _data)
        {
            InitializeComponent();
            data = _data;
            searched = string.Empty;
            lblGndSpeed.Text = trackBar1.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Departure = (Aeroport)comboBox1.SelectedItem;
            Arrival = (Aeroport)lbArrivals.SelectedItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
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

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            searched = comboBox1.Text.ToLower();
            if (data != null)
            {
                // search = comboBox1.Text;
                List<Aeroport> possible = data.aeroports.FindAll(a => (a.fullName.ToLower().Contains(searched)));
                comboBox1.Items.Clear();
                if (possible.Count > 0)
                {
                    comboBox1.Items.AddRange(possible.ToArray());
                    comboBox1.DroppedDown = true;
                }
                else
                {
                }
                comboBox1.SelectionStart = comboBox1.Text.Length;
            }
            else
            {
                MessageBox.Show("No airport database loaded");
            }
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

        private void btnSearchFlights_Click(object sender, EventArgs e)
        {
            lbArrivals.Items.Clear();
            DateTime flightTime = dateTimePicker1.Value;
            double speed = trackBar1.Value;
            double hours = flightTime.Hour + (double)flightTime.Minute / 60;
            double miles = hours * speed;
            Aeroport start = (Aeroport)comboBox1.SelectedItem;

            if (start != null) { 
                foreach (Aeroport a in data.aeroports)
                {
                    double distance = a.DistanceTo(start.latitude_deg,start.longitude_deg);
                    if ((distance<miles*1.1)&&(distance>miles*0.9))
                    {
                        lbArrivals.Items.Add(a);
                    }
                }
            } else
            {
                MessageBox.Show("ICAO départ non renseigné !", "SimAddon", MessageBoxButtons.OK,MessageBoxIcon.Warning); ;
            }
        }
    }
}
