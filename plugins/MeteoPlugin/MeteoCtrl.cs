using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Drawing.Text;
using System.Reflection;

namespace MeteoPlugin
{
    public partial class MeteoCtrl : UserControl, ISimAddonPluginCtrl
    {
        private Font customFont;
        PrivateFontCollection fontCollection;
        simData simdata;

        public MeteoCtrl()
        {
            LoadCustomFont();
            InitializeComponent();
            if (fontCollection != null)
            {
                lblDecodedMETAR.Font = new Font(fontCollection.Families[0], lblDecodedMETAR.Font.Size);
                tbMETAR.Font = new Font(fontCollection.Families[0], tbMETAR.Font.Size);
            }
            tbMETAR.Text = "Request fo METAR informations...";
            this.cbICAO.ValueMember = "fullName";
            this.cbICAO.DisplayMember = "fullName";
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

        private async void requestForMetar()
        {
            //create a request to https://aviationweather.gov/cgi-bin/data/metar.php?ids=LFMT
            //https://vfrmap.com/?type=osm&lat=62.321&lon=-150.093&zoom=12&api_key=763xxE1MJHyhr48DlAP2qQ

            string searchItem = "";
            if (cbICAO.SelectedItem!=null)
            {
                searchItem = ((Aeroport)cbICAO.SelectedItem).ident;
            }
            else
            {
                searchItem = cbICAO.Text;
            }

            string metar = await Meteo.getMetar(searchItem);
            tbMETAR.Text = metar;
            try
            {
                lblDecodedMETAR.Text = Meteo.decodeMetar(metar);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
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

    }
}
