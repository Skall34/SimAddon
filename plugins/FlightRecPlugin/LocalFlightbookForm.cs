using SimAddonPlugin;
using SimDataManager;
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
        private simData data;
        private string FlightbookFilePath { get; set; } = string.Empty;
        public LocalFlightBook LocalFlightBook { get; set; } = new LocalFlightBook();

        private FlightRecCtrl pluginCtrl;

        public void loadFlightbook(string flightbookFilePath)
        {
            FlightbookFilePath = flightbookFilePath;
            // Logic to load the flightbook from the specified file path
            float filesize = LocalFlightBook.loadFromJson(FlightbookFilePath);
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

            lblLocalFlightbookSize.Text = $"Local Flightbook Size: {filesize:F2} MB";
        }

        public LocalFlightbookForm(FlightRecCtrl parent, simData _data)
        {
            InitializeComponent();
            data = _data;
            pluginCtrl = parent;
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
            //ask confirmation
            var result = MessageBox.Show("Are you sure you want to clear the local flightbook? This action cannot be undone.", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {
                //clear the local flightbook
                LocalFlightBook.ClearFlights();
                LocalFlightBook.saveToJson(FlightbookFilePath);
                tbFlightDetails.Text = string.Empty;
                listView1.Items.Clear();
            }
        }

        private void tbFlightDetails_TextChanged(object sender, EventArgs e)
        {

        }

        private void pushToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //send the selected flight to the server
            if (listView1.SelectedItems.Count > 0)
            {
                //get the index of the selected flight
                int selectedIndex = listView1.SelectedItems[0].Index;
                //get the flight from the local flightbook
                var flight = LocalFlightBook.Flights[selectedIndex];
                //show that flight in a SaveFlightDialog
                SaveFlightDialog saveFlightDialog = new SaveFlightDialog(pluginCtrl, data);
                saveFlightDialog.Callsign = Properties.Settings.Default.callsign;
                saveFlightDialog.DepartureICAO = flight.departureICAO;
                saveFlightDialog.ArrivalICAO = flight.arrivalICAO;
                saveFlightDialog.Immat = flight.immatriculation;
                saveFlightDialog.DepartureTime = flight.departureTime;
                saveFlightDialog.ArrivalTime = flight.arrivalTime;
                saveFlightDialog.DepartureFuel = flight.departureFuel;
                saveFlightDialog.ArrivalFuel = flight.arrivalFuel;
                saveFlightDialog.Comment = flight.commentaire;
                saveFlightDialog.Cargo = flight.payload;
                saveFlightDialog.Mission = flight.mission;
                saveFlightDialog.Note = flight.noteDuVol;
                saveFlightDialog.GPSTrace = flight.GPSData;
                saveFlightDialog.SimPlane = flight.SimPlane;

                saveFlightDialog.ShowDialog();
                if (saveFlightDialog.DialogResult == DialogResult.OK)
                {
                    // Logic to push the flight to the server
                    // This could involve calling an API or some other method to send the flight data
                    // For now, we will just show a message box
                    pluginCtrl.ShowMsgBox("Flight pushed to server successfully!", "Success", MessageBoxButtons.OK);

                    // Optionally, you can remove the flight from the local flightbook after pushing it to the server
                    LocalFlightBook.RemoveFlightAt(selectedIndex);
                    LocalFlightBook.saveToJson(FlightbookFilePath);
                    // Remove the item from the listview
                    listView1.Items.RemoveAt(selectedIndex);

                }
                else
                {
                }

            }
            else
            {
                pluginCtrl.ShowMsgBox("Please select a flight to push to the server.", "Error", MessageBoxButtons.OK);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //delete the selected flight from the local flightbook
            if (listView1.SelectedItems.Count > 0)
            {
                //get the index of the selected flight
                int selectedIndex = listView1.SelectedItems[0].Index;
                //remove the flight from the local flightbook
                LocalFlightBook.RemoveFlightAt(selectedIndex);
                //save the flightbook to the file
                LocalFlightBook.saveToJson(FlightbookFilePath);
                //remove the item from the listview
                listView1.Items.RemoveAt(selectedIndex);
            }
            else
            {
                pluginCtrl.ShowMsgBox("Please select a flight to delete.", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
