using Newtonsoft.Json;
using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ATISPlugin
{
    public partial class ATISCtrl : UserControl, ISimAddonPluginCtrl
    {
        private simData simdata;
        private genericATC ATC;
        private bool _refreshInProgress = false;

        //ecoded.Replace("RWY", "RUNWAY")
        //                            .Replace("DEP", "DEPARTURE")
        //                            .Replace("ARR", "ARRIVAL")
        //                            .Replace("VIS ", "VISIBILITY ")
        //                            .Replace("SID", "STANDARD INSTRUMENT DEPARTURE")
        //                            .Replace("RNP", "REQUIRED NAVIGATION PERFORMANCE")
        //                            .Replace("VPT", "VISUAL PATH")
        //                            .Replace("CAVOK"," CEILING AND VISIBILITY OK.")
        //                            .Replace("SCT", "SCATTERED AT")
        //                            .Replace("BKN", "BROKEN AT")
        //                            .Replace(" /",". ");

        private Dictionary<string, string> abbreviations = new Dictionary<string, string>{
            {@" \",". " },
            {"ACK",". ACKNOWLEDGE " },
            {"ADVS",". ADVISE" },
            {"ARR","ARRIVAL " },
            {"APCH","APPROACH" },
            {"BKN", "BKOKEN ATR" },
            {"BTN", "BETWEEN" },
            {"DP",", DEW POINT" },
            {"DEP","DEPARTURE " },
            {"DEG","° " },
            {"EXP","EXPECT " },
            {"ILS","I L S" },
            {"KT","KNOTS." },
            {"LDG","LANDING" },
            {"QNH",". QNH" },
            {"RNP", ". REQUIRED NAVIGATION PERFORMANCE" },
            {"RWY","RUNWAY" },
            {"SCT","SCATTERED AT" },
            {"SID", "STANDARD INSTRUMENT DEPARTURE" },
            {"TEMP",". TEMPERATURE " },
            {"VRB", "VARIABLE " },
            {"VIS", "VISIBILITY " },
            {"VPT", "VISUAL PATH" },
            {"CAVOK", ". CEILING AND VISIBILITY OK" },
            {"WND", ". WIND" }
        };

        static void SaveDictionaryToJsonFile(Dictionary<string, string> dictionary, string filePath)
        {
            // Serialize the dictionary to a JSON string
            string jsonString = JsonConvert.SerializeObject(dictionary, Formatting.Indented);

            // Write the JSON string to the specified file
            File.WriteAllText(filePath, jsonString);
        }

        static Dictionary<string, string> LoadDictionaryFromJsonFile(string filePath)
        {
            // Read the JSON string from the specified file
            string jsonString = File.ReadAllText(filePath);

            // Deserialize the JSON string back into a dictionary
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        }

        public ATISCtrl()
        {
            InitializeComponent();
            this.cbICAO.ValueMember = "name";
            this.cbICAO.DisplayMember = "name";
        }

        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;
        public event ISimAddonPluginCtrl.OnShowDialogHandler OnShowDialog;

        private void UpdateStatus(string message)
        {
            if (OnStatusUpdate != null)
            {
                OnStatusUpdate(this, message);
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
            if (simdata.flyingNetwork.Name == FlyingNetwork.VATSIM)
            {
                ATC = new VATSIMATC();
            }
            else
            {
                ATC = new IVAOATC();
            }
            panel1.BackgroundImage = ATC.GetNetworkImage();

            _ = RunWithProgress(RefreshATISData());
        }

        public TabPage registerPage()
        {
            //parent.SuspendLayout();
            TabPage pluginPage = new TabPage();
            pluginPage.Text = getName();
            pluginPage.Controls.Add(this);
            this.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Dock = DockStyle.Fill;
            pluginPage.Visible = true;
            return pluginPage;
            //parent.TabPages.Add(pluginPage);
            //parent.ResumeLayout();
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

        private void annonce(string textToSpeech)
        {
            if (OnTalk != null)
            {
                OnTalk(this, textToSpeech);
            }
        }

        public void updateSituation(situation data)
        {
        }

        private string decodeATIS(string rawATIS)
        {
            string decoded = rawATIS;

            //first, change the date
            Regex r = new Regex(@"^(?<start>.+)(?<hour>\d{2})(?<minute>\d{2})Z(?<end>.*)");

            Match result = r.Match(rawATIS);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string hour = result.Groups["hour"].Value;
                string minute = result.Groups["minute"].Value;
                string fin = result.Groups["end"].Value;

                if (minute == "00")
                {
                    minute = "0 0 .";
                    decoded = debut + hour + " " + minute + fin;
                }
                else
                {
                    decoded = debut + hour + ":" + minute + " ." + fin;
                }
            }

            r = new Regex(@"^(?<start>.+)(?<temp>\d{2})/(?<dew>\d{2})(?<end>.*)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string temp = result.Groups["temp"].Value;
                string dew = result.Groups["dew"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + " AIR TEMPERATURE " + temp + "°C, DEW POINT " + dew + "°C ." + fin;
            }

            r = new Regex(@"^(?<start>.+)(?<dir>\d{3})(?<speed>\d{2})KT(?<end>.*)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string dir = result.Groups["dir"].Value;
                string speed = result.Groups["speed"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + ". WIND : " + speed + " KNOTS AT " + dir + " ." + fin;
            }

            r = new Regex(@"^(?<start>.+)(?<startdir>\d{3})V(?<stopdir>\d{3})(?<end>.*)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string startdir = result.Groups["startdir"].Value;
                string stopdir = result.Groups["stopdir"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + " VARIABLE BETWEEN " + startdir + " AND " + stopdir + " ." + fin;
            }

            r = new Regex(@"^(?<start>.+)TRL (?<trl>\d{2})(?<end>.+)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string trl = result.Groups["trl"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + ". TRANSITION LEVEL " + trl + " ." + Environment.NewLine + fin;
            }

            r = new Regex(@"^(?<start>.+)BKN(?<level>\d{3})(?<end>.+)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string level = result.Groups["level"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + ". BROKEN AT " + level + "00 ." + Environment.NewLine + fin;
            }

            r = new Regex(@"^(?<start>.+)SCT(?<level>\d{3})(?<end>.+)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string level = result.Groups["level"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + ". SCATTERED AT " + level + "00 ." + Environment.NewLine + fin;
            }

            r = new Regex(@"^(?<start>.+)Q(?<qnh>\d{4})(?<end>.+)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string qnh = result.Groups["qnh"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + ". QNH " + qnh + " ." + Environment.NewLine + fin;
            }

            //then replace some abbreviations.
            foreach (string key in abbreviations.Keys)
            {
                string[] elements = decoded.Split(" ");
                string[] newElements = new string[elements.Length];

                for (int i = 0; i < elements.Length; i++)
                {
                    if (elements[i] != key)
                    {
                        newElements[i] = elements[i].Trim();
                    }
                    else
                    {
                        newElements[i] = abbreviations[key];
                    }
                }
                decoded = string.Join(" ", newElements);
            }
            return decoded;
        }

        // helper: runs an async bool operation and shows an animated progress until it completes
        private async Task<bool> RunWithProgress(Task<bool> operation)
        {
            if (_refreshInProgress) return false;
            _refreshInProgress = true;

            // Prepare progress bar
            try
            {
                if (!progressBar1.Visible)
                {
                    progressBar1.Style = ProgressBarStyle.Continuous;
                    progressBar1.Value = 0;
                    progressBar1.Visible = true;
                }

                using (var timer = new System.Windows.Forms.Timer())
                {
                    timer.Interval = 150; // update every 150ms
                    timer.Tick += (s, e) =>
                    {
                        try
                        {
                            if (!progressBar1.Visible) return;
                            int next = progressBar1.Value + 7;
                            if (next > progressBar1.Maximum) next = progressBar1.Minimum;
                            progressBar1.Value = next;
                        }
                        catch { }
                    };
                    timer.Start();

                    bool result = false;
                    try
                    {
                        result = await operation.ConfigureAwait(false); // attend la tâche asynchrone
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        try { timer.Stop(); timer.Dispose(); } catch { }
                        // hide/reset progress bar on UI thread
                        if (this.IsHandleCreated && this.InvokeRequired)
                        {
                            this.Invoke((Action)(() =>
                            {
                                progressBar1.Visible = false;
                                progressBar1.Style = ProgressBarStyle.Blocks;
                                progressBar1.Value = 0;
                            }));
                        }
                        else
                        {
                            try
                            {
                                progressBar1.Visible = false;
                                progressBar1.Style = ProgressBarStyle.Blocks;
                                progressBar1.Value = 0;
                            }
                            catch { }
                        }
                        _refreshInProgress = false;
                    }

                    return result;
                }
            }
            catch
            {
                _refreshInProgress = false;
                try { progressBar1.Visible = false; } catch { }
                return false;
            }
        }

        private async void requestATIS()
        {
            string url = simdata.flyingNetwork.GetGlobalATISUrl();

            // show progress during ATC.refresh
            bool refreshOK = await RunWithProgress(ATC.refresh(url));

            tbATISText.Text = string.Empty;
            tbController.Text = string.Empty;
            lvControllers.Items.Clear();
            if (refreshOK)
            {
                ATCInfo searchItem = null;
                if (cbICAO.SelectedItem != null)
                {
                    searchItem = (ATCInfo)cbICAO.SelectedItem;
                }
                else
                {
                    searchItem = new ATCInfo() { name = cbICAO.Text.ToUpper(), tag = "" };
                    cbICAO.Text = searchItem.name;
                }

                //ne pas chercher si aucun critere de recherche n'a été entré.
                if (searchItem.name.Trim() != string.Empty)
                {
                    string localUrl = simdata.flyingNetwork.GetAirportATISUrl();
                    List<string> atis = await ATC.GetATISText(searchItem, localUrl);
                    if (atis.Count > 0)
                    {
                        foreach (string s in atis)
                        {
                            string atisText = decodeATIS(s);
                            tbATISText.Text += atisText + " ";
                            tbATISText.Text += Environment.NewLine + "-------------------" + Environment.NewLine;
                        }
                    }
                    else
                    {
                        tbATISText.Text = "No ATIS available";
                    }

                    lvControllers.Items.Clear();
                    ListViewItem item = new ListViewItem(searchItem.facility);
                    item.SubItems.Add(searchItem.name);
                    item.SubItems.Add(searchItem.frequency.ToString());
                    item.Tag = searchItem;
                    lvControllers.Items.Add(item);
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
            _ = RunWithProgress(RefreshATISData());
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
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        void ISimAddonPluginCtrl.SetExecutionFolder(string path)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> RefreshATISData()
        {
            if (_refreshInProgress) return false;
            string url = simdata.flyingNetwork.GetGlobalATISUrl();

            if (url != null)
            {
                // kick the refresh and show progress until it completes
                await ATC.refresh(url);

                try
                {
                    //ne pas rafraichir si la dropdown est ouverte.
                    if (cbICAO.DroppedDown)
                    {
                        //ne pas rafraichir la liste si l'utilisateur est en train de taper quelque chose.
                        return false;
                    }
                    List<ATCInfo> possibles = ATC.FindATISList();

                    string selectedName = string.Empty;
                    if (cbICAO.SelectedItem != null)
                    {
                        selectedName = ((ATCInfo)cbICAO.SelectedItem).name;
                    }
                    else
                    {
                        selectedName = cbICAO.Text.ToUpper();
                    }
                    cbICAO.Items.Clear();
                    if (possibles.Count > 0)
                    {
                        int selectedIndex = -1;
                        for (int i = 0; i < possibles.Count; i++)
                        {
                            cbICAO.Items.Add(possibles[i]);
                            if (possibles[i].name == selectedName)
                            {
                                selectedIndex = i;
                            }
                        }
                        if (selectedIndex >= 0)
                        {
                            //cbICAO.SelectedIndex = selectedIndex;
                        }
                        else
                        {
                            cbICAO.SelectedItem = null;
                            cbICAO.Text = selectedName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }

            }
            return true;
        }

        public void ManageSimEvent(object sender, SimEventArg eventArg)
        {
            if (eventArg.reason == SimEventArg.EventType.CHANGENETWORK)
            {
                if (simdata.flyingNetwork.Name == FlyingNetwork.VATSIM)
                {
                    ATC = new VATSIMATC();
                }
                else
                {
                    ATC = new IVAOATC();
                }
                panel1.BackgroundImage = ATC.GetNetworkImage();
                cbICAO.Items.Clear();
                cbICAO.Text = string.Empty;
                tbATISText.Text = string.Empty;

                //pendant le refresh, fais avancer la progressbar
                _ = RunWithProgress(RefreshATISData());
            }
        }

        public string getFlightReport()
        {
            throw new NotImplementedException();
        }
    }
}
