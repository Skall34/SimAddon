using Microsoft.Web.WebView2.Core;
using SimAddonLogger;
using SimAddonPlugin;
using SimDataManager;
using System.Text.Json;

namespace ATCPlugin
{
    public partial class WebBrowserCtrl : UserControl, ISimAddonPluginCtrl
    {
        public class ATCSettings
        {
            public string ATCName { get; set; }
            public string Url { get; set; }
            public string DestUrl { get; set; }
        }
        public class JsonSettings
        {
            public string Name { get; set; }
            public List<ATCSettings> ATCs { get; set; }
            public string lastATCUsed { get; set; }
        }

        private string executionFolder;

        JsonSettings settings;
        ATCSettings currentATCSettings;

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
                settings = JsonSerializer.Deserialize<JsonSettings>(jsonContent);

                //add menu entries for each ATC in the settings
                Logger.WriteLine($"JSON file successfully read from {filePath}");
                ATCMenu.DropDownItems.Clear();
                foreach (var atc in settings.ATCs)
                {
                    Logger.WriteLine($"Loaded ATC: {atc.ATCName}, URL: {atc.Url}, DestURL: {atc.DestUrl}");
                    //add each atc as menu item under the ATCMenu
                    ATCMenu.DropDownItems.Add(atc.ATCName, null, (s, e) =>
                    {
                        currentATCSettings = atc;
                        webView21.Source = new Uri(currentATCSettings.Url);
                        settings.lastATCUsed = atc.ATCName;
                        //save settings
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
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An error occurred: {ex.Message}");
                settings = new JsonSettings();
                settings.Name = "google";
                settings.ATCs = new List<ATCSettings>();
                settings.lastATCUsed = "Google";
                ATCSettings ATCsetting = new ATCSettings();
                ATCsetting.Url = "https://www.google.com";
                ATCsetting.DestUrl = "https://www.google.com/maps";
                ATCsetting.ATCName = "Google";
                settings.ATCs.Add(ATCsetting);
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
        public event ISimAddonPluginCtrl.OnShowDialogHandler OnShowDialog;

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

        //public void registerPage(TabControl parent)
        //{
        //    parent.SuspendLayout();
        //    TabPage pluginPage = new TabPage();
        //    pluginPage.Text = getName();
        //    pluginPage.Controls.Add(this);
        //    this.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        //    this.Dock = DockStyle.Fill;
        //    pluginPage.Visible = true;
        //    parent.TabPages.Add(pluginPage);
        //    parent.ResumeLayout();
        //}

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

            if (settings == null)
            {
                loadSettings();
            }
            try
            {
                currentATCSettings = settings.ATCs.Where(a => a.ATCName == settings.lastATCUsed).First();
                webView21.Source = new Uri(currentATCSettings.Url);
            }
            catch
            {
                currentATCSettings = settings.ATCs[0];
                webView21.Source = new Uri(currentATCSettings.Url);
            }
        }

        void ISimAddonPluginCtrl.SetExecutionFolder(string path)
        {
            executionFolder = path;
            loadSettings();
        }

        public void ManageSimEvent(object sender, SimEventArg eventArg)
        {
            if (eventArg.reason == SimEventArg.EventType.SETDESTINATION)
            {
                if (eventArg.value is string destination)
                {
                    try
                    {
                        // Assuming the destination is a URL
                        string destinationUrl = currentATCSettings.DestUrl.Replace("<destICAO>", destination);
                        Uri uri = new Uri(destinationUrl);
                        webView21.Source = uri;
                    }
                    catch (UriFormatException ex)
                    {
                        Logger.WriteLine($"Invalid URL format: {ex.Message}");
                    }
                }
                else
                {
                    Logger.WriteLine("Destination data is not a string.");
                }
            }
        }
    }
}
