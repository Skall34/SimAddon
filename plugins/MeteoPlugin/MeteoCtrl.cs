using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System;
using System.Drawing.Text;
using System.Reflection;

namespace MeteoPlugin
{
    public partial class MeteoCtrl : UserControl, ISimAddonPluginCtrl
    {
        private Font customFont;
        PrivateFontCollection fontCollection;

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
            throw new NotImplementedException();
        }

        private async void requestForMetar()
        {
            //create a request to https://aviationweather.gov/cgi-bin/data/metar.php?ids=LFMT
            //https://vfrmap.com/?type=osm&lat=62.321&lon=-150.093&zoom=12&api_key=763xxE1MJHyhr48DlAP2qQ

            string metar = await Meteo.getMetar(tbICAO.Text);
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

        private void MeteoCtrl_Load(object sender, EventArgs e)
        {

        }

        private void tbDecodedMETAR_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbICAO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                requestForMetar();
            }
        }

        private void tbICAO_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblDecodedMETAR_Click(object sender, EventArgs e)
        {

        }
    }
}
