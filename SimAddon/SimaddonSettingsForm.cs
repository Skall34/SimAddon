using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimAddon
{
    public partial class SimaddonSettingsForm : Form
    {
        public bool AlwaysOnTop
        {
            get { return cbAlwaysOnTop.Checked; }
            set { cbAlwaysOnTop.Checked = value; }
        }

        public bool AutoHide
        {
            get { return cbAutoHide.Checked; }
            set { cbAutoHide.Checked = value; }
        }

        public string screenshotFolder
        {
            get { return tbScreenshotsFolder.Text; }
            set { tbScreenshotsFolder.Text = value; }
        }

        public string SiteUrl
        {
            get { return tbSiteUrl.Text; }
            set { tbSiteUrl.Text = value; }
        }

        public Dictionary<string, bool> VisiblePlugins { get; set; } = new Dictionary<string, bool>();

        public SimaddonSettingsForm()
        {
            InitializeComponent();
        }

        private void cbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SimaddonSettingsForm_Load(object sender, EventArgs e)
        {
            // Populate the plugins list view
            listView1.Items.Clear();
            foreach (var plugin in VisiblePlugins)
            {
                ListViewItem item = new ListViewItem(plugin.Key);
                item.Checked = plugin.Value;
                listView1.Items.Add(item);
            }
            //set the listview column width to fit the content
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        }

        private void btnChooseScreenshotFolder_Click(object sender, EventArgs e)
        {
            // Open a folder browser dialog to select the screenshots folder
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select Screenshots Folder";
                folderBrowserDialog.SelectedPath = tbScreenshotsFolder.Text;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    tbScreenshotsFolder.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // Update the VisiblePlugins dictionary based on the list view items
            foreach (ListViewItem item in listView1.Items)
            {
                VisiblePlugins[item.Text] = item.Checked;
            }

            //set the dialog result to OK
            this.DialogResult = DialogResult.OK;
            // Close the form
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void tbSiteUrl_TextChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void linkTest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //open the link in the default browser
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = tbSiteUrl.Text,
                UseShellExecute = true
            });
        }
    }
}
