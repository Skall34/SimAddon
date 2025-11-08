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

        private Dictionary<string, string> abbreviations = new Dictionary<string,string>{
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
            this.cbICAO.ValueMember = "fullName";
            this.cbICAO.DisplayMember = "fullName";
            //if (!File.Exists("dictionnary.json"))
            //{
            //    SaveDictionaryToJsonFile(abbreviations, "dictionnary.json");
            //}
            //else
            //{
            //    abbreviations = LoadDictionaryFromJsonFile("dictionnary.json");
            //}
        }

        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;
        public event ISimAddonPluginCtrl.OnShowDialogHandler OnShowDialog;

        //event ISimAddonPluginCtrl.UpdateStatusHandler ISimAddonPluginCtrl.OnStatusUpdate
        //{
        //    add
        //    {
        //        updateStatusHandler = value;
        //    }

        //    remove
        //    {
        //        updateStatusHandler = null;
        //    }
        //}

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
            VATSIM.refresh();
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
            if (OnTalk!=null)
            {
                OnTalk(this, textToSpeech);
            }
        }

        public void updateSituation(situation data)
        {
            try
            {

                //todo : rafraichis la list des aéroports assez proches pour être interrogés.
                if ((simdata != null) && (simdata.isConnectedToSim))
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
                    decoded = debut + hour + ":" + minute+ " ." + fin;
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
                decoded = debut +" AIR TEMPERATURE "+ temp + "°C, DEW POINT " + dew+"°C ." + fin;
            }

            r = new Regex(@"^(?<start>.+)(?<dir>\d{3})(?<speed>\d{2})KT(?<end>.*)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string dir = result.Groups["dir"].Value;
                string speed = result.Groups["speed"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + ". WIND : " + speed + " KNOTS AT " + dir+ " ." + fin;
            }

            r = new Regex(@"^(?<start>.+)(?<startdir>\d{3})V(?<stopdir>\d{3})(?<end>.*)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string startdir = result.Groups["startdir"].Value;
                string stopdir = result.Groups["stopdir"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + " VARIABLE BETWEEN " + startdir + " AND " + stopdir+ " ." + fin;
            }

            r = new Regex(@"^(?<start>.+)TRL (?<trl>\d{2})(?<end>.+)");
            result = r.Match(decoded);
            if (result.Success)
            {
                string debut = result.Groups["start"].Value;
                string trl = result.Groups["trl"].Value;
                string fin = result.Groups["end"].Value;
                decoded = debut + ". TRANSITION LEVEL " + trl +" ."+ Environment.NewLine + fin;
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
                string[] newElements=new string[elements.Length];

                for (int i= 0; i < elements.Length; i++)
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
                decoded = string.Join(" ",newElements);
            }
            return decoded;
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

                //ne pas chercher si aucun critere de recherche n'a été entré.
                if (searchItem.Trim() != string.Empty)
                {

                    if (VATSIM.data.atis != null)
                    {
                        List<ATISData> atis = VATSIM.FindATIS(searchItem);
                        if (atis.Count > 0)
                        {
                            foreach (ATISData a in atis)
                            {
                                string speechText = string.Empty;
                                foreach (string s in a.text_atis)
                                {
                                    string atisText = decodeATIS(s);
                                    tbATISText.Text += atisText + " ";
                                    speechText += atisText+Environment.NewLine;
                                }
                                annonce(speechText);
                                tbATISText.Text += Environment.NewLine + "-------------------" + Environment.NewLine;

                            }
                        }
                        else
                        {
                            tbATISText.Text = "No ATIS available";
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

        void ISimAddonPluginCtrl.SetExecutionFolder(string path)
        {
            throw new NotImplementedException();
        }

        public void ManageSimEvent(object sender, SimEventArg eventArg)
        {
            throw new NotImplementedException();
        }
    }
}
