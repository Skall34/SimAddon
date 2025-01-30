using System;

namespace SimDataManager
{
    public class FlightPerfs
    {
        public bool stallWarning { get; set; }

        public bool crashed { get; set; }
        public bool overRunwayCrashed { get; set; }
        public bool overspeed { get; set; }

        private double _landingVSpeed;
        public double landingVSpeed { get {
                return _landingVSpeed;
            }
            set {
                //only keep new value if it's worse that before.
                //this is to keep the worst value un case of rebound.

                //hack : sometime the sim give a positive vertical speed during landing which is NOT possible.
                // so reverse it.
                double newValue = value<=0?value:-value;
                if (newValue < _landingVSpeed)
                {
                    _landingVSpeed = newValue;
                }
            }
        }
        public double landingVerticalAcceleration { get; set; }
        public double landingSpeed { get; set; }
        public double flapsDownSpeed { get; set; }
        public double gearDownSpeed { get; set; }

        public double takeOffWeight { get; set; }
        public double landingWeight { get; set; }

        public DateTime takeOffTime{ get; set; }
        public DateTime landingTime { get; set; }

        public FlightPerfs()
        {
            reset();
        }

        public void reset()
        {
            overspeed = false;
            crashed = false;
            overRunwayCrashed = false;
            stallWarning = false;

            _landingVSpeed = 0;
            landingVerticalAcceleration = 0;
            landingSpeed = 0;

            takeOffWeight = 0;
            landingWeight = 0;
        }

        public short getFlightNote() {
            short note = 10;
            if (flapsDownSpeed > 130)
            {
                note -= 1; // pareil que note = note -1
            }

            if (gearDownSpeed > 130)
            {
                note -= 1;
            }

            if (landingVSpeed > 500)
            {
                note -= 2;
            }

            if (overspeed) note -= 2; // note = note -2
            if (stallWarning) note -= 2;
            if (overRunwayCrashed) note = 2;
            if (crashed) note = 1;
            return note; 
        }

        public string getFlightNoteDetails()
        {
            string result = "";
            if (overspeed) result += "Overspeed detected \n";
            if (stallWarning) result += "Stall warning detected \n";
            if (overRunwayCrashed) result += "Over runway crashed \n";
            if (crashed) result += "Crashed \n";
            result += "vertical speed at touchdown: " + landingVSpeed + " fpm\n";
            result += "gear down speed : " + gearDownSpeed + " m/s\n";
            result += "flaps down speed : " + flapsDownSpeed + " m/s\n";

            return result;
        }

        public string getFlightComment()
        {
            string result = "";

            result = "Landing speed : " + landingSpeed.ToString("0.00") + " Knts ";
            result += " Landing vertical speed : " + landingVSpeed.ToString("0.00") + " fpm ";
            result += " Takeoff weight : " + takeOffWeight.ToString("0.00") + " Kg ";
            result += " Landing weight : " + landingWeight.ToString("0.00") + " Kg ";
            result += " OFF : " + takeOffTime.ToShortTimeString();
            result += " ON : " + landingTime.ToShortTimeString();

            return result;
        }

    }
}
