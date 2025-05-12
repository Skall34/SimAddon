using Microsoft.Web.WebView2.Core;
using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;

namespace ChartFoxPlugin
{
    public partial class WebBrowserCtrl : UserControl, ISimAddonPluginCtrl
    {
        public class JsonData
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        private string executionFolder;

        JsonData settings;

        public WebBrowserCtrl()
        {
            InitializeComponent();
        }

        void loadSettings()
        {
            string currentDir = executionFolder;

            string filePath = Path.Combine(currentDir, "settings.json"); // Path to your JSON file
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

            InitializeWebView2();
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
            // Get the application name
            //string appName = Assembly.GetEntryAssembly().GetName().Name;
            string appName = settings.Name;

            // Get the path to the user's AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Combine the AppData path with the folder name
            string fullPath = Path.Combine(appDataPath, appName);

            CoreWebView2Environment cwv2Environment = await CoreWebView2Environment.CreateAsync(null, fullPath, new CoreWebView2EnvironmentOptions());
            await webView21.EnsureCoreWebView2Async(cwv2Environment);
            webView21.Source = new Uri(settings.Url);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            webView21.Source = new Uri(settings.Url);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            webView21.GoBack();
        }

        void ISimAddonPluginCtrl.SetExecutionFolder(string path)
        {
            executionFolder = path;
            loadSettings();
        }

        public void ManageSimEvent(object sender, SimEventArg eventArg)
        {
        }
    }
}
