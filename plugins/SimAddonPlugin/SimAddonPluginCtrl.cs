using SimDataManager;
using System.Windows.Forms;


namespace SimAddonPlugin
{
    public interface ISimAddonPluginCtrl
    {
        //simData data;

        //const string name = "stubPlugin";
        enum WindowMode
        {
            FULL,
            COMPACT           
        }

        void SetWindowMode(WindowMode mode);

        //methodes
        string getName();

        void init(ref simData _data);
        void registerPage(TabControl parent);

        void updateSituation(situation data);

        //request to terminate to the plugin. 
        //must return true, if terminate can be done OK,
        //or false if termination should be canceled
        void FormClosing(object sender, FormClosingEventArgs e);

        //events
        public delegate void UpdateStatusHandler(object sender, string statusMessage);
        public event UpdateStatusHandler OnStatusUpdate;
    }

    public  class situation
    {
        public bool readyToFly { get; set; }
        public double airSpeed { get; set; }
        public short crashedFlag { get; set; }
        public double currentFuel { get; set; }
        public uint flapsAvailableFlag { get; set; }
        public uint flapsPosition { get; set; }
        public bool gearIsUp { get; set; }
        public uint gearRetractableFlag { get; set; }
        public bool isAtLeastOneEngineFiring { get; set; }
        public double landingVerticalSpeed { get; set; }
        public short offRunwayCrashed { get; set; }
        public short onGround { get; set; }
        public byte overSpeedWarning { get; set; }
        public double payload { get; set; }
        public double planeWeight { get; set; }
        public byte stallWarning { get; set; }

        public bool MasterAvionicsOn { get; set; }

        public bool MasterBatteryOn { get; set; }

        public double verticalAcceleration { get; set; }

        public double magVariation { get; set; }

        public  PositionSnapshot position { get; set; }

    }
}
