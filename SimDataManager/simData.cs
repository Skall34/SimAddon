using FSUIPC;
using SimAddonLogger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public PositionSnapshot(PositionSnapshot position)
        {
            Location = new LatLonPoint()
            {
                Latitude = position.Location.Latitude,
                Longitude = position.Location.Longitude
            };
            Altitude = position.Altitude;
            HeadingDegreesTrue = position.HeadingDegreesTrue;
            PitchDegrees = position.PitchDegrees;
            BankDegrees = position.BankDegrees;
            IndicatedAirspeedKnots = position.IndicatedAirspeedKnots;
            IsOnGround = position.IsOnGround;
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
        private readonly Offset<short> pauseIndicator = new Offset<short>(0x0264);

        private readonly Offset<short> magVariation    =  new Offset<short>(0x02A0);
        private readonly Offset<uint> airspeed         =   new Offset<uint>(0x02BC);
        private readonly Offset<int> verticalSpeed     =    new Offset<int>(0x02C8);

        private readonly Offset<int> landingVerticalSpeed = new Offset<int>(0x030C);

        private readonly Offset<short> COM1 = new Offset<short>(0x034E);
        private readonly Offset<short> COM1Stdby = new Offset<short>(0x311A);
        private Offset<byte> COM1Switch = new Offset<byte>(0x3123);

        private readonly Offset<short> squawk = new Offset<short>(0x0354);
        private readonly Offset<byte> squawkMode = new Offset<byte>(0x6D8C);

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

        //fuel flows pounds / hour
        private readonly Offset<double> engine1FuelFlow = new Offset<double>(0x0918);
        private readonly Offset<double> engine2FuelFlow = new Offset<double>(0x09B0);
        private readonly Offset<double> engine3FuelFlow = new Offset<double>(0x0A48);
        private readonly Offset<double> engine4FuelFlow = new Offset<double>(0x0AE0);

        //manifold pressure in inches of mercury
        private readonly Offset<ushort> engine1ManifoldPressure = new Offset<ushort>(0x08C0);
        //RPM
        private readonly Offset<short> engine1RPM = new Offset<short>(0x0898);
        private readonly Offset<short> engine1RPMScaler = new Offset<short>(0x08C8);

        private readonly Offset<short> engineNumber = new Offset<short>(0x0AEC);

        private readonly Offset<uint> flapsPosition      = new Offset<uint>(0x0BE0);
        private readonly Offset<uint> gearPosition       = new Offset<uint>(0x0BF0);

        private readonly Offset<double> verticalAcceleration = new Offset<double>(0x3068);

        private readonly Offset<uint> avionicsMaster = new Offset<uint>(0x2E80);
        private readonly Offset<uint> batteryMaster  = new Offset<uint>(0x281C);

        private readonly Offset<string> flightNumber = new Offset<string>(0x3130, 12);
        private readonly Offset<string> tailNumber   = new Offset<string>(0x313C, 12);

        private readonly Offset<byte> readyToFLy = new Offset<byte>(0x3364);
        private readonly Offset<byte> inMenu =     new Offset<byte>(0x3365);

        private readonly Offset<string> aircraftModel = new Offset<string>(0x3500, 24);
        private readonly Offset<string> aircraftType =  new Offset<string>(0x3D00, 256);

        private readonly Offset<byte> viewMode = new Offset<byte>(0x8320);


        private PayloadServices payloadServices;
        //private readonly PlayerLocationInfo locationInfos = new PlayerLocationInfo();

        public FlightPerfs flightPerfs;
        public readonly List<Mission> missions;
        public readonly List<Avion> avions;
        public readonly List<Aeroport> aeroports;

        //private readonly string BASERURL = "https://script.google.com/macros/s/AKfycbyhK_pky8u_OUPNwtQJtpRSiWwE0gF64zHHkbbDJuY6I9-3jjegoiIIfJfV0QKcVC2IAg/exec";
        private string BASERURL;

        private bool _isConnectedToSim;

        private SiteConnection SiteConnection;
        private string sessionToken;

        public bool isConnectedToSim { get {
                return (_isConnectedToSim);
            }
            //set {
            //    _isConnectedToSim = value;
            //}
        }

        private static HttpClient httpClient;

        private FlyingNetwork _flyingNetwork;

        public FlyingNetwork flyingNetwork {
            get { return _flyingNetwork; }
        }

        public simData(string GSheetURL)
        {
            _isConnectedToSim = false;

            BASERURL = GSheetURL;

            aeroports = new List<Aeroport>();
            avions = new List<Avion>();
            missions = new List<Mission>();

            //flightPerfs = new FlightPerfs();
            SiteConnection = new SiteConnection(BASERURL);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.DefaultConnectionLimit = 10;
            ServicePointManager.MaxServicePointIdleTime = 15000; //15s
            httpClient = new HttpClient();
        }

        public void useFlyingNetwork(string networkName)
        {
            if (networkName.Equals(FlyingNetwork.VATSIM, StringComparison.OrdinalIgnoreCase))
            {
                _flyingNetwork = new VATSIMFlyingNetwork();
            }
            else if (networkName.Equals(FlyingNetwork.IVAO, StringComparison.OrdinalIgnoreCase))
            {
                _flyingNetwork = new IVAOFlyingNetwork();
            }
            else
            {
                if (networkName.Equals(string.Empty))
                {
                    _flyingNetwork = null;
                    return;
                }
                throw new ArgumentException("Invalid network name");
            }
        }

        public async Task<string> loginToSite()
        {
            sessionToken = await SiteConnection.Login();
            Logger.WriteLine("Login result token: " + sessionToken);
            return sessionToken;
        }

        public void logoutFromSite()
        {
            SiteConnection.Logout();
            sessionToken = string.Empty;
        }

        public async Task<bool> checkSession(string token)
        {
            bool result = false;
            result = await SiteConnection.CheckSession(token);
            if (result) {
                //update the current token if session is valid
                sessionToken = token;
            }
            return result;
        }

        public bool isConnectedToSite()
        {
            return SiteConnection.Status == SiteConnection.ConnectionStatus.Connected;
        }

        public async Task<int> loadDataFromSheet()
        {
            // Lancer les tâches simultanément
            var tAirports = LoadAirportsFromSheet();
            var tData = LoadDataFromSheet();

            // Attendre les deux
            await Task.WhenAll(tAirports, tData).ConfigureAwait(false);

            // Optionnel : récupérer les résultats si besoin
            int resAirports = await tAirports;
            int resData = await tData;

            return resAirports + resData;
        }

        //get the fleet, and missions from the google sheet
        private async Task<int> LoadAirportsFromSheet()
        {
            int result = 0;

            //load the airports.
            this.aeroports.AddRange(await Aeroport.fetchAirports(BASERURL, DateTime.MinValue,sessionToken));
            //just in case, reload the statc values
            Logger.WriteLine("done loading airports database");
            //this.Cursor = Cursors.Default;
            return result;
        }

        public Reservation CheckReservation(string callsign)
        {
            Task<Reservation> result = ReservationMgr.CheckReservation(callsign, BASERURL,sessionToken);
            return result.Result;
        }

        public void ApplyReservation(string callsign, Reservation reservation)
        {
             ReservationMgr.ApplyReservation(callsign, reservation, BASERURL,sessionToken);
        }

        public void CompleteReservation(string callsign, Reservation reservation)
        {
            ReservationMgr.CompleteReservation(callsign, reservation, BASERURL,sessionToken);
            //mark the plane as not reserved 
            Avion avion = avions.Find(a => a.Immat == reservation.Immat);
            if (avion != null)
            {
                avion.Reserved = 0;
                avion.Status = Avion.PlaneStatus.Disponible;
            }
        }

        private async Task<int> LoadDataFromSheet()
        {
            int result = 0;
            List<Mission> missions = await Mission.FetchMissionsFromSheet(httpClient, BASERURL,sessionToken);           
            List<Avion> avions = await Avion.FetchAvionsFromSheet(httpClient, BASERURL,sessionToken);

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

        private static bool updateInProgress = false;
        public async void UpdatePlaneFromSheet()
        {
            if (updateInProgress)
            {
                Logger.WriteLine("UpdatePlaneFromSheet already in progress, skipping this call.");
                return;
            }
            updateInProgress = true;
            if (avions.Count == 0)
            {
                Logger.WriteLine("planes database is empty, cannot update planes status !");
                return;
            }
            Avion.UpdateAvionsStatus(httpClient, avions, BASERURL,sessionToken);
            updateInProgress = false;
        }

        public async Task<bool> UpdatePlaneStatus(int isFlying, Dictionary<string, string> planedata)
        {
            if (updateInProgress) {
                Logger.WriteLine("UpdatePlaneStatus already in progress, skipping this call.");
                return false;
            }
            updateInProgress = true;
            //crée un dictionnaire des valeurs à envoyer
            bool result = false;
            string phpUrl = BASERURL + "/api/api_update_status.php";
            Dictionary<string, string> dataToSend = new Dictionary<string, string>(planedata);
            dataToSend["session_token"] = sessionToken;

            var content = new FormUrlEncodedContent(dataToSend);

            try
            {
                var response = await httpClient.PostAsync(phpUrl, content);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Logger.WriteLine("Erreur lors de l'envoi des données de vol : " + response.StatusCode);
                    // Affiche tout, même si c'est vide
                    string message = $"api_update_status return code: {response.StatusCode}\nRéponse brute:\n{responseContent}";
                    Logger.WriteLine(message);
                }
                else
                {
                    Logger.WriteLine("UpdatePlaneStatus OK");
                }

                if (!response.IsSuccessStatusCode)
                {
                    // Affiche le détail dans le popup
                    result = false;
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Exception lors de l'envoi des données de vol : " + ex.Message);
                if (ex.InnerException != null)
                {
                    Logger.WriteLine("Détail de l'erreur : " + ex.InnerException.Message);
                }
                result = false;
            }
            updateInProgress = false;
            return result;
        }

        private class FlightDataResponse
        {
            public string status{ get; set; }
            public string message { get; set; }
        }

        public async Task<(bool,string)> SendFlightDataToPhpAsync(Dictionary<string, string> flightData)
        {
            
            string phpUrl = BASERURL + "/api/api_import_vol_direct.php";

            Dictionary<string, string> dataToSend = new Dictionary<string, string>(flightData);
            dataToSend["session_token"] = sessionToken;

            var content = new FormUrlEncodedContent(dataToSend);

            try
            {
                var response = await httpClient.PostAsync(phpUrl, content);
                string responseContent = await response.Content.ReadAsStringAsync();

                // Affiche tout, même si c'est vide
                string message = $"Code: {response.StatusCode}\nRéponse brute:{responseContent}";
                Logger.WriteLine(message);

                //deserialize the response to get the message
                FlightDataResponse responseData = System.Text.Json.JsonSerializer.Deserialize<FlightDataResponse>(responseContent);

                if (!response.IsSuccessStatusCode)
                {

                    // Affiche le détail dans le popup
                    return (false,responseData.message);
                }
                Logger.WriteLine("Réponse du serveur : " + responseData.message);

                return (true, responseData.message);
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Exception lors de l'envoi des données de vol : " + ex.Message);
                if (ex.InnerException != null)
                {
                    Logger.WriteLine("Détail de l'erreur : " + ex.InnerException.Message);
                }
                return (false,ex.Message);
            }
        }


        public async Task<float> GetFretOnAirport(string airportIdent)
        {
            string url = BASERURL + "/api/api_getFretByIcao.php?ICAO=" + airportIdent;
            url+= "&session_token=" + sessionToken;
            UrlDeserializer dataReader = new UrlDeserializer(httpClient,url);
            float fret = await dataReader.FetchFreightDataAsync();
            return fret;
        }


        public void OpenConnection()
        {
            FSUIPCConnection.Open();
            _isConnectedToSim = true;
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
                _isConnectedToSim=false;
                throw;
            }
        }

        public string GetStartSituation()
        {
            return startSituation.Value;
        }

        public bool IsPaused()
        {
            return (pauseIndicator.Value != 0);
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

        public float GetCOM1()
        {
            short bcdValue = COM1.Value;
            int thousands = (bcdValue >> 12) & 0xF;
            int hundreds = (bcdValue >> 8) & 0xF;
            int tens = (bcdValue >> 4) & 0xF;
            int ones = bcdValue & 0xF;

            int integerValue = thousands * 1000 + hundreds * 100 + tens * 10 + ones;

            // Si on sait que la virgule est après deux chiffres
            return 100 + integerValue / 100.0f;
        }

        public void SetCOM1(float newValue)
        {
            // Convertir la valeur en BCD
            int integerValue = (int)((newValue-100) * 100);
            short bcdValue = (short)((((integerValue / 1000) % 10) << 12) |
                                     (((integerValue / 100) % 10) << 8) |
                                     (((integerValue / 10) % 10) << 4) |
                                     (integerValue % 10));
            COM1.Value = bcdValue;
        }

        public float GetCOM1Stdby()
        {
            short bcdValue = COM1Stdby.Value;
            int thousands = (bcdValue >> 12) & 0xF;
            int hundreds = (bcdValue >> 8) & 0xF;
            int tens = (bcdValue >> 4) & 0xF;
            int ones = bcdValue & 0xF;

            int integerValue = thousands * 1000 + hundreds * 100 + tens * 10 + ones;

            // Si on sait que la virgule est après deux chiffres
            return 100 + integerValue / 100.0f;
        }

        public void SetCOM1Stdby(float newValue)
        {
            // Convertir la valeur en BCD
            int integerValue = (int)((newValue - 100) * 100);
            short bcdValue = (short)((((integerValue / 1000) % 10) << 12) |
                                     (((integerValue / 100) % 10) << 8) |
                                     (((integerValue / 10) % 10) << 4) |
                                     (integerValue % 10));
            COM1Stdby.Value = bcdValue;
        }

        public void SwitchCOM1() {
            // Toggle the COM1 switch value
            //COM1Switch.Value = (byte)8; //2^3 = COM1 swap
            FSUIPCConnection.SendControlToFS(FsControl.COM_STBY_RADIO_SWAP, 0);
        }


        public int GetSquawk()
        {
            short bcdValue = squawk.Value;
            int thousands = (bcdValue >> 12) & 0xF;
            int hundreds = (bcdValue >> 8) & 0xF;
            int tens = (bcdValue >> 4) & 0xF;
            int ones = bcdValue & 0xF;
            int integerValue = thousands * 1000 + hundreds * 100 + tens * 10 + ones;
            return integerValue;
        }

        public void SetSquawk(int newValue)
        {
            // Convertir la valeur en BCD
            short bcdValue = (short)((((newValue / 1000) % 10) << 12) |
                                     (((newValue / 100) % 10) << 8) |
                                     (((newValue / 10) % 10) << 4) |
                                     (newValue % 10));
            squawk.Value = bcdValue;
        }

        //transponder mode off=0,stdby=1,on=2,test=3
        public byte GetSquawkMode() => squawkMode.Value;
        public void SetSquawkMode(byte value) {
            squawkMode.Value = value;
        }


        public double GetMagVariation() => ((double)this.magVariation.Value) * 360 / 65536;

        public double GetAirSpeed() => (double)this.airspeed.Value / 128d;

        public double GetVerticalSpeed() => ((double)verticalSpeed.Value/256 ) * 60 * 3.28084;
        
        public double GetVerticalAcceleration() => verticalAcceleration.Value;

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

        public double GetAverageFuelFlow()
        {
            double totalFuelFlow = 0;
            int engineCount = this.engineNumber.Value;
            switch (engineCount)
            {
                case (1):
                    {
                        totalFuelFlow = engine1FuelFlow.Value;
                    }; break;
                case (2):
                    {
                        totalFuelFlow = engine1FuelFlow.Value + engine2FuelFlow.Value;
                    }; break;
                case (3):
                    {
                        totalFuelFlow = engine1FuelFlow.Value + engine2FuelFlow.Value + engine3FuelFlow.Value;
                    }; break;
                case (4):
                    {
                        totalFuelFlow = engine1FuelFlow.Value + engine2FuelFlow.Value + engine3FuelFlow.Value + engine4FuelFlow.Value;
                    }; break;
            }
            if (engineCount > 0)
            {
                return totalFuelFlow / engineCount;
            }
            else
            {
                return 0;
            }
        }

        public int GetEngine1RPM()
        {
            int rpmScaler = engine1RPMScaler.Value;
            if (rpmScaler == 0)
            {
                rpmScaler = 1;
            }
            return (engine1RPM.Value * rpmScaler) / 65536;
        }

        public int GetEngine1ManifoldPressure()
        {
            return (int)(engine1ManifoldPressure.Value / 1024);
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
