using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static MeteoPlugin.Meteo;
using static SimAddonPlugin.ISimAddonPluginCtrl;

namespace MeteoPlugin
{
    public partial class MeteoCtrl : UserControl, ISimAddonPluginCtrl
    {
        private Font customFont;
        PrivateFontCollection fontCollection;
        simData simdata;
        METARData metarData;

        string departureICAO;
        string arrivalICAO;
        string departureRawMETAR;
        string arrivalRawMETAR;

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

        private void annonce(string textToSpeech)
        {
            if (OnTalk != null)
            {
                OnTalk(this, textToSpeech);
            }
        }

        public MeteoCtrl()
        {
            InitializeComponent();

            if (fontCollection != null)
            {
                lblDecodedMETAR.Font = new Font(fontCollection.Families[0], lblDecodedMETAR.Font.Size);
                tbMETAR.Font = new Font(fontCollection.Families[0], tbMETAR.Font.Size);
                lbAirportInfo.Font = new Font(fontCollection.Families[0], lbAirportInfo.Font.Size);
            }
            tbMETAR.Text = "Request fo METAR informations...";
            this.cbICAO.ValueMember = "fullName";
            this.cbICAO.DisplayMember = "fullName";
        }

        public void SetWindowMode(ISimAddonPluginCtrl.WindowMode mode)
        {
            if (mode == ISimAddonPluginCtrl.WindowMode.COMPACT)
            {
                splitContainer1.Panel1Collapsed = true;
            }
            else
            {
                splitContainer1.Panel1Collapsed = false;
            }
        }

        public string getName()
        {
            return "METAR";
        }

        public void init(ref simData _data)
        {
            simdata = _data;
            lbAirportInfo.Items.Clear();
            //Logger.register();
        }

