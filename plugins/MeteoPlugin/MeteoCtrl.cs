using SimAddonPlugin;
using SimDataManager;

namespace MeteoPlugin
{
    public partial class MeteoCtrl : UserControl, ISimAddonPluginCtrl
    {
        public MeteoCtrl()
        {
            InitializeComponent();
        }

        public string getName()
        {
            return "Meteo";
        }

        public void init(ref simData _data)
        {
            throw new NotImplementedException();
        }

        public void registerPage(TabControl parent)
        {
            parent.SuspendLayout();
            TabPage pluginPage = new TabPage();
            pluginPage.Text = getName();
            pluginPage.Controls.Add(this);
            this.Dock = DockStyle.Fill;
            pluginPage.Visible = true;
            parent.TabPages.Add(pluginPage);
            parent.ResumeLayout();
        }

        public void updateSituation(situation data)
        {
            throw new NotImplementedException();
        }

        private async void btnRequest_Click(object sender, EventArgs e)
        {
            //create a request to https://aviationweather.gov/cgi-bin/data/metar.php?ids=LFMT

            string metar = await Meteo.getMetar(tbICAO.Text);
            tbMETAR.Text = metar;
            tbDecodedMETAR.Text = Meteo.decodeMetar(metar);
        }

        private void MeteoCtrl_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbDecodedMETAR.Text = Meteo.decodeMetar(tbMETAR.Text);

        }
    }
}
