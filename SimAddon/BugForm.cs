using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Octokit;

namespace FlightRecorder
{
    public partial class BugForm : Form
    {
        private string githubtoken;
        private string googleSheetUrl;
        public BugForm(string callsign,List<string> data,string baseUrl)
        {
            InitializeComponent();
            tbTitle.Text = callsign + " problem : ";
            tbDesc.Text = "Describe the problem:\r\n\r\n\r\n==================\r\nLog starts here \r\n================\r\n";
            foreach (string l in data)
            {
                tbDesc.Text += l+"\r\n";
            }
            googleSheetUrl = baseUrl;
            
        }

        //public async Task<string> getGitHubToken()
        //{
        //    string url = googleSheetUrl + "?query=github_token";
        //    UrlDeserializer dataReader = new UrlDeserializer(url);
        //    string? token;
        //    Logger.WriteLine("Fechting airport informations from server");
        //    token = await dataReader.FetchGithubTokenAsync();
        //    return token;
        //}

        private async void btnSend_Click(object sender, EventArgs e)
        {
            //// Replace these with your GitHub credentials and repository details
            //var owner = "Skall34";
            //var repoName = "FlightRecorder";
            
            //string githubtoken = await getGitHubToken();
            //// Initialize the GitHub client
            //var client = new GitHubClient(new ProductHeaderValue("FlightRecorder"))
            //{
            //    Credentials = new Credentials(githubtoken)
            //};

            //// Create a new issue
            //var newIssue = new NewIssue(tbTitle.Text)
            //{
            //    Body = tbDesc.Text
            //};
            //try
            //{
            //    var issue = await client.Issue.Create(owner, repoName, newIssue);
            //    Logger.WriteLine($"Issue created: {issue.HtmlUrl}");
            //    MessageBox.Show($"Nous essaierons de le traiter au plus vite", "Bug envoyé !");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"{ex.Message}","Erreur lors de l'envoi du bug");
            //}
            //this.Close();
        }
    }
}
