using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Windows.Forms;

namespace ATISPlugin
{
    public partial class ATISCtrl : UserControl, ISimAddonPluginCtrl
    {
        private simData simdata;




        public ATISCtrl()
        {
            InitializeComponent();
            this.Enabled = false;
            this.cbICAO.ValueMember = "fullName";
            this.cbICAO.DisplayMember = "fullName";
        }

        ISimAddonPluginCtrl.UpdateStatusHandler updateStatusHandler;
        event ISimAddonPluginCtrl.UpdateStatusHandler ISimAddonPluginCtrl.OnStatusUpdate
        {
            add
            {
                updateStatusHandler = value;
            }

            remove
            {
                updateStatusHandler = null;
            }
        }

        private void UpdateStatus(string message)
        {
            if (updateStatusHandler != null)
            {
                updateStatusHandler(this, message);
            }
        }
        public void FormClosing(object sender, FormClosingEventArgs e)
        {
            //nothing particular for termination
        }

        public string getName()
        {
            return "ATIS";
        }

        public void init(ref simData _data)
        {
            //nothing particular for init
            simdata = _data;
            //ask a first refresh of vatsim data
            VATSIM.refresh();

            //for debug purpose without simulator
            if (!_data.isConnected)
            {
                this.Enabled = true;
            }
        }

        public void registerPage(TabControl parent)
        {
            parent.SuspendLayout();
            TabPage pluginPage = new TabPage();
            pluginPage.Text = getName();
            pluginPage.Controls.Add(this);
            this.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Dock = DockStyle.Fill;
            pluginPage.Visible = true;
            parent.TabPages.Add(pluginPage);
            parent.ResumeLayout();
        }

        public void SetWindowMode(ISimAddonPluginCtrl.WindowMode mode)
        {
            if (mode == ISimAddonPluginCtrl.WindowMode.COMPACT)
            {
                splitContainer1.Panel2Collapsed = true;
            }
            else
            {
                splitContainer1.Panel2Collapsed = false;
            }
        }

        public void updateSituation(situation data)
        {
            try
            {
                if (data.MasterBatteryOn && data.MasterAvionicsOn && !this.Enabled)
                {
                    this.Enabled = true;
                    ledBulb1.On = true;
                }

                if (((!data.MasterAvionicsOn) || (!data.MasterBatteryOn)) && this.Enabled)
                {
                    this.Enabled = false;
                    ledBulb1.On = false;
                }

                //todo : rafraichis la list des aéroports assez proches pour être interrogés.
                if ((simdata != null) && (simdata.isConnected))
                {
                    uint VHFRange = NavigationHelper.GetVHFRangeNauticalMiles(data.position.Altitude);
                    List<Aeroport> proches = Aeroport.FindAirportsInRange(simdata.aeroports, data.position.Location.Latitude, data.position.Location.Longitude, VHFRange);
                    List<Aeroport> possibles = new List<Aeroport>();
                    foreach (Aeroport a in proches)
                    {
                        List<ControllerData> controllers = VATSIM.FindControllers(a.ident);
                        if (controllers.Count > 0)
                        {
                            possibles.Add(a);
                        }
                    }

                    if (possibles.Count > 0)
                    {
                        possibles.Sort((x, y) => x.distance.CompareTo(y.distance));
                        foreach (Aeroport a in cbICAO.Items)
                        {
                            if (!possibles.Contains(a))
                            {
                                try
                                {
                                    cbICAO.Items.Remove(a);
                                }catch(Exception ex)
                                {

                                }
                            }
                        }

                        foreach (Aeroport a in possibles)
                        {
                            if (!cbICAO.Items.Contains(a))
                            {
                                cbICAO.Items.Add(a);
                            }
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }
        }

        private async void requestATIS()
        {

            bool refreshOK = await VATSIM.refresh();
            tbATISText.Text = string.Empty;
            tbController.Text = string.Empty;
            lvControllers.Items.Clear();

            if (refreshOK)
            {
                string searchItem = "";
                if (cbICAO.SelectedItem != null)
                {
                    searchItem = ((Aeroport)cbICAO.SelectedItem).ident;
                }
                else
                {
                    searchItem = cbICAO.Text.ToUpper();
                    cbICAO.Text = searchItem;
                }

                if (VATSIM.data.atis != null)
                {
                    List<ATISData> atis = VATSIM.FindATIS(searchItem);

                    foreach (ATISData a in atis)
                    {
                        foreach (string s in a.text_atis)
                        {
                            string atisText = s.Replace("RWY ", "RUNWAY ")
                                .Replace("DEP", "DEPARTURE")
                                .Replace("ARR", "ARRIVAL")
                                .Replace("VIS ", "VISIBILITY ")
                                .Replace("TRL ", "TRANSITION LEVEL ");
                            tbATISText.Text += atisText + " ";
                        }
                        tbATISText.Text += Environment.NewLine + "-------------------" + Environment.NewLine;

                    }
                }
                else
                {
                    tbATISText.Text = "No ATIS available";
                }

                if (VATSIM.data.controllers != null)
                {
                    List<ControllerData> controlers = VATSIM.FindControllers(searchItem);
                    foreach (ControllerData controller in controlers)
                    {
                        string rating = VATSIM.GetRatingLabel(controller);
                        string facility = VATSIM.GetFacilityLabel(controller);
                        string frequency = controller.frequency;
                        string callsign = controller.callsign;
                        ListViewItem newItem = new ListViewItem(new string[] { facility, rating, callsign, frequency });
                        newItem.Tag = controller;
                        lvControllers.Items.Add(newItem);
                    }
                }


            }
            else
            {
                UpdateStatus("Failed to get ATIS information");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            requestATIS();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbICAO_SelectedIndexChanged(object sender, EventArgs e)
        {
            requestATIS();
        }

        private void UpdateVATSIMTimer_Tick(object sender, EventArgs e)
        {
            //refresh VATSIM data at least every 5 minutes.
            VATSIM.refresh();
        }

        private void cbICAO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                requestATIS();
            }
        }

        private void lvControllers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvControllers.SelectedItems.Count > 0)
            {
                ControllerData c = (ControllerData)lvControllers.SelectedItems[0].Tag;
                if (c != null)
                {
                    string text = string.Empty;
                    if (c.text_atis != null)
                    {
                        foreach (string s in c.text_atis)
                        {
                            text += s;
                        }
                    }
                    tbController.Text = text;
                }
            }
            else
            {
                tbController.Text = string.Empty;
            }
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
