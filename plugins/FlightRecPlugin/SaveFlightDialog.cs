using FlightRecPlugin.Properties;
using SimAddonLogger;
using SimDataManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Quic;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlightRecPlugin
{
    public partial class SaveFlightDialog : Form
    {
        simData data;

        public string Callsign { get; set; }
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

        public  string GPSTrace { get; set; }

        public SaveFlightDialog(simData _data)
        {
            InitializeComponent();
            //set default values for properties
            data = _data;
            dtDeparture.Value = DateTime.Now;
            dtArrival.Value = DateTime.Now;
            valNote.Value = 8;
            List<string> missions = new List<string>();

            missions.AddRange(data.missions.Select(mission => mission.Libelle).Where(mission => !string.IsNullOrEmpty(mission)).ToArray());
            Missions = missions;

            data.avions.Sort();
            // Parcourez la liste des avions
            List<string> immatriculations = new List<string>();
            foreach (Avion avion in data.avions)
            {
                if (null != avion.Immat)
                {
                    //immatriculations.Add(avion.Immat);
                    cbImmat.Items.Add(avion);
                    immatriculations.Add(avion.Immat);
                }
            }
            Planes = immatriculations;
        }

        private async void btnSave_Click(object sender, EventArgs e)
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
            //else if (DepartureTime >= ArrivalTime)
            //{
            //    MessageBox.Show("Departure time must be before arrival time");
            //    result = false;
            //}

            if (result)
            {
                Dictionary<string, string> values = new Dictionary<string, string>();

                // Construction du dictionnaire à partir de flightdata
                var flightData = new Dictionary<string, string>
                    {
                        { "callsign", Callsign},
                        { "immatriculation", Immat },
                        { "departure_icao", DepartureICAO },
                        { "departure_fuel", DepartureFuel.ToString("0.00", CultureInfo.InvariantCulture) },
                        { "departure_time", DepartureTime.ToShortTimeString()},
                        { "arrival_icao", ArrivalICAO },
                        { "arrival_fuel", ArrivalFuel.ToString("0.00", CultureInfo.InvariantCulture) },
                        { "arrival_time", ArrivalTime.ToShortTimeString() },
                        { "note_du_vol", Note.ToString("0.00") },
                        { "mission", Mission },
                        { "commentaire", Comment },
                        { "payload", Cargo.ToString("0.00", CultureInfo.InvariantCulture) },
                        // Add GPS data if available
                        { "tracegps", string.IsNullOrWhiteSpace(GPSTrace) ? string.Empty : GPSTrace }
                    };

                if (string.IsNullOrWhiteSpace(DepartureICAO) || DepartureICAO == "Not Yet Available")
                    throw new Exception("Aéroport de départ non détecté !");
                if (string.IsNullOrWhiteSpace(ArrivalICAO) || ArrivalICAO == "Waiting end flight ...")
                    throw new Exception("Aéroport d’arrivée non détecté !");
                if (string.IsNullOrWhiteSpace(Mission))
                    throw new Exception("Mission non sélectionnée !");
                if (DepartureFuel == 0 || ArrivalFuel == 0)
                    throw new Exception("Carburant non détecté !");
                if (DepartureTime == DateTime.UnixEpoch || ArrivalTime == DateTime.UnixEpoch)
                    throw new Exception("Heure de départ ou d’arrivée non détectée !");

                result = await data.SendFlightDataToPhpAsync(flightData);
                // Fin JFK 18062025

                if (result)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Error sending flight to server");
                this.DialogResult = DialogResult.None;
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