        public void FormClosing(object sender, FormClosingEventArgs e)
        {
            //nothing particular for termination
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


        public void updateSituation(situation data)
        {
            try
            {
                //todo : rafraichis la list des aéroports assez proches pour être interrogés.
                if ((simdata != null) && (simdata.isConnectedToSim))
                {

                    uint VHFRange = NavigationHelper.GetVHFRangeNauticalMiles(data.position.Altitude);
                    List<Aeroport> proches = Aeroport.FindAirportsInRange(simdata.aeroports, data.position.Location.Latitude, data.position.Location.Longitude, VHFRange);
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

        private async Task<string> getRawMetarText(string searchItem)
        {
            UpdateStatus("Requesting METAR informations");
            Logger.WriteLine("Requesting METAR informations");

            string url = simdata.flyingNetwork.GetMETARUrl(searchItem);
            Logger.WriteLine("METAR request URL : " + url);
            string serverData = await Meteo.getMetar(url);
            string rawMetarText = simdata.flyingNetwork.GetRawMETARText(serverData);
            Logger.WriteLine("Raw METAR text : " + rawMetarText);
            return rawMetarText;
        }

        private void displayAirportInfo(string searchItem)
        {
            lbAirportInfo.Items.Clear();
            //show airport information in panel
            if ((simdata != null) && (simdata.aeroports != null))
            {
                Aeroport airport = simdata.aeroports.FirstOrDefault(a => a.ident == searchItem);
                if (airport != null)
                {
                    //update the name of the airport in the metar data if there any
                    if ((metarData != null) && (metarData.icao != null))
                    {
                        metarData.icao.name = airport.name;
                    }
                    string[] runways = airport.Piste.Split('/');
                    lbAirportInfo.Items.Add(airport.name);
                    lbAirportInfo.Items.Add($"Runways : {airport.Piste.Replace('/', ' ')}");
                    lbAirportInfo.Items.Add($"Type : {airport.Type_de_piste.Replace('/', ' ')}");
                    lbAirportInfo.Items.Add($"Length (ft) :{airport.Longueur_de_piste.Replace('/', ' ')}");
                    if ((airport.Piste != "?-?") && (airport.Piste != string.Empty))
                    {
                        compas1.LabelText = runways[0];
                        string[] pistes = airport.Piste.Split('-');
                        if (pistes.Length > 0)
                        {
                            //petite expression reguliere pour nettoyer l'ax de piste (enlever le L, ou R s'il y en a)
                            Regex r = new Regex("^([0-9]+)([A-Z]?)");
                            Match result = r.Match(runways[0]);
                            if (result.Success)
                            {
                                int axePiste1 = 10 * int.Parse(result.Groups[1].Value);
                                compas1.Headings[0] = axePiste1;
                                //if we have some meteo data, show it !
                                if ((metarData != null) && (metarData.Wind != null))
                                {
                                    if (metarData.Wind.Direction == "VRB")
                                    {
                                        VariableWindTimer.Start();
                                        VariableWindAnimation.Start();
                                    }
                                    else
                                    {
                                        if (metarData.WindVariation != null)
                                        {
                                            VariableWindTimer.Start();
                                            VariableWindAnimation.Start();
                                        }
                                        else
                                        {
                                            VariableWindTimer.Stop();
                                            VariableWindAnimation.Stop();
                                        }
                                        displayedWindDirection = int.Parse(metarData.Wind.Direction);
                                        compas1.Headings[1] = displayedWindDirection;
                                    }
                                    compas1.NumericValue = int.Parse(metarData.Wind.Speed);
                                    compas1.Unit = "Knts";
                                }
                                else
                                {
                                    VariableWindTimer.Start();
                                    VariableWindAnimation.Start();
                                }
                            }
                            else
                            {

                            }

                        }
                    }
                    else
                    {
                        compas1.Headings[0] = 0;
                        compas1.Headings[1] = 0;
                        compas1.NumericValue = double.NaN;
                        compas1.Unit = "???";
                    }
                }
                compas1.Invalidate();

            }
            else
            {
                lbAirportInfo.Items.Add("Still loading airports database");
            }
        }

        private void decodeAndDisplayMetar(string rawMetarText)
        {
            lblDecodedMETAR.Text = string.Empty;
            tbMETAR.Text = rawMetarText;
            try
            {
                //decode into a logical structure
                try
                {
                    //show metar information in panel
                    metarData = Meteo.decodeMetar(rawMetarText);
                    if (metarData != null)
                    {
                        //build the decoded string humanly understandable
                        lblDecodedMETAR.Text = metarData.toString();
                    }
                    else
                    {
                        lblDecodedMETAR.Text = "No data available";
                    }

                    //envoyer le text au speaker
                    if (metarData != null)
                    {
                        annonce(metarData.toSpeech());
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Error while decoding metar data " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }
        }

        private async void requestForMetar()
        {
            //create a request to https://aviationweather.gov/cgi-bin/data/metar.php?ids=LFMT
            //https://vfrmap.com/?type_aeroport=osm&lat=62.321&lon=-150.093&zoom=12&api_key=763xxE1MJHyhr48DlAP2qQ


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
            //clear airport infos.
            string rawMetarText = await getRawMetarText(searchItem);
            decodeAndDisplayMetar(rawMetarText);
            displayAirportInfo(searchItem);

            if (OnStyleChanged != null)
            {
            }
            if (metarData != null)
            {
                UpdateStatus("Done METAR informations decoding");
            }
            else
            {
                UpdateStatus("No METAR available");
            }

        }

        private async void btnRequest_Click(object sender, EventArgs e)
        {
            requestForMetar();
        }

        private void cbICAO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                requestForMetar();
            }
        }

        private void cbICAO_SelectedIndexChanged(object sender, EventArgs e)
        {
            requestForMetar();
        }

        private void lbAirportInfo_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                ListBox lb = (ListBox)sender;
                e.DrawBackground();
                Font myFont;
                Brush myBrush;
                int i = e.Index;
                if (i >= 0)
                {
                    myFont = e.Font;
                    myBrush = Brushes.Black;
                    e.Graphics.DrawString(lb.Items[i].ToString(), myFont, myBrush, e.Bounds, StringFormat.GenericDefault);
                }
            }
            catch (Exception)
            {

            }

        }

        private int displayedWindDirection;
        private void VariableWindTimer_Tick(object sender, EventArgs e)
        {
            int variableMin = 0;
            int variableMax = 360;
            if (metarData != null)
            {
                if (metarData.WindVariation != null)
                {
                    try
                    {
                        variableMin = int.Parse(metarData.WindVariation.StartAngle);
                        variableMax = int.Parse(metarData.WindVariation.EndAngle);
                        if (variableMax < variableMin)
                        {
                            variableMin -= 360;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("Error decoding wind variation angle " + ex.Message);
                    }
                }
            }
            Random random = new Random();
            displayedWindDirection = random.Next(variableMin, variableMax);
        }

        private void VariableWindAnimation_Tick(object sender, EventArgs e)
        {
            int delta1 = ((displayedWindDirection - compas1.Headings[1]));
            int delta2 = ((compas1.Headings[1] + 360 - displayedWindDirection));
            int bestDelta = 0;
            if (Math.Abs(delta1) < Math.Abs(delta2))
            {
                bestDelta = delta1;
            }
            else
            {
                bestDelta = delta2;
            }
            compas1.Headings[1] = (compas1.Headings[1] + bestDelta / 20) % 360;
            compas1.Invalidate();
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        void ISimAddonPluginCtrl.SetExecutionFolder(string path)
        {
            throw new NotImplementedException();
        }

        public async void ManageSimEvent(object sender, SimEventArg eventArg)
        {
            if (eventArg.reason == SimEventArg.EventType.SETDEPARTURE)
            {
                string icao = eventArg.value;
                if (Regex.IsMatch(icao, @"^[A-Z0-9]{4}$"))
                {
                    //check if the airport is in the list
                    Aeroport a = simdata.aeroports.FirstOrDefault(x => x.ident == icao);
                    if (a != null)
                    {
                        departureICAO = icao;
                        cbICAO.Text = icao;
                        string rawMetarText = await getRawMetarText(cbICAO.Text);
                        decodeAndDisplayMetar(rawMetarText);
                        displayAirportInfo(cbICAO.Text);
                        departureRawMETAR = rawMetarText;
                    }
                    else
                    {
                        UpdateStatus($"Airport {icao} not found in database");
                        Logger.WriteLine($"Airport {icao} not found in database");
                    }
                }
                else
                {
                    UpdateStatus($"Invalid ICAO code {icao}");
                    Logger.WriteLine($"Invalid ICAO code {icao}");
                }

            }

            if (eventArg.reason == SimEventArg.EventType.SETDESTINATION)
            {
                string icao = eventArg.value;
                if (Regex.IsMatch(icao, @"^[A-Z0-9]{4}$"))
                {
                    //check if the airport is in the list
                    Aeroport a = simdata.aeroports.FirstOrDefault(x => x.ident == icao);
                    if (a != null)
                    {
                        arrivalICAO = icao;
                        cbICAO.Text = icao;
                        string rawMetarText = await getRawMetarText(cbICAO.Text);
                        arrivalRawMETAR = rawMetarText;
                    }
                    else
                    {
                        UpdateStatus($"Airport {icao} not found in database");
                        Logger.WriteLine($"Airport {icao} not found in database");
                    }
                }
                else
                {
                    UpdateStatus($"Invalid ICAO code {icao}");
                    Logger.WriteLine($"Invalid ICAO code {icao}");
                }
            }
        }

        public string createMArkdownReport()
        {
            //build a meteo report in markdown format
            string report = "# METAR Report\n\n";
            report += "## Departure Airport: " + departureICAO + "\n\n";
            report += "### Raw METAR:\n";
            report += "```\n" + departureRawMETAR + "\n```\n\n";
            METARData departureMetarData = decodeMetar(departureRawMETAR);
            if (departureMetarData != null)
            {
                try
                {
                    string decodedDepartureMETAR = decodeMetar(departureRawMETAR).toString();
                    report += "### Decoded METAR:\n";
                    report += "```\n" + decodedDepartureMETAR + "\n```\n\n";
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Error decoding departure METAR for report " + ex.Message);
                }
            }

            report += "## Arrival Airport: " + arrivalICAO + "\n\n";
            report += "### Raw METAR:\n";
            report += "```\n" + arrivalRawMETAR + "\n```\n\n";
            METARData arrivalMetarData = decodeMetar(arrivalRawMETAR);
            if (arrivalMetarData != null)
            {
                try
                {
                    string decodedArrivalMETAR = decodeMetar(arrivalRawMETAR).toString();
                    report += "### Decoded METAR:\n";
                    report += "```\n" + decodedArrivalMETAR + "\n```\n\n";
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Error decoding arrival METAR for report " + ex.Message);
                }
            }
            return report;
        }

        public string createHTMLReport()
        {
            //build a meteo report in HTML format
            string report = "<h2>METAR Report</h2>\n";
            report += "<h3>Departure Airport: " + departureICAO + "</h3>\n";
            report += "<h4>Raw METAR:</h4>\n";
            report += "<pre>" + departureRawMETAR + "</pre>\n";
            METARData departureMetarData = decodeMetar(departureRawMETAR);
            if (departureMetarData != null)
            {
                try
                {
                    string decodedDepartureMETAR = decodeMetar(departureRawMETAR).toString();
                    report += "<h4>Decoded METAR:</h4>\n";
                    report += "<pre>" + decodedDepartureMETAR + "</pre>\n";
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Error decoding departure METAR for report " + ex.Message);
                }
            }
            report += "<h3>Arrival Airport: " + arrivalICAO + "</h3>\n";
            report += "<h4>Raw METAR:</h4>\n";
            report += "<pre>" + arrivalRawMETAR + "</pre>\n";
            METARData arrivalMetarData = decodeMetar(arrivalRawMETAR);
            if (arrivalMetarData != null)
            {
                try
                {
                    string decodedArrivalMETAR = decodeMetar(arrivalRawMETAR).toString();
                    report += "<h4>Decoded METAR:</h4>\n";
                    report += "<pre>" + decodedArrivalMETAR + "</pre>\n";
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Error decoding arrival METAR for report " + ex.Message);
                }
            }
            return report;
        }

        public string createJSONReport()
        {
            string report = string.Empty;
            //create a json object containing the metar data for departure and arrival
            report = "{\n";

            METARData departureMetarData = decodeMetar(departureRawMETAR);
            METARData arrivalMetarData = decodeMetar(arrivalRawMETAR);
            report += "\"departure\": " + System.Text.Json.JsonSerializer.Serialize(departureMetarData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }) + ",\n";
            report += "\"arrival\": " + System.Text.Json.JsonSerializer.Serialize(arrivalMetarData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }) + "\n";
            report += "}\n";
            return report;
        }

        public string getFlightReport(REPORTFORMAT format)
        {
            string report = string.Empty;
            switch (format)
            {
                case REPORTFORMAT.MD:
                    report = createMArkdownReport();
                    break;
                case REPORTFORMAT.JSON:
                    report = createJSONReport();
                    break;
                case REPORTFORMAT.HTML:
                        report = createHTMLReport();
                    break;
                default:
                    report = "{ \"error\": \"Unknown format requested\" }";
                    break;
            }
            return report;
        }
    }
}
