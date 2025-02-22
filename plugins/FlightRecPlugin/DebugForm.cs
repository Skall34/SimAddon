using SimAddonPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FlightRecPlugin
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        public void updateInfos(situation currentFlightStatus)
        {
            listView1.Items.Clear();

            if (currentFlightStatus != null)
            {
                Type type = currentFlightStatus.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo prop in properties)
                {
                    object value = prop.GetValue(currentFlightStatus, null) ?? "NULL";
                    ListViewItem item = new ListViewItem(new[] { prop.Name, value.ToString() });
                    listView1.Items.Add(item);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
