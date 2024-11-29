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
                    proches.Sort((x, y) => x.distance.CompareTo(y.distance));
                    if (proches.Count > 0)
                    {
                        foreach (Aeroport a in cbICAO.Items)
                        {
                            if (!proches.Contains(a))
                            {
                                cbICAO.Items.Remove(a);
                            }
                        }

                        foreach (Aeroport a in proches)
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
            bool refreshOK = await ATIS.refresh();
            tbATISText.Text = string.Empty;
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
                ATISData atis = ATIS.data.FirstOrDefault(a => a.callsign.StartsWith(searchItem));
                if (atis != null)
                {
                    foreach (string s in atis.text_atis)
                    {
                        string atisText = s.Replace("RWY ", "RUNWAY ")
                            .Replace("DEP", "DEPARTURE")
                            .Replace("ARR", "ARRIVAL")
                            .Replace("VIS ", "VISIBILITY ")
                            .Replace("TRL ", "TRANSITION LEVEL ");
                        tbATISText.Text += atisText + " ";
                    }
                }
                else
                {
                    tbATISText.Text = "No ATIS available";
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
    }
}
