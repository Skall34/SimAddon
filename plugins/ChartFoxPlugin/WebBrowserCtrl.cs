using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Net.Http.Json;
using System.Text.Json;

namespace ChartFoxPlugin
{
    public partial class WebBrowserCtrl : UserControl, ISimAddonPluginCtrl
    {
        public class JsonData
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        JsonData settings;

        public WebBrowserCtrl()
        {
            loadSettings();
            InitializeComponent();

            InitializeWebView2();
        }

        void loadSettings()
        {
            string currentDir = System.Reflection.Assembly.GetExecutingAssembly().Location;
            
            string filePath = Path.Combine(Path.GetDirectoryName(currentDir),"settings.json"); // Path to your JSON file
            try
            {
                // Read the JSON file content
                string jsonContent = File.ReadAllText(filePath);

                // Deserialize the JSON content into a C# object
                settings = JsonSerializer.Deserialize<JsonData>(jsonContent);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An error occurred: {ex.Message}");
                settings = new JsonData();
                settings.Name = "google";
                settings.Url = "https://www.google.com";
                string jsonContent = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                try
                {
                    // Write the JSON content to the file
                    File.WriteAllText(filePath, jsonContent);

                    Logger.WriteLine($"JSON file successfully written to {filePath}");
                }
                catch (Exception ex2)
                {
                    Logger.WriteLine($"An error occurred: {ex2.Message}");
                }
            }
        }

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;

        public void FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        public string getName()
        {
            return settings.Name;
        }

        public void init(ref simData _data)
        {
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
        }

        public void updateSituation(situation data)
        {
        }
        private async void InitializeWebView2()
        {
            await webView21.EnsureCoreWebView2Async(null); // Initializes the control
            webView21.Source = new Uri(settings.Url);
        }
    }
}
