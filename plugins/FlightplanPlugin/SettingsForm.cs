using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlightplanPlugin
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public string SimbriefUserName
        {
            get
            {
                return tbSimbriefUsername.Text;
            }

            set
            {
                tbSimbriefUsername.Text = value;
            }
        }

        public string pdfStorageFolder
        {
            get
            {
                return tbPdfFolder.Text;
            }

            set
            {
                tbPdfFolder.Text = value;
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnBrowsePdfFolder_Click(object sender, EventArgs e)
        {
            //open a dialog to allow the user to choose a storage folder
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                pdfStorageFolder = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
