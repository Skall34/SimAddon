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
    public partial class LoadingForm : Form
    {
        public int Progress { 
            get
            {
                return progressBar1.Value;
            }
            set
            {
                progressBar1.Value = value;
            }

            }

        public void updateProgress(int val, string status)
        {
            if (val < 0) val = 0;
            if (val > 100) val = 100;
            progressBar1.Value = val;
            textBox1.Text = status;
            Application.DoEvents();
        }

        public LoadingForm()
        {
            InitializeComponent();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            Progress = 0;
        }
    }
}
