namespace SimDataManager
{
    public class FlightPerfs
    {
        public bool stallWarning { get; set; }

        public bool crashed { get; set; }
        public bool overRunwayCrashed { get; set; }
        public bool overspeed { get; set; }

        public double touchDownVSpeed { get; set; }
        public double landingVerticalAcceleration { get; set; }
        public double landingSpeed { get; set; }
        public double flapsDownSpeed { get; set; }
        public double gearDownSpeed { get; set; }

        public double takeOffWeight { get; set; }
        public double landingWeight { get; set; }

        public FlightPerfs()
        {
            overspeed = false;
            crashed = false;
            overRunwayCrashed = false;
            stallWarning = false;

            touchDownVSpeed = 0;
            landingVerticalAcceleration = 0;
            landingSpeed = 0;

            takeOffWeight = 0;
            landingWeight = 0;
        }

        public int getFlightNote() {
            int note = 10;
            if (flapsDownSpeed > 130)
            {
                note -= 1; // pareil que note = note -1
            }

            if (gearDownSpeed > 130)
            {
                note -= 1;
            }

            if (touchDownVSpeed > 500)
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
            result += "vertical speed at touchdown: " + touchDownVSpeed + " fpm\n";
            result += "gear down speed : " + gearDownSpeed + " m/s\n";
            result += "flaps down speed : " + flapsDownSpeed + " m/s\n";

            return result;
        }

        public string getFlightComment()
        {
            string result = "";

            result = "Landing speed : " + landingSpeed.ToString("0.00") + " Knts ";
            result += " Landing vertical speed : " + touchDownVSpeed.ToString("0.00") + " fpm ";
            result += " Takeoff weight : " + takeOffWeight.ToString("0.00") + " Kg ";
            result += " Landing weight : " + landingWeight.ToString("0.00") + " Kg ";

            return result;
        }

    }
}
