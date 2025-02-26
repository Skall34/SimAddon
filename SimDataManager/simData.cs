﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSUIPC;
using SimAddonLogger;

//using Microsoft.VisualBasic.ApplicationServices;

namespace SimDataManager
{
    public class LatLonPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            string result=Latitude.ToString("0.000000") + "/" + Longitude.ToString("0.000000");
            return result;
        }
    }

    public class PositionSnapshot
    {
        //
        // Résumé :
        //     The location of the aircraft
        public LatLonPoint Location { get; set; }

        //
        // Résumé :
        //     The altitude of the aircraft
        public Double Altitude { get; set; }

        //
        // Résumé :
        //     The heading of the aircraft in degrees TRUE
        public double HeadingDegreesTrue { get; set; }

        //
        // Résumé :
        //     The pitch angle of teh aircraft in degrees
        public double PitchDegrees { get; set; }

        //
        // Résumé :
        //     The bank angle of the aircraft in degrees
        public double BankDegrees { get; set; }

        //
        // Résumé :
        //     The indicated airspeed of the aircraft in Knots
        public double IndicatedAirspeedKnots { get; set; }

        //
        // Résumé :
        //     True if the aircraft is on the ground.
        public bool IsOnGround { get; set; }

        public PositionSnapshot() { 
            Location = new LatLonPoint();
        }

        public override string ToString()
        {
            return Location.ToString();
        }

    }

    public class simData
    {

        // =====================================
        // DECLARE OFFSETS YOU WANT TO USE HERE
        // =====================================
        //https://www.projectmagenta.com/all-fsuipc-offsets/

        private readonly Offset<string> startSituation = new Offset<string>(0x0024, 256);
        private readonly Offset<short> magVariation    =  new Offset<short>(0x02A0);
        private readonly Offset<uint> airspeed         =   new Offset<uint>(0x02BC);
        private readonly Offset<int> verticalSpeed     =    new Offset<int>(0x02C8);

        private readonly Offset<int> landingVerticalSpeed = new Offset<int>(0x030C);

        private readonly Offset<short> onGround         = new Offset<short>(0x0366);
        private readonly Offset<byte> stallWarning       = new Offset<byte>(0x036C);
        private readonly Offset<byte> overSpeedWarning   = new Offset<byte>(0x036D);

        private readonly Offset<uint> initialized1 = new Offset<uint>(0x04D2);
        private readonly Offset<uint> initialized2 = new Offset<uint>(0x04D4);

        private readonly Offset<uint> gearRetractableFlag = new Offset<uint>(0x060C);

        private readonly Offset<uint> flapsAvailable = new Offset<uint>(0x0778);

        private readonly Offset<short> crashed          = new Offset<short>(0x0840);
        private readonly Offset<short> offRunwayCrashed = new Offset<short>(0x0848);

        private readonly Offset<short> engine1Firing = new Offset<short>(0x0894);
        private readonly Offset<short> engine2Firing = new Offset<short>(0x092C);
        private readonly Offset<short> engine3Firing = new Offset<short>(0x09C4);
        private readonly Offset<short> engine4Firing = new Offset<short>(0x0A5C);

        private readonly Offset<short> engineNumber = new Offset<short>(0x0AEC);

        private readonly Offset<uint> flapsPosition      = new Offset<uint>(0x0BE0);
        private readonly Offset<uint> gearPosition       = new Offset<uint>(0x0BF0);



        private readonly Offset<short> verticalAcceleration = new Offset<short>(0x11BA);

        private readonly Offset<uint> avionicsMaster = new Offset<uint>(0x2E80);
        private readonly Offset<uint> batteryMaster  = new Offset<uint>(0x281C);

        private readonly Offset<string> flightNumber = new Offset<string>(0x3130, 12);
        private readonly Offset<string> tailNumber   = new Offset<string>(0x313C, 12);

        private readonly Offset<byte> readyToFLy = new Offset<byte>(0x3364);
        private readonly Offset<byte> inMenu =     new Offset<byte>(0x3365);

        private readonly Offset<string> aircraftModel = new Offset<string>(0x3500, 24);
        private readonly Offset<string> aircraftType =  new Offset<string>(0x3D00, 256);

        private readonly Offset<byte> viewMode = new Offset<byte>(0x8320);

        //private Offset<byte> navLights = new Offset<byte>(0x0280);
        //private Offset<byte> beaconStrobe = new Offset<byte>(0x0281);
        //private Offset<byte> landingLights = new Offset<byte>(0x02BC);

        //30C Vertical speed, copy of offset 02C8 whilst airborne, not updated
        //whilst the “on ground” flag(0366) is set.Can be used to check
        //hardness of touchdown(but watch out for bounces which may
        //change this). 






        //private Offset<long> altitude = new Offset<long>(0x0570);
        //private Offset<uint> pitch = new Offset<uint>(0x0578);
        //private Offset<uint> bank = new Offset<uint>(0x057C);
        //private Offset<uint> heading = new Offset<uint>(0x0580);




        //private readonly Offset<short> fuelWheight = new Offset<short>(0x0AF4); //pounds per gallons * 256

        //private readonly Offset<uint> centerTankLevelPercent = new Offset<uint>(0x0B74); // % * 128 * 65536
        //private readonly Offset<uint> centerTankGallonsCapacity = new Offset<uint>(0x0B78);

        //private readonly Offset<uint> leftTankLevelPercent = new Offset<uint>(0x0B7C); // % * 128 * 65536
        //private readonly Offset<uint> leftTankGallonsCapacity = new Offset<uint>(0x0B80);

        //private readonly Offset<uint> rightTankLevelPercent = new Offset<uint>(0x0B94); // % * 128 * 65536
        //private readonly Offset<uint> rightTankGallonsCapacity = new Offset<uint>(0x0B98);

        //private readonly Offset<uint> payloadNumber = new Offset<uint>(0x13FC);
        //private Offset<FsPayloadStation> payloads = new Offset<FsPayloadStation>(0x1400, 48);


        //private readonly Offset<string> airlineName = new Offset<string>(0x3148, 24);

        private PayloadServices payloadServices;
        //private readonly PlayerLocationInfo locationInfos = new PlayerLocationInfo();

        public FlightPerfs flightPerfs;
        public readonly List<Mission> missions;
        public readonly List<Avion> avions;
        public readonly List<Aeroport> aeroports;

        //private readonly string BASERURL = "https://script.google.com/macros/s/AKfycbyhK_pky8u_OUPNwtQJtpRSiWwE0gF64zHHkbbDJuY6I9-3jjegoiIIfJfV0QKcVC2IAg/exec";
        private string BASERURL;

        private bool _isConnected;
        public bool isConnected { get {
                return (_isConnected);
            }
            //set {
            //    _isConnected = value;
            //}
        }


        public simData(string GSheetURL)
        {
            _isConnected = false;

            BASERURL = GSheetURL;

            aeroports = new List<Aeroport>();
            avions = new List<Avion>();
            missions = new List<Mission>();

            //flightPerfs = new FlightPerfs();

        }

        public async Task<int> loadDataFromSheet()
        {
            int result = 0;
            await LoadAirportsFromSheet();
            await LoadDataFromSheet();
            return result;
        }



        //get the fleet, and missions from the google sheet
        private async Task<int> LoadAirportsFromSheet()
        {
            int result = 0;

            //load the airports.
            this.aeroports.AddRange(await Aeroport.fetchAirports(BASERURL, DateTime.MinValue));
            //just in case, reload the statc values
            Logger.WriteLine("done loading airports database");
            //this.Cursor = Cursors.Default;
            return result;
        }

        private async Task<int> LoadDataFromSheet()
        {
            int result = 0;
            string url = BASERURL + "?query=fleet";
            UrlDeserializer dataReader = new UrlDeserializer(url);

            (List<Avion> avions, List<Mission> missions) = await dataReader.FetchDataAsync();
            this.avions.Clear();
            this.missions.Clear();
            this.avions.AddRange(avions);
            this.missions.AddRange(missions);

            if (avions.Count == 0)
            {
                Logger.WriteLine("planes database is empty !");
            }
            if (missions.Count == 0)
            {
                Logger.WriteLine("No mission available !");
            }

            Logger.WriteLine("done loading planes and missions");
            return result;

            //this.Cursor = Cursors.Default;
        }


        public async Task<int> UpdatePlaneStatus(int isFlying, UrlDeserializer.PlaneUpdateQuery planedata)
        {
                //crée un dictionnaire des valeurs à envoyer
                UrlDeserializer urlDeserializer = new UrlDeserializer(BASERURL);
                int result = await urlDeserializer.PushJSonAsync<UrlDeserializer.PlaneUpdateQuery>(planedata);

            return result;
        }

        public async Task<int> saveFlight(UrlDeserializer.SaveFlightQuery flightdata)
        {
            UrlDeserializer urlDeserializer = new UrlDeserializer(BASERURL);

            //sauve le vol dans le fichier "lastflight.json"
            await urlDeserializer.SaveLocalJsonAsync(flightdata, "lastflight.json");
            //envoie le vol vers le serveur
            int result = await urlDeserializer.PushJSonAsync<UrlDeserializer.SaveFlightQuery>(flightdata);
            //int result = await urlDeserializer.PushFlightAsync(data);

            return result;
        }


        public async Task<float> GetFretOnAirport(string airportIdent)
        {
            string url = BASERURL + "?query=freight&airport=" + airportIdent;
            float fret = await Aeroport.fetchFreight(BASERURL, airportIdent);
            return fret;
        }


        public void OpenConnection()
        {
            FSUIPCConnection.Open();
            _isConnected = true;
            payloadServices = FSUIPCConnection.PayloadServices;
            FSUIPCConnection.Process();
            payloadServices.RefreshData();
        }

        public void CloseConnection()
        {
            FSUIPCConnection.Close();
        }

        public void Refresh()
        {
            try
            {
                FSUIPCConnection.Process();
                payloadServices?.RefreshData();
            }catch(Exception)
            {
                _isConnected=false;
                throw;
            }
        }

        public bool GetReadyToFly()
        {
            return ((readyToFLy.Value == 0)&&(inMenu.Value == 0)&&(viewMode.Value < 4));
        }

        public double GetFuelWeight()
        {
            double result = 0;
            if (null != payloadServices)
            {
                result = Math.Floor(payloadServices.FuelWeightKgs);
            }
            return result;
        }

        public double GetPlaneWeight()
        {
            double result = 0;
            if (null != payloadServices)
            {
                result = payloadServices.GrossWeightKgs;
            }
            return result;
        }

        public void SetFuelWheight(double newFuelWeight)
        {
            if (null != payloadServices)
            {
                payloadServices.LoadFuelKgs(newFuelWeight, true);
                payloadServices.WriteChanges();
            }
        }

        public double GetMaxFuel()
        {
            if (null != payloadServices)
            {
                return payloadServices.FuelCapacityKgs;
            }
            else
            {
                return 0;
            }
        }

        public short GetOnground() => onGround.Value;

        public Double GetCargoWeight()
        {
            double result = 0;
            if (null != payloadServices)
            {
                result = payloadServices.PayloadWeightKgs;
            }
            return result;
        }

        public double GetMagVariation() => ((double)this.magVariation.Value) * 360 / 65536;

        public double GetAirSpeed() => (double)this.airspeed.Value / 128d;

        public double GetVerticalSpeed() => ((double)verticalSpeed.Value/256 ) * 60 * 3.28084;
        
        public double GetVerticalAcceleration() => ((double)verticalAcceleration.Value / 625);

        public double GetLandingVerticalSpeed() => ((double)landingVerticalSpeed.Value/256) * 60 * 3.28084;

        public byte GetStallWarning() => stallWarning.Value;

        public byte GetOverspeedWarning() => overSpeedWarning.Value;

        public short GetCrashedFlag() => crashed.Value;

        public short GetOffRunwayCrashed() => offRunwayCrashed.Value;

        public uint GetGearRetractableFlag() => gearRetractableFlag.Value;

        public uint GetAvionicsMaster() => avionicsMaster.Value;

        public uint GetBatteryMaster()=> batteryMaster.Value;

        public byte GetViewMode() => viewMode.Value;

        public bool GetIsGearUp() =>
            // gear down = 16383
            gearPosition.Value == 0;

        public uint GetFlapsAvailableFlag() => flapsAvailable.Value;

        public uint GetFlapsPosition() => flapsPosition.Value;

        public bool IsAtLeastOneEngineFiring()
        {
            bool result = false;
            switch (this.engineNumber.Value)
            {
                case (1):
                    {
                        result = (engine1Firing.Value == 1);
                    }; break;
                case (2):
                    {
                        result = (engine1Firing.Value == 1) ||
                                                 (engine2Firing.Value == 1);
                    }; break;
                case (3):
                    {
                        result = (engine1Firing.Value == 1) ||
                                                 (engine2Firing.Value == 1) ||
                                                 (engine3Firing.Value == 1);
                    }; break;
                case (4):
                    {
                        result = (engine1Firing.Value == 1) ||
                                                 (engine2Firing.Value == 1) ||
                                                 (engine3Firing.Value == 1) ||
                                                 (engine4Firing.Value == 1);
                    }; break;
            }
            return result;
        }

        public PositionSnapshot GetPosition()
        {
            FsPositionSnapshot pt = FSUIPCConnection.GetPositionSnapshot();
            PositionSnapshot position = new PositionSnapshot();
            position.Altitude = pt.Altitude.Feet;
            position.Location.Latitude = pt.Location.Latitude.DecimalDegrees;
            position.Location.Longitude = pt.Location.Longitude.DecimalDegrees;
            position.BankDegrees = pt.BankDegrees;
            position.IsOnGround = pt.IsOnGround;
            position.HeadingDegreesTrue = pt.HeadingDegreesTrue;
            position.IndicatedAirspeedKnots = pt.IndicatedAirspeedKnots;
            position.PitchDegrees = pt.PitchDegrees;
            return position;


        }

        public string GetAircraftType() => aircraftType.Value;
        public string GetAircraftModel() => aircraftModel.Value;

        public string GetFlightNumber() => flightNumber.Value;


        public string GetTailNumber() => tailNumber.Value;

        public double GetPayload()
        {
            if (payloadServices != null)
            {
                double adujstedPayload = payloadServices.PayloadWeightKgs-80;
                if (adujstedPayload < 0)
                {
                    adujstedPayload = 0;
                }
                //remove the pilot weight from this payload
                return adujstedPayload;
            }
            else
            {
                return 0;
            }
        }


    }
}
