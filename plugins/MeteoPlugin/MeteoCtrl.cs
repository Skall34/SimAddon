using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System;
using System.Drawing.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static MeteoPlugin.Meteo;

namespace MeteoPlugin
{
    public partial class MeteoCtrl : UserControl, ISimAddonPluginCtrl
    {
        private Font customFont;
        PrivateFontCollection fontCollection;
        simData simdata;
        METARData metarData;

        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;

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
            LoadCustomFont();
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


        private void LoadCustomFont()
        {
            fontCollection = new PrivateFontCollection();

            // Load font from embedded resource
            var fontStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("MeteoPlugin.Font.LCD2B___.TTF");

            if (fontStream != null)
            {
                byte[] fontData = new byte[fontStream.Length];
                fontStream.Read(fontData, 0, (int)fontStream.Length);

                unsafe
                {
                    fixed (byte* pFontData = fontData)
                    {
                        fontCollection.AddMemoryFont((IntPtr)pFontData, fontData.Length);
                    }
                }
                // Create the font object with desired size
            }
        }


        public string getName()
        {
            return "Meteo";
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

        public void updateSituation(situation data)
        {
            try
            {
                //todo : rafraichis la list des aéroports assez proches pour être interrogés.
                if ((simdata != null) && (simdata.isConnected))
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

        private async void requestForMetar()
        {
            //create a request to https://aviationweather.gov/cgi-bin/data/metar.php?ids=LFMT
            //https://vfrmap.com/?type=osm&lat=62.321&lon=-150.093&zoom=12&api_key=763xxE1MJHyhr48DlAP2qQ

            UpdateStatus("Requesting METAR informations");
            Logger.WriteLine("Requesting METAR informations");

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
            lbAirportInfo.Items.Clear();
            lblDecodedMETAR.Text = string.Empty;
            string rawMetarText = await Meteo.getMetar(searchItem);
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

                    //show airport information in panel
                    if ((simdata != null) && (simdata.aeroports != null))
                    {
                        Aeroport airport = simdata.aeroports.FirstOrDefault(a => a.ident == searchItem);
                        if (airport != null)
                        {
                            //update the name of the airport in the metar data if there any
                            if (metarData != null)
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
            catch (Exception ex)
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
    }
}
