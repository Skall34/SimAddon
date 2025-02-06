using SimAddonLogger;
using SimDataManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlightRecPlugin
{
    public partial class SaveFlightDialog : Form
    {
        simData data;
        public string Immat
        {
            get
            {
                return cbImmat.Text;
            }
            set
            {
                cbImmat.Text = value;
            }
        }
        public double Cargo
        {
            get
            {
                return (double)valCargo.Value;
            }
            set
            {
                valCargo.Value = (decimal)value;
            }
        }
        public string DepartureICAO
        {
            get
            {
                return tbDepICAO.Text;
            }
            set
            {
                tbDepICAO.Text = value;
            }
        }
        public double DepartureFuel
        {
            get
            {
                return (double)valDepFuel.Value;
            }
            set
            {
                valDepFuel.Value = (decimal)value;
            }
        }
        public DateTime DepartureTime
        {
            get
            {
                return dtDeparture.Value;
            }
            set
            {
                dtDeparture.Value = value;
            }
        }
        public string ArrivalICAO
        {
            get
            {
                return tbArrICAO.Text;
            }
            set
            {
                tbArrICAO.Text = value;
            }
        }
        public DateTime ArrivalTime
        {
            get
            {
                return dtArrival.Value;
            }
            set
            {
                dtArrival.Value = value;
            }
        }
        public double ArrivalFuel
        {
            get
            {
                return (double)valArrFuel.Value;
            }
            set
            {
                valArrFuel.Value = (decimal)value;
            }
        }
        public string Comment
        {
            get
            {
                return tbComments.Text;
            }
            set
            {
                tbComments.Text = value;
            }
        }
        public short Note
        {
            get
            {
                return (short)valNote.Value;
            }
            set
            {
                valNote.Value = value;
            }
        }

        public string Mission
        {
            get
            {
                return cbMission.Text;
            }
            set
            {
                cbMission.Text = value;
            }
        }

        public List<string> Missions
        {
            set
            {
                cbMission.Items.Clear();
                cbMission.Items.AddRange(value.ToArray());
            }
        }

        public List<string> Planes
        {
            set
            {
                cbImmat.Items.Clear();
                cbImmat.Items.AddRange(value.ToArray());
            }
        }


        public SaveFlightDialog(simData _data)
        {
            InitializeComponent();
            //set default values for properties
            data = _data;
            dtDeparture.Value = DateTime.Now;
            dtArrival.Value = DateTime.Now;
            valNote.Value = 8;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool result = true;

            //check the departure ICAO
            if (data.aeroports != null)
            {
                Aeroport? start = data.aeroports.Find(a => a.ident.ToLower() == DepartureICAO.ToLower());
                if (start == null)
                {
                    MessageBox.Show("Can't find start ICAO in database");
                    result = false;
                }
            }
            else
            {
                MessageBox.Show("Can't find start ICAO in database");
                Logger.WriteLine("can't check departure, aeroport DB is empty");
                result = false;
            }

            if (data.aeroports != null)
            {
                //check the departure ICAO
                Aeroport? end = data.aeroports.Find(a => a.ident.ToLower() == ArrivalICAO.ToLower());
                if (end == null)
                {
                    MessageBox.Show("Can't find end ICAO in database");
                    result = false;
                }
            }
            else
            {
                MessageBox.Show("Can't find start ICAO in database");
                Logger.WriteLine("can't check departure, aeroport DB is empty");
                result = false;
            }


            if (Mission == string.Empty)
            {
                MessageBox.Show("Please select a mission");
                result = false;
            }
            else if (Immat == string.Empty)
            {
                MessageBox.Show("Please select a plane");
                result = false;
            }
            else if (ArrivalFuel >= DepartureFuel)
            {
                MessageBox.Show("Departure fuel can't be lower than arrival fuel");
                result = false;
            }
            else if ((DepartureICAO == string.Empty) || (ArrivalICAO == string.Empty))
            {
                MessageBox.Show("Please check departure and arrival ICAOs");
                result = false;
            }
            else if (DepartureTime >= ArrivalTime)
            {
                MessageBox.Show("Departure time must be before arrival time");
                result = false;
            }

            if (result)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void valArrFuel_ValueChanged(object sender, EventArgs e)
        {          
            if (valArrFuel.Value > valDepFuel.Value)
            {
                valDepFuel.Value = valArrFuel.Value;
            }
        }

        private void valDepFuel_ValueChanged(object sender, EventArgs e)
        {
           
            if (valArrFuel.Value > valDepFuel.Value)
            {
                valArrFuel.Value =valDepFuel.Value ;
            }
        }
    }
}
