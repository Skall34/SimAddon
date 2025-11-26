using SimAddonPlugin;
using SimDataManager;

namespace Calculator
{
    public partial class ConverterCtrl : UserControl, ISimAddonPluginCtrl
    {
        List<string> capacityUnits = new List<string>
        {
            "Liters",
            "US Gallons",
            "Imp. G."
        };

        List<string> fuelUnits = new List<string>
        {
            "Kilograms",
            "lbs",
            "Liters"
        };

        List<string> distanceUnits = new List<string>
        {
            "Meters",
            "Feet",
            "Nautical Miles",
            "Statute Miles"
        };

        List<string> pressureUnits = new List<string>
        {
            "psi",
            "In Hg",
            "mbars"
        };

        List<string> temperatureUnits = new List<string>
        {
            "Celsius",
            "Fahrenheit",
            "Kelvin"
        };

        List<string> speedUnits = new List<string>
        {
            "Km/h",
            "Ft/S",
            "Knots",
            "Miles/H"
        };

        public ConverterCtrl()
        {
            InitializeComponent();
            CapaConverter.units = capacityUnits;
            FuelConverter.units = fuelUnits;
            DistanceConverter.units = distanceUnits;
            PressureConverter.units = pressureUnits;
            TempConverter.units = temperatureUnits;
            SpeedConverter.units = speedUnits;

            CapaConverter.OnValueChanged += CapaConverter_OnValueChanged;
            FuelConverter.OnValueChanged += FuelConverter_OnValueChanged;
            DistanceConverter.OnValueChanged += DistanceConverter_OnValueChanged;
            PressureConverter.OnValueChanged += PressureConverter_OnValueChanged;
            TempConverter.OnValueChanged += TempConverter_OnValueChanged;
            SpeedConverter.OnValueChanged += SpeedConverter_OnValueChanged;


        }

        private float SpeedConverter_OnValueChanged(object sender, Converter.ConvertEventArgs e)
        {
            float result = float.NaN;
            // Convert input value to knots
            float valueInKnots = e.srcUnit switch
            {
                "Km/h" => e.inputValue / 1.852f,
                "Ft/S" => e.inputValue / 1.68781f,
                "Knots" => e.inputValue,
                "Miles/H" => e.inputValue / 1.15078f,
                _ => throw new NotImplementedException(),
            };
            // Convert knots to destination unit
            result = e.dstUnit switch
            {
                "Km/h" => valueInKnots * 1.852f,
                "Ft/S" => valueInKnots * 1.68781f,
                "Knots" => valueInKnots,
                "Miles/H" => valueInKnots * 1.15078f,
                _ => throw new NotImplementedException(),
            };
            return result;
        }

        private float TempConverter_OnValueChanged(object sender, Converter.ConvertEventArgs e)
        {
            float result = float.NaN;
            // Convert input value to Celsius
            float valueInCelsius = e.srcUnit switch
            {
                "Celsius" => e.inputValue,
                "Fahrenheit" => (e.inputValue - 32) * 5 / 9,
                "Kelvin" => e.inputValue - 273.15f,
                _ => throw new NotImplementedException(),
            };
            // Convert Celsius to destination unit
            result = e.dstUnit switch
            {
                "Celsius" => valueInCelsius,
                "Fahrenheit" => (valueInCelsius * 9 / 5) + 32,
                "Kelvin" => valueInCelsius + 273.15f,
                _ => throw new NotImplementedException(),
            };
            return result;
        }

        private float PressureConverter_OnValueChanged(object sender, Converter.ConvertEventArgs e)
        {
            float result = float.NaN;
            // Convert input value to mbars
            float valueInMbars = e.srcUnit switch
            {
                "psi" => e.inputValue * 68.9476f,
                "In Hg" => e.inputValue * 33.8639f,
                "mbars" => e.inputValue,
                _ => throw new NotImplementedException(),
            };
            // Convert mbars to destination unit
            result = e.dstUnit switch
            {
                "psi" => valueInMbars / 68.9476f,
                "In Hg" => valueInMbars / 33.8639f,
                "mbars" => valueInMbars,
                _ => throw new NotImplementedException(),
            };
            return result;
        }

        private float DistanceConverter_OnValueChanged(object sender, Converter.ConvertEventArgs e)
        {
            float result = float.NaN;
            // Convert input value to meters
            float valueInMeters = e.srcUnit switch
            {
                "Meters" => e.inputValue,
                "Feet" => e.inputValue * 0.3048f,
                "Nautical Miles" => e.inputValue * 1852f,
                "Statute Miles" => e.inputValue * 1609.34f,
                _ => throw new NotImplementedException(),
            };
            // Convert meters to destination unit
            result = e.dstUnit switch
            {
                "Meters" => valueInMeters,
                "Feet" => valueInMeters / 0.3048f,
                "Nautical Miles" => valueInMeters / 1852f,
                "Statute Miles" => valueInMeters / 1609.34f,
                _ => throw new NotImplementedException(),
            };
            return result;
        }

        private float FuelConverter_OnValueChanged(object sender, Converter.ConvertEventArgs e)
        {
            float result = float.NaN;
            float fuelDensity = 0.8f; // Assuming average fuel density of 0.8 kg/L
            // Convert input value to Liters
            float valueInLiters = e.srcUnit switch
            {
                "Kilograms" => e.inputValue / fuelDensity, 
                "lbs" => (e.inputValue / 2.20462f) / fuelDensity,
                "Liters" => e.inputValue,
                _ => throw new NotImplementedException(),
            };
            // Convert Liters to destination unit
            result = e.dstUnit switch
            {
                "Kilograms" => valueInLiters * fuelDensity,
                "lbs" => (valueInLiters * fuelDensity) * 2.20462f,
                "Liters" => valueInLiters,
                _ => throw new NotImplementedException(),
            };
            return result;
        }

        private float CapaConverter_OnValueChanged(object sender, Converter.ConvertEventArgs e)
        {
            float result = float.NaN;
            // Convert input value to Liters
            float valueInLiters = e.srcUnit switch
            {
                "Liters" => e.inputValue,
                "US Gallons" => e.inputValue * 3.78541f,
                "Imp. G." => e.inputValue * 4.54609f,
                _ => throw new NotImplementedException(),
            };
            // Convert Liters to destination unit
            result = e.dstUnit switch
            {
                "Liters" => valueInLiters,
                "US Gallons" => valueInLiters / 3.78541f,
                "Imp. G." => valueInLiters / 4.54609f,
                _ => throw new NotImplementedException(),
            };
            return result;
        }

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;
        public event ISimAddonPluginCtrl.OnShowDialogHandler OnShowDialog;

        public void FormClosing(object sender, FormClosingEventArgs e)
        {
            throw new NotImplementedException();
        }

        public string getName()
        {
            return "Convert";
        }

        public void init(ref simData _data)
        {
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
        }

        public void SetExecutionFolder(string path)
        {
        }

        public void SetWindowMode(ISimAddonPluginCtrl.WindowMode mode)
        {
        }

        public void updateSituation(situation data)
        {
        }
    }
}
