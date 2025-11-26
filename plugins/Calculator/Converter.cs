using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Converter : UserControl
    {
        public class ConvertEventArgs : EventArgs
        {
            public float inputValue;
            public string srcUnit;
            public string dstUnit;
            public ConvertEventArgs(float inputValue, string srcUnit, string dstUnit)
            {
                this.inputValue = inputValue;
                this.srcUnit = srcUnit;
                this.dstUnit = dstUnit;
            }
        }

        public delegate float OnValueChangedHandler(object sender, ConvertEventArgs e);
        public event OnValueChangedHandler OnValueChanged;

        public List<string> units
        {
            get
            {
                return new List<string>
                {
                };
            }
            set
            {
                cbSrcUnits.Items.Clear();
                cbDstUnits.Items.Clear();
                foreach (string unit in value)
                {
                    cbSrcUnits.Items.Add(unit);
                    cbDstUnits.Items.Add(unit);
                }
            }
        }

        public Converter()
        {
            InitializeComponent();
        }

        private void convertValue()
        {
            try
            {
                float val = float.Parse(tbInput.Text);
                string srcUnit = cbSrcUnits.SelectedItem as string;
                string dstUnit = cbDstUnits.SelectedItem as string;
                ConvertEventArgs args = new ConvertEventArgs(val, srcUnit, dstUnit);
                float? result = OnValueChanged?.Invoke(this, args);
                lblResult.Text = result.HasValue ? result.Value.ToString() : "N/A";
            }
            catch
            {
                // Ignore parse errors
                lblResult.Text = "N/A";
            }
        }

        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            convertValue();
        }

        private void cbSrcUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            convertValue();
        }

        private void cbDstUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            convertValue();
        }
    }
}
