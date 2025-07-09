using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlightRecPlugin
{
    public partial class LocalFlightbookForm : Form
    {
        private string FlightbookFilePath { get; set; } = string.Empty;
        public LocalFlightBook LocalFlightBook { get; set; } = new LocalFlightBook();

        public void loadFlightbook(string flightbookFilePath)
        {
            FlightbookFilePath = flightbookFilePath;
            // Logic to load the flightbook from the specified file path
            LocalFlightBook.loadFromJson(FlightbookFilePath);
            foreach (var flight in LocalFlightBook.Flights)
            {
                //add a flight entry to the UI listview1
                ListViewItem item = new ListViewItem(flight.departureTime.ToString("yyyy-MM-dd"));
                item.SubItems.Add(flight.departureICAO);
                item.SubItems.Add(flight.arrivalICAO);
                item.SubItems.Add(flight.immatriculation);
                item.SubItems.Add(flight.commentaire);
                listView1.Items.Add(item);
            }
        }

        public LocalFlightbookForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void LocalFlightbookForm_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                // Assuming the first subitem is the date, second is departure ICAO, etc.
                string date = selectedItem.SubItems[0].Text;
                string departureICAO = selectedItem.SubItems[1].Text;
                string arrivalICAO = selectedItem.SubItems[2].Text;
                string immatriculation = selectedItem.SubItems[3].Text;
                string commentaire = selectedItem.SubItems[4].Text;
                toolTip1.SetToolTip(listView1, $"Flight on {date} from {departureICAO} to {arrivalICAO} with immatriculation {immatriculation}.\nComment: {commentaire}");

                tbFlightDetails.Text = $"Flight Details:" + Environment.NewLine +
                    $"Date: {date}" + Environment.NewLine +
                    $"Departure ICAO: {departureICAO}" + " \t " + $"Arrival ICAO: {arrivalICAO}" + Environment.NewLine +
                    $"Immatriculation: {immatriculation}" + Environment.NewLine +
                    $"Commentaire: {commentaire}";
            }
            else
            {
                toolTip1.SetToolTip(listView1, "Select a flight to view details.");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            LocalFlightBook.ClearFlights();
            LocalFlightBook.saveToJson(FlightbookFilePath);
        }

        private void tbFlightDetails_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
