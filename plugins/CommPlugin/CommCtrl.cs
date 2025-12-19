using SimAddonPlugin;
using SimDataManager;
using System.Drawing.Text;
using System.Reflection;
using static SimAddonPlugin.ISimAddonPluginCtrl;

namespace CommPlugin
{
    public partial class CommCtrl : UserControl, ISimAddonPluginCtrl
    {
        private simData? simdata;
        private float COM1StdbyFrequency = 121.50f; // Default standby frequency
        private float COM1Frequency = 121.50f; // Default COM1 frequency
        private bool gotData = false;
        PrivateFontCollection fontCollection;


        public CommCtrl()
        {
            LoadCustomFont();
            InitializeComponent();

            if (fontCollection != null)
            {
                lblCom1.Font = new Font(fontCollection.Families[0], 18, FontStyle.Regular);
                lblComStdby.Font = new Font(fontCollection.Families[0], 18, FontStyle.Regular);
                lblSquawk1.Font = new Font(fontCollection.Families[0], 28, FontStyle.Regular);
                lblSquawk2.Font = new Font(fontCollection.Families[0], 28, FontStyle.Regular);
                lblSquawk3.Font = new Font(fontCollection.Families[0], 28, FontStyle.Regular);
                lblSquawk4.Font = new Font(fontCollection.Families[0], 28, FontStyle.Regular);
            }
        }

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;
        public event ISimAddonPluginCtrl.OnShowDialogHandler OnShowDialog;

        private void LoadCustomFont()
        {
            fontCollection = new PrivateFontCollection();

            // Load font from embedded resource
            var fontStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("CommPlugin.Font.LCD2B___.TTF");

            if (fontStream != null)
            {
                byte[] fontData = new byte[fontStream.Length];
                fontStream.Read(fontData, 0, (int)fontStream.Length);

                unsafe
                {
                    fixed (byte* pFontData = fontData)
                    {
                        fontCollection.AddMemoryFont((IntPtr)pFontData, fontData.Length);
                    }
                }
                // Create the font object with desired size
            }
        }

        public void FormClosing(object sender, FormClosingEventArgs e)
        {
            throw new NotImplementedException();
        }

        public string getName()
        {
            return ("Comm");
        }

        public void init(ref simData _data)
        {
            simdata = _data;
        }

        public void ManageSimEvent(object sender, SimEventArg eventArg)
        {
        }

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

        public void SetExecutionFolder(string path)
        {
            throw new NotImplementedException();
        }

        public void SetWindowMode(ISimAddonPluginCtrl.WindowMode mode)
        {
            throw new NotImplementedException();
        }

        public void updateSituation(situation data)
        {

            COM1Frequency = data.COM1Frequency;
            lblCom1.Text = COM1Frequency.ToString("F3", System.Globalization.CultureInfo.InvariantCulture);
            COM1StdbyFrequency = data.COM1StdbyFrequency;
            lblComStdby.Text = data.COM1StdbyFrequency.ToString("F3", System.Globalization.CultureInfo.InvariantCulture);

            int squawk = data.squawkCode;

            lblSquawk1.Text = ((squawk / 1000) % 10).ToString(); // Thousands place
            lblSquawk2.Text = ((squawk / 100) % 10).ToString(); // Hundreds place
            lblSquawk3.Text = ((squawk / 10) % 10).ToString(); // Tens place
            lblSquawk4.Text = (squawk % 10).ToString(); // Units place
            rotaryKnobMode.Value = data.squawkMode; // Set squawk mode

            // Update the rotary knobs with the new values
            if (!gotData)
            {
                rotaryKnob1.Value = (int)Math.Floor(COM1StdbyFrequency);
                rotaryKnob2.Value = (int)((COM1StdbyFrequency - Math.Floor(COM1StdbyFrequency)) * 1000); // Set the decimal part for rotary knob 2
                rotaryKnobS1.Value = (squawk / 1000) % 10; // Thousands place
                rotaryKnobS2.Value = (squawk / 100) % 10; // Hundreds place
                rotaryKnobS3.Value = (squawk / 10) % 10; // Tens place
                rotaryKnobS4.Value = squawk % 10; // Units place

                gotData = true;
            }

        }

        private void btnChangeCOM1Freq_Click(object sender, EventArgs e)
        {
            if (simdata != null)
            {
                if (simdata != null)
                {
                    simdata.SetCOM1(COM1StdbyFrequency);
                    simdata.SetCOM1Stdby(COM1Frequency);
                    //simdata.SwitchCOM1();

                    OnStatusUpdate?.Invoke(this, $"COM1 frequency changed to {COM1StdbyFrequency:F3}");
                }
                else
                {
                    OnStatusUpdate?.Invoke(this, "Error: simData is not initialized.");
                }
            }
        }

        private void CommCtrl_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void rotaryKnob1_ValueChanged(object sender, EventArgs e)
        {
            float value = rotaryKnob1.Value;
            if (value < 118)
            {
                value = 118; // Minimum COM1 frequency
            }
            else if (value > 136)
            {
                value = 136; // Maximum COM1 frequency
            }
            float newval = (float)(value + ((float)rotaryKnob2.Value) / 1000); // Add the decimal part from rotary knob 2
            simdata.SetCOM1Stdby(newval);
        }

        private void rotaryKnob2_ValueChanged(object sender, EventArgs e)
        {
            float value = rotaryKnob2.Value;
            if (value < 0)
            {
                value = 0; // Minimum decimal part
            }
            else if (value > 995)
            {
                value = 995; // Maximum decimal part
            }
            float newval = rotaryKnob1.Value + (value / 1000); // Combine with the integer part from rotary knob 1
            simdata.SetCOM1Stdby(newval);
        }

        private void rotaryKnobS1_ValueChanged(object sender, EventArgs e)
        {
            int squawk = (int)(rotaryKnobS1.Value * 1000 + rotaryKnobS2.Value * 100 + rotaryKnobS3.Value * 10 + rotaryKnobS4.Value);
            simdata.SetSquawk(squawk);
        }

        private void btnUnicom_Click(object sender, EventArgs e)
        {
            rotaryKnob1.Value = 122; // Set COM1 standby frequency to UNICOM
            rotaryKnob2.Value = 800; // Set decimal part to .800
            simdata.SetCOM1Stdby(122.8F);
        }

        private void rotaryKnobMode_ValueChanged(object sender, EventArgs e)
        {
            simdata.SetSquawkMode((byte)rotaryKnobMode.Value);
        }

        private void Off(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rotaryKnobMode_Load(object sender, EventArgs e)
        {

        }

        public string getFlightReport(REPORTFORMAT format)
        {
            throw new NotImplementedException();
        }
    }
}
