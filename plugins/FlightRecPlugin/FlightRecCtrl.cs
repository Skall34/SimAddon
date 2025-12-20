using FlightRecPlugin.Properties;
using SimAddonPlugin;
using SimDataManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SimAddonLogger;
using System.Reflection;
using System.Windows.Forms.VisualStyles;
using System.IO;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Text;
using FSUIPC;
using System.Net.Http;
using System.Globalization;
using Newtonsoft.Json;
using static SimAddonPlugin.ISimAddonPluginCtrl;

namespace FlightRecPlugin
{
    public partial class FlightRecCtrl : UserControl, ISimAddonPluginCtrl
    {
        const string name = "FlightRecorder";
        Version? version;

        DebugForm dbg;
        bool simReady;
        bool isPaused;
        private TimeSpan pauseTime;
        private DateTime pauseStartTime;
        private DateTime pauseEndTime;
        private bool isRecording;

        private bool atLeastOneEngineFiring;
        private bool stopEngineConfirmed;
        private int startDisabled; // if startDisabled==0, then start is possible, if not, start is disabled. each 100ms, the counter will be decremented
        private int endDisabled;

        PositionSnapshot _currentPosition;
        PositionSnapshot _startPosition;
        PositionSnapshot _endPosition;

        private Aeroport localAirport;
        private Avion currentPlane;

        private double _startFuel;
        private double _endFuel;
        private double _endPayload;
        private short _note;

        private DateTime _startTime;
        private DateTime _endTime;

        private DateTime _airborn;
        private DateTime _notAirborn;

        public bool onGround;
        private bool gearIsUp;
        private uint flapsPosition;
        private bool _planeReserved;
        private double _currentFuel;
        private bool _refuelDetected;
        private bool _noEngineFlight;

        LocalFlightBook localFlightBook;
        private GPSRecorder GPSRecorder;
        private FlightParamsRecorder flightParamsRecorder;

        private readonly FlightPerfs flightPerfs;

        private ReservationMgr.ReservationStatus reservationStatus = ReservationMgr.ReservationStatus.Unknown;
        private Reservation reservation;
        private static bool checkingReservation = false;

        private bool staticValuesReadOnce = false;
        private simData data;

        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;
        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;
        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;
        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;
        public event ISimAddonPluginCtrl.OnShowDialogHandler OnShowDialog;

        private string executionFolder;

        private enum STATE
        {
            WAITING,
            TAXIING,
            INFLIGHT,
            GLIDING,
            ENDED
        };

        private STATE currentState = STATE.WAITING;

        private enum EVENT
        {
            NONE,
            ENGINESTART,
            TAKEOFF,
            LANDING,
            ENGINESTOP,
            CRASHING
        };


        private void UpdateStatus(string message)
        {
            if (OnStatusUpdate != null)
            {
                OnStatusUpdate(this, message);
            }
        }
        private void SimEvent(SimEventArg eventArg)
        {
            if (OnSimEvent != null)
            {
                OnSimEvent(this, eventArg);
            }
        }

        private void SimEvent(SimEventArg.EventType eventType)
        {
            if (OnSimEvent != null)
            {
                SimEventArg eventArg = new SimEventArg();
                eventArg.reason = eventType;
                OnSimEvent(this, eventArg);
            }
        }

        public DialogResult ShowMsgBox(string text, string caption, MessageBoxButtons buttons)
        {
            if (OnShowMsgbox != null)
            {
                return OnShowMsgbox(this, text, caption, buttons);
            }
            else
            {
                //if the caller did not managed this, consider a cancel. (could have been ignore also ?)
                return DialogResult.Cancel;
            }
        }


        public FlightRecCtrl()
        {
            InitializeComponent();

            Logger.WriteLine("Starting...");
            Assembly? assembly = Assembly.GetEntryAssembly();
            version = assembly.GetName().Version;
            //utilisation de la nouvelle classe de combobox item pour mettre des elements non selectionables
            this.cbImmat.ValueMember = "Immat";
            this.cbImmat.DisplayMember = "Immat";

            simReady = false;

            //get the google sheet API url from the app settings
            //initialise des variables qui servent à garder un état.
            cbNote.SelectedItem = 8;
            atLeastOneEngineFiring = false;
            stopEngineConfirmed = false;

            _startTime = DateTime.UnixEpoch;
            _endTime = DateTime.UnixEpoch;
            _airborn = DateTime.UnixEpoch;
            _notAirborn = DateTime.UnixEpoch;

            _startFuel = 0;
            _endFuel = 0;
            _currentFuel = 0;
            _refuelDetected = false;
            _endPayload = 0;
            _noEngineFlight = false;

            pauseTime = TimeSpan.Zero;
            isPaused = false;
            isRecording = false;

            lbStartFuel.Text = "Not Yet Available";
            lbEndFuel.Text = "Waiting end flight ...";
            lbStartIata.Text = "Not Yet Available";
            lbEndIata.Text = "Waiting end flight ...";
            lbStartPosition.Text = "Not Yet Available";
            lbEndPosition.Text = "Waiting end flight ...";
            lbStartTime.Text = "--:--";
            lbEndTime.Text = "--:--";
            lbPayload.Text = "Not Yet Available";
            lbTimeAirborn.Text = "--:--";
            lbTimeOnGround.Text = "--:--";
            lbLibelleAvion.Text = "Not Yet Available";

            onGround = true;
            gearIsUp = false;
            flapsPosition = 0;

            //recupere le callsign qui a été sauvegardé en settings de l'application
            this.tbCallsign.Text = Settings.Default.callsign;
            //desactive le bouton de maj du setting. Il sera reactivé si le callsign est modifié.
            btnSaveSettings.Enabled = false;

            localFlightBook = new LocalFlightBook();
            flightPerfs = new FlightPerfs();
            GPSRecorder = new GPSRecorder();
            flightParamsRecorder = new FlightParamsRecorder();
        }


        public void SetWindowMode(ISimAddonPluginCtrl.WindowMode mode)
        {
            if (mode == ISimAddonPluginCtrl.WindowMode.COMPACT)
            {
                splitContainer1.Panel1Collapsed = true;
            }
            else
            {
                splitContainer1.Panel1Collapsed = false;
            }
        }

        public string getName()
        {
            return name;
        }

        public void init(ref simData _data)
        {
            data = _data;

            RemplirComboImmat();
            RemplirComboMissions();

            //send the callsign event
            SimEventArg eventArg = new SimEventArg();
            eventArg.reason = SimEventArg.EventType.SETCALLSIGN;
            eventArg.value = tbCallsign.Text;
            SimEvent(eventArg);

            timerUpdateStaticValues.Start();
            timerUpdateFleetStatus.Start();

            this.Enabled = true;

            UpdateStatus("FlightRecorder is ready");
            Logger.WriteLine("FlightRecorder is ready");
        }

        public async void FormClosing(object sender, FormClosingEventArgs e)
        {
            //need to ask for save before close
            string message = "Confirm close ACARS ?";
            DialogResult res = DialogResult.OK;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                res = ShowMsgBox(message, "Closing", MessageBoxButtons.OKCancel);
            }
            if (res == DialogResult.OK)
            {
                if (atLeastOneEngineFiring)
                {
                    this.Cursor = Cursors.WaitCursor;
                    // Libère l'avion sur le fichier en cas de fermeture de l'acars avant la fin du vol
                    // on ne le fait que si un moteur tourne encore ==> vol interrompu avant la fin

                    //stop the timer
                    updatePlaneStatusTimer.Stop();

                    UpdatePlaneStatus(0);

                    System.Threading.Thread.Sleep(2000);
                    this.Cursor = Cursors.Default;
                }

                //if the reservation is ongoing, we need to free the reservation
                if (reservationStatus == ReservationMgr.ReservationStatus.Accepted)
                {
                    data.CompleteReservation(Settings.Default.callsign, reservation);
                }

            }
            else
            {
                // Si l'utilisateur clique sur Annuler, annule la fermeture de la fenêtre.
                Logger.WriteLine("close canceled by user");
                e.Cancel = true;
            }
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

        private bool detectEngineStart(situation currentFlightStatus)
        {
            bool result = false;
            //on va verifier l'etat des moteurs :
            //sauvegarde l'etat precedent des moteurs
            bool _previousEngineStatus = atLeastOneEngineFiring;

            //si aucun moteur de tournait, mais que maintenant, au moins un moteur tourne, on commence a enregistrer.
            //on va memoriser les etats de carburant, et l'heure. On récupere aussi quel est l'aeroport.

            //test pour savoir si on est vraiment pret à voler. On le sim doit etre pret, et le pilote dans le cockpit
            //(viewmode à moins de 4 sur msfs (pas de pb de viewmode avec xplane apparement)
            //c'est pour eviter d'etre detecté à DGTK si on demarre directement sur la piste moteurs allumés.

            if ((!_previousEngineStatus && currentFlightStatus.isAtLeastOneEngineFiring) && (startDisabled == 0))
            {
                //garde le nouvel etat.
                atLeastOneEngineFiring = currentFlightStatus.isAtLeastOneEngineFiring;
                stopEngineConfirmed = false;
                if (engineStopTimer.Enabled)
                {
                    Logger.WriteLine("Engine stop canceled. Validation timer stopped");
                    engineStopTimer.Stop();
                }
                //on a detecté un demarrage moteur
                result = true;
                Logger.WriteLine("Engine start detected. onGround=" + onGround.ToString() + " startDisabled=" + startDisabled.ToString());
            }
            return result;
        }

        private bool detectEngineStop(situation currentFlightStatus)
        {
            bool result = false;
            //on va verifier l'etat des moteurs :
            //sauvegarde l'etat precedent des moteurs
            bool _previousEngineStatus = atLeastOneEngineFiring;
            //si un moteur tournait, mais que maintenant, plus aucun moteur ne tourne, on arrete d'enregistrer.
            if (_previousEngineStatus && !currentFlightStatus.isAtLeastOneEngineFiring)
            {
                //garde le nouvel etat.
                atLeastOneEngineFiring = currentFlightStatus.isAtLeastOneEngineFiring;
                if (endDisabled == 0)
                {
                    Logger.WriteLine("Potential engine stop detected. Start validation timer");
                    engineStopTimer.Start();
                }
            }

            if (stopEngineConfirmed)
            {
                //the engine stop has been confirmed by the timer.
                result = true;
                //reset the flag to detect the engine stop again later.
                stopEngineConfirmed = false;
            }
            return result;
        }

        private void measureTakeoffPerfs(situation currentFlightStatus)
        {
            //we just took off ! read the plane weight
            flightPerfs.takeOffWeight = currentFlightStatus.planeWeight;
            // on veut afficher la date
            _airborn = data.GetSimDateTimeUTC();  //DateTime.Now;
            flightPerfs.takeOffTime = _airborn;
            if (lbTimeAirborn.Text == "--:--")
            {
                this.lbTimeAirborn.Text = _airborn.ToString("HH:mm");
            }

            // On cache le label du Fret après le décollage. On en a plus besoin
            this.lbFret.Visible = false;
            //on grise le bouton save flight en vol
            btnSubmit.Enabled = false;
            submitFlightToolStripMenuItem.Enabled = false;

            //just incase of rebound during takeoff, reset the onground label
            lbTimeOnGround.Text = "--:--";

        }

        private bool detectTakeoff(situation currentFlightStatus)
        {
            bool result = false;
            //check if we are in the air
            if (currentFlightStatus.onGround == 0)
            {
                //yes, we are airborn.
                //check if we were already airborn
                if (onGround)
                {
                    //we were on ground, and now we are airborn.
                    //filter to only consider the first takeoff
                    if (_airborn == DateTime.UnixEpoch)
                    {
                        //only take the first takeoff for takeoff time. To manage rebounds when landing.
                        Logger.WriteLine("Takeoff detected !");
                        UpdateStatus("In flight");
                        //keep memory that we're airborn
                        onGround = false;
                        result = true;
                    }
                }
            }
            return result;
        }

        private void measureLandingPerfs(situation currentFlightStatus)
        {
            //only update the touchDownVSpeed if we've been airborn once
            flightPerfs.landingSpeed = currentFlightStatus.airSpeed;
            flightPerfs.landingVSpeed = currentFlightStatus.verticalSpeed;
            flightPerfs.landingWeight = currentFlightStatus.planeWeight;

            _notAirborn = data.GetSimDateTimeUTC();  //DateTime.Now;
            flightPerfs.landingTime = _notAirborn;

            if (lbTimeOnGround.Text == "--:--")
            {
                this.lbTimeOnGround.Text = _notAirborn.ToString("HH:mm");
            }
        }

        private bool detectLanding(situation currentFlightStatus)
        {
            bool result = false;
            double currentFuel = currentFlightStatus.currentFuel;
            //we are on ground, check if we were airborn before
            if (currentFlightStatus.onGround == 1)
            {
                if (!onGround)
                {
                    //we were airborn, and now we are on ground.
                    DateTime now = data.GetSimDateTimeUTC();  //DateTime.Now;
                    if (now - _airborn >= TimeSpan.FromSeconds(30))
                    {
                        Logger.WriteLine("Landing detected !");
                        UpdateStatus("On ground");
                        result = true;

                        onGround = true;

                        //enable the save button
                        btnSubmit.Enabled = true;
                        submitFlightToolStripMenuItem.Enabled = true;
                        SimEvent(SimEventArg.EventType.LANDING);
                    }
                    else
                    {
                        Logger.WriteLine("take off rebound detected. Just ignore");
                    }
                }

                //si on est au sol, et qu'on a lu une valeur de fuel, ET le fuel augmente, on detecte un refuel !
                if (!_refuelDetected && (_currentFuel > 0) && (currentFuel > _currentFuel))
                {
                    _refuelDetected = true;
                    //on detecte un refuel !
                    //il faut peut-être faire un reset ?
                    Logger.WriteLine("Refuel detected ! new fuel " + currentFuel + " > " + _currentFuel + " (old fuel))");
                    //this.WindowState = FormWindowState.Normal;
                    _currentFuel = currentFuel;
                }
            }
            return result;
        }

        private bool detectCrash(situation currentFlightStatus)
        {
            bool result = false;
            //check for crashes
            if ((currentFlightStatus.offRunwayCrashed != 0) && (!flightPerfs.overRunwayCrashed))
            {
                Logger.WriteLine("off runway crashed detected");
                flightPerfs.overRunwayCrashed = true;
                result = true;
                //getEndOfFlightData();
            }
            if ((currentFlightStatus.crashedFlag != 0) && (!flightPerfs.crashed))
            {
                Logger.WriteLine("crash detected");
                flightPerfs.crashed = true;
                result = true;
                //getEndOfFlightData();
            }
            return result;
        }

        private void checkReservation()
        {

            if (!checkingReservation)
            {
                checkingReservation = true;
                // After initialization, check reservation for configured callsign
                try
                {
                    //only check reservation, if the planes and missions data are loaded
                    if (data.avions == null || data.avions.Count == 0 ||
                        data.missions == null || data.missions.Count == 0)
                    {
                        Logger.WriteLine("CheckReservation: planes or missions data not yet loaded");
                        checkingReservation = false;
                        return;
                    }

                    // call and wait so popup appears during startup
                    if (reservationStatus == ReservationMgr.ReservationStatus.Unknown)
                    {
                        reservation = data.CheckReservation(Settings.Default.callsign);
                        if (reservation.Reserved)
                        {
                            //there is a reservation for this callsign at this airport
                            if ((localAirport != null) && (localAirport.ident == reservation.DepartureIcao))
                            {
                                Logger.WriteLine("CheckReservation: local airport matches reservation departure");
                                //plane is reserved, ask if user wants to apply reservation data
                                Logger.WriteLine("CheckReservation: reservation found for callsign " + Settings.Default.callsign);
                                string message = "A reservation has been found for callsign " + Settings.Default.callsign + ":\n" +
                                    "Departure: " + reservation.DepartureIcao + "\n" +
                                    "Arrival: " + reservation.ArrivalIcao + "\n" +
                                    "Plane: " + reservation.Immat + "\n\n" +
                                    "Do you want to apply this reservation data to the flight ?";
                                DialogResult res = ShowMsgBox(message, "Reservation found", MessageBoxButtons.YesNo);
                                if (res == DialogResult.Yes)
                                {
                                    Logger.WriteLine("CheckReservation: user accepted to apply reservation data");
                                    //apply reservation data
                                    tbEndICAO.Text = reservation.ArrivalIcao;
                                    tbEndICAO.Enabled = false;
                                    cbImmat.SelectedItem = data.avions.Where(a => a.Immat == reservation.Immat).FirstOrDefault();
                                    cbMission.SelectedItem = "";
                                    reservationStatus = ReservationMgr.ReservationStatus.Accepted;

                                    //apply the reservation in the sim data manager
                                    ApplyReservation(reservation.Immat, reservation.DepartureIcao, reservation.ArrivalIcao);

                                    cbMission.Enabled = false;
                                }
                                else
                                {
                                    Logger.WriteLine("CheckReservation: user refused to apply reservation data");
                                    reservationStatus = ReservationMgr.ReservationStatus.Ignored;
                                }
                            }
                            else
                            {
                                Logger.WriteLine("CheckReservation: local airport does not match reservation departure");
                            }
                        }
                        else
                        {
                            //no reservation found
                            reservationStatus = ReservationMgr.ReservationStatus.Unknown;
                        }
                    }
                    else
                    {
                        //already checked just do nothing
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("CheckReservationAsync scheduling failed: " + ex.Message);
                }
                checkingReservation = false;
            }
            else
            {
                Logger.WriteLine("CheckReservation: already checking");
            }
        }

        public void updateSituation(situation currentFlightStatus)
        {

            try
            {
                if (isRecording)
                {
                    //check if the sim is paused
                    if (data.IsPaused() && !isPaused)
                    {
                        isPaused = true;
                        pauseStartTime = DateTime.Now;
                        Logger.WriteLine("Pause detected. Pause started at " + pauseStartTime.ToString("HH:mm:ss"));
                        UpdateStatus("Sim is paused. Pause started at " + pauseStartTime.ToString("HH:mm:ss"));
                    }
                    if (!data.IsPaused() && isPaused)
                    {
                        isPaused = false;
                        pauseEndTime = DateTime.Now;
                        pauseTime += (pauseEndTime - pauseStartTime);
                        Logger.WriteLine("Pause detected. Pause time is now " + Math.Round(pauseTime.TotalSeconds) + " seconds");
                        UpdateStatus("Sim is resumed. Pause time is now " + Math.Round(pauseTime.TotalSeconds) + " seconds");
                    }
                }

                if (currentFlightStatus.readyToFly)
                {
                    //one shot flag to avoid to take bad data until the pilot is in the plane.
                    simReady = true;
                }

                if (dbg != null && dbg.Visible)
                {
                    dbg.updateInfos(currentFlightStatus);
                }

                if (startDisabled > 0)
                {
                    startDisabled -= 1;

                    if (startDisabled <= 0)
                    {
                        //the start detection disable timer expired, restore the start textboxes.
                        gbStartInfos.Enabled = true;
                        Cursor = Cursors.Default;
                    }
                }

                if (endDisabled > 0)
                {
                    endDisabled -= 1;

                    if (endDisabled <= 0)
                    {
                        //the start detection disable timer expired, restore the start textboxes.
                        gbEndInfos.Enabled = true;
                    }
                }

                if (simReady)
                {
                    _currentPosition = currentFlightStatus.position;

                    // Airspeed
                    double airspeedKnots = currentFlightStatus.airSpeed;
                    double currentFuel = currentFlightStatus.currentFuel;

                    EVENT eventDetected = EVENT.NONE;
                    if (detectEngineStart(currentFlightStatus))
                    {
                        eventDetected = EVENT.ENGINESTART;
                    }
                    if (detectEngineStop(currentFlightStatus))
                    {
                        eventDetected = EVENT.ENGINESTOP;
                    }

                    if (detectTakeoff(currentFlightStatus))
                    {
                        eventDetected = EVENT.TAKEOFF;
                    }

                    if (detectLanding(currentFlightStatus))
                    {
                        eventDetected = EVENT.LANDING;
                    }

                    if (detectCrash(currentFlightStatus))
                    {
                        eventDetected = EVENT.CRASHING;
                    }

                    if (eventDetected != EVENT.NONE)
                    {
                        Logger.WriteLine(getName() + " : Event detected : " + eventDetected.ToString());
                    }

                    //manage the state machine
                    switch (currentState)
                    {
                        case STATE.WAITING:
                            {

                                if (localAirport != null)
                                {
                                    //only check reservation every 50 updates
                                    if (currentFlightStatus.counter % 50 == 0)
                                    {
                                        checkReservation();
                                    }
                                }

                                switch (eventDetected)
                                {
                                    case EVENT.ENGINESTART:
                                        {
                                            //we just started the engine, so we are taxiing
                                            Logger.WriteLine("State change WAITING -> TAXIING");
                                            getStartOfFlightData();

                                            //disable the immat selection if one is selected
                                            if (cbImmat.SelectedIndex >= 0)
                                            {
                                                cbImmat.Enabled = false;
                                            }
                                            else
                                            {
                                                Logger.WriteLine("No plane selected when engine started !");
                                                //try to guess the plane based on the current plane in the sim
                                                if (guessPlaneFromSim())
                                                {
                                                    cbImmat.Enabled = false;
                                                }
                                                else
                                                {
                                                    Logger.WriteLine("No plane could be guessed from sim !");
                                                    ShowMsgBox("No plane selected for this flight !\nPlease select a plane before starting the engines.", "No plane selected", MessageBoxButtons.OK);
                                                }
                                            }
                                            currentState = STATE.TAXIING;
                                            SimEvent(SimEventArg.EventType.ENGINESTART);

                                        }
                                        ; break;
                                    case EVENT.TAKEOFF:
                                        {
                                            //we just took off, so we are inflight
                                            getStartOfFlightData();

                                            //takeoff detected while waiting for engine start. Consider a no-engine flight.
                                            _noEngineFlight = true;

                                            //we just took off, so we are inflight
                                            measureTakeoffPerfs(currentFlightStatus);
                                            Logger.WriteLine("State change WAITING -> INFLIGHT");
                                            currentState = STATE.INFLIGHT;
                                            SimEvent(SimEventArg.EventType.TAKEOFF);
                                        }
                                        ; break;
                                    case EVENT.LANDING:
                                        {
                                            //we just landed, so we are taxiing
                                            measureLandingPerfs(currentFlightStatus);
                                            Logger.WriteLine("State change WAITING -> TAXIING");
                                            currentState = STATE.TAXIING;
                                            SimEvent(SimEventArg.EventType.LANDING);
                                        }
                                        ; break;
                                    case EVENT.ENGINESTOP:
                                        {
                                            //we just stopped the engine, so the flight is ended.
                                            Logger.WriteLine("State change WAITING -> ENDED (ENGINE STOP)");
                                            getEndOfFlightData();
                                            currentState = STATE.ENDED;
                                            SimEvent(SimEventArg.EventType.ENGINESTOP);
                                        }
                                        ; break;
                                    case EVENT.CRASHING:
                                        {
                                            //we just crashed, so the flight is ended.
                                            Logger.WriteLine("State change WAITING -> ENDED (CRASH)");
                                            getEndOfFlightData();
                                            currentState = STATE.ENDED;
                                            SimEvent(SimEventArg.EventType.CRASHING);
                                        }
                                        ; break;
                                }
                            }
                            ; break;
                        case STATE.INFLIGHT:
                            {
                                switch (eventDetected)
                                {
                                    case EVENT.LANDING:
                                        {
                                            measureLandingPerfs(currentFlightStatus);
                                            //we just landed, so we are taxiing
                                            Logger.WriteLine("State change INFLIGHT -> TAXIING");
                                            if (_noEngineFlight)
                                            {
                                                getEndOfFlightData();
                                                currentState = STATE.ENDED;
                                            }
                                            else
                                            {
                                                currentState = STATE.TAXIING;
                                                SimEvent(SimEventArg.EventType.LANDING);
                                            }
                                        }
                                        ; break;
                                    case EVENT.CRASHING:
                                        {
                                            //we crashed, so the flight is ended.
                                            Logger.WriteLine("State change INFLIGHT -> ENDED (CRASH)");
                                            getEndOfFlightData();
                                            currentState = STATE.ENDED;
                                            SimEvent(SimEventArg.EventType.CRASHING);
                                        }
                                        ; break;
                                    case EVENT.ENGINESTOP:
                                        {
                                            Logger.WriteLine("State change INFLIGHT -> GLIDING");
                                            //we need to wait for the validation timer to end before really stopping the flight.
                                            currentState = STATE.GLIDING;
                                            SimEvent(SimEventArg.EventType.ENGINESTOP);
                                        }
                                        ; break;
                                }
                            }
                            ; break;
                        case STATE.ENDED:
                            {
                                //we are ended, so we need to wait for a reset (engine start)
                                if (updatePlaneStatusTimer.Enabled)
                                {
                                    updatePlaneStatusTimer.Stop();
                                }

                                //the flight is ended, if there's an engine start, begin a new flight
                                switch (eventDetected)
                                {
                                    case EVENT.ENGINESTART:
                                        {
                                            //new flight started
                                            clearStartOfFLightData();
                                            clearEndOfFlightData();

                                            checkReservation();

                                            //il ne faut pas faire de reset flight ici, car sinon, on va re-détecter le demarrage moteur.
                                            //resetFlight(true);
                                            Logger.WriteLine("State change ENDED -> TAXIING");
                                            getStartOfFlightData();

                                            currentState = STATE.TAXIING;
                                            cbImmat.Enabled = false;
                                            SimEvent(SimEventArg.EventType.ENGINESTART);
                                        }
                                        ; break;
                                    case EVENT.TAKEOFF:
                                        {
                                            //we just took off, so we are inflight
                                            getStartOfFlightData();

                                            //takeoff detected while waiting for engine start. Consider a no-engine flight.
                                            _noEngineFlight = true;

                                            //we just took off, so we are inflight
                                            measureTakeoffPerfs(currentFlightStatus);
                                            Logger.WriteLine("State change WAITING -> INFLIGHT");
                                            currentState = STATE.INFLIGHT;
                                            SimEvent(SimEventArg.EventType.TAKEOFF);
                                        }
                                            ; break;
                                }
                                //wait for manual flight reset
                            }
                            ; break;
                        case STATE.TAXIING:

                            if ((localAirport) != null && ((reservation == null) || (!reservation.checkedOnce)))
                            {
                                checkReservation();
                            }

                            if (eventDetected == EVENT.TAKEOFF)
                            {
                                measureTakeoffPerfs(currentFlightStatus);
                                currentState = STATE.INFLIGHT;
                                SimEvent(SimEventArg.EventType.TAKEOFF);
                            }
                            else if (eventDetected == EVENT.CRASHING)
                            {
                                getEndOfFlightData();
                                currentState = STATE.ENDED;
                                SimEvent(SimEventArg.EventType.CRASHING);
                            }
                            else if (eventDetected == EVENT.ENGINESTOP)
                            {
                                //if this happen, then the engine are definitively stopped.
                                getEndOfFlightData();
                                currentState = STATE.ENDED;
                                SimEvent(SimEventArg.EventType.ENGINESTOP);
                            }
                            else if (eventDetected == EVENT.ENGINESTART)
                            {
                                //we just started the engine, while taxiing (should not happen, but who knows)
                                Logger.WriteLine("State change TAXIING -> TAXIING");
                                getStartOfFlightData();
                            }
                            break;
                        case STATE.GLIDING:
                            if (eventDetected == EVENT.LANDING)
                            {
                                measureLandingPerfs(currentFlightStatus);
                                currentState = STATE.TAXIING;
                                SimEvent(SimEventArg.EventType.LANDING);
                            }
                            else if (eventDetected == EVENT.CRASHING)
                            {
                                getEndOfFlightData();
                                currentState = STATE.ENDED;
                                SimEvent(SimEventArg.EventType.CRASHING);
                            }
                            else if (eventDetected == EVENT.ENGINESTART)
                            {
                                //the pilot restarted the engine during the gliding phase.
                                currentState = STATE.INFLIGHT;
                                SimEvent(SimEventArg.EventType.ENGINESTART);
                            }
                            break;
                    }

                    //keep new value of current fuel quantity
                    _currentFuel = currentFuel;

                    //update the flight performances data
                    //check gear and flaps deployment speeds
                    if (currentFlightStatus.gearRetractableFlag == 1)
                    {
                        //check gear position, if gear just deployed, get the airspeed
                        bool currentGearIsUp = gearIsUp;
                        gearIsUp = currentFlightStatus.gearIsUp;
                        if (!gearIsUp && currentGearIsUp)
                        {
                            Logger.WriteLine("Gear down detected. Measure speed");
                            //get the max air speed while deploying the gears
                            if (airspeedKnots > flightPerfs.gearDownSpeed)
                            {
                                //gear just went to be deployed ! check the current speed !!!
                                flightPerfs.gearDownSpeed = airspeedKnots;
                            }
                        }
                    }

                    if (currentFlightStatus.flapsAvailableFlag == 1)
                    {
                        //check the flaps deployment speed
                        uint currentFlapsPosition = flapsPosition;
                        flapsPosition = currentFlightStatus.flapsPosition;
                        //if flaps just went deployed, get the air speed.
                        if ((currentFlapsPosition == 0) && (flapsPosition > 0))
                        {
                            Logger.WriteLine("Flaps down detected. Measure speed");
                            //only keep the max flaps deployment air speed for this flight
                            if (airspeedKnots > flightPerfs.flapsDownSpeed)
                            {
                                flightPerfs.flapsDownSpeed = airspeedKnots;
                            }
                        }
                    }

                    if (currentFlightStatus.overSpeedWarning != 0)
                    {
                        Logger.WriteLine("overspeed warning detected");
                        flightPerfs.overspeed = true;
                    }

                    if (currentFlightStatus.stallWarning != 0)
                    {
                        Logger.WriteLine("stall warning detected");
                        flightPerfs.stallWarning = true;
                    }

                    //enregistre la position GPS si un moteur tourne, ou si on est pas au sol.
                    if (atLeastOneEngineFiring || !onGround)
                    {
                        if (currentFlightStatus.counter % Properties.Settings.Default.GPSRecordingInterval == 0)
                        {
                            DateTime timestamp = data.GetSimDateTimeUTC();  //DateTime.Now;
                            GPSRecorder.AddPoint(
                            _currentPosition.Location.Latitude,
                            _currentPosition.Location.Longitude,
                            _currentPosition.Altitude,
                            timestamp);
                        }
                    }

                    if (currentFlightStatus.counter % 100 == 0)
                    {
                        flightParamsRecorder.RecordFlightParams(currentFlightStatus);
                    }
                }
                else
                {
                    //sim not yet loaded 
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }

        }

        private bool guessPlaneFromSim()
        {
            bool found = false;
            //try to guess the plane based on the current plane in the sim
            string currentSimPlane = data.GetAircraftType();
            Avion guessedPlane = data.avions.Where(a => a.Designation == currentSimPlane).FirstOrDefault();
            if (guessedPlane != null)
            {
                cbImmat.SelectedItem = guessedPlane;
                Logger.WriteLine("Guessed plane from sim: " + guessedPlane.Immat);
                found = true;
            }
            else
            {
                Logger.WriteLine("Could not guess plane from sim: " + currentSimPlane);
            }
            return found;
        }

        private void clearEndOfFlightData()
        {
            tbCommentaires.Text = string.Empty;
            cbMission.Text = string.Empty;
            tbEndICAO.Text = string.Empty;

            lbEndFuel.Text = "Waiting end ...";
            lbEndIata.Text = "Waiting end ...";
            lbEndPosition.Text = "Waiting end ...";
            lbEndTime.Text = "--:--";

            _startTime = DateTime.UnixEpoch;
            _airborn = DateTime.UnixEpoch;
            _notAirborn = DateTime.UnixEpoch;
            _endTime = DateTime.UnixEpoch;

            lbTimeAirborn.Text = "--:--";
            lbTimeOnGround.Text = "--:--";
            cbNote.SelectedItem = 8;
        }

        private void RemplirComboImmat()
        {
            // Effacez les éléments existants dans la combobox
            cbImmat.Items.Clear();
            if ((data.avions != null) && (data.avions.Count > 0))
            {
                data.avions.Sort();
                // Parcourez la liste des avions
                foreach (Avion avion in data.avions)
                {
                    if (null != avion.Immat)
                    {
                        //immatriculations.Add(avion.Immat);
                        cbImmat.Items.Add(avion);
                    }
                }
                cbImmat.DisplayMember = "Immat";

                //pre-select the last used immat (stored as setting)
                string lastImmat = Settings.Default.lastImmat;
                if (lastImmat != string.Empty)
                {
                    try
                    {
                        Avion selected = data.avions.Where(a => a.Immat == lastImmat).First();
                        if (selected != null)
                        {
                            cbImmat.SelectedItem = selected;
                            //send the SETAIRCRAFT event
                            SimEventArg eventArg = new SimEventArg();
                            eventArg.reason = SimEventArg.EventType.SETAIRCRAFT;
                            eventArg.value = selected.Designation;
                            SimEvent(eventArg);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("Error selecting last used immat: " + ex.Message);
                        //if the last immat is not found, just select the first one
                        if (cbImmat.Items.Count > 0)
                        {
                            cbImmat.SelectedIndex = 0;
                        }

                    }
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void RemplirComboMissions()
        {
            if ((data.missions != null) && (data.missions.Count > 0))
            {
                //cbMission.Items.AddRange(data.missions.Select(mission => mission.Libelle).Where(mission => !string.IsNullOrEmpty(mission)).ToArray());
                cbMission.Items.AddRange(data.missions.ToArray());
            }
            //await dataReader.FillComboBoxMissionsAsync(cbMission);
            cbMission.DisplayMember = "Libelle";
            this.Cursor = Cursors.Default;
        }


        private short AnalyseFlight()
        {

            string comment = flightPerfs.getFlightComment();

            tbCommentaires.Text = comment;

            tbCommentaires.Text += " (F.R. V" + version.ToString(3) + ")";

            short note = flightPerfs.getFlightNote();
            cbNote.Text = note.ToString();
            return note;
        }

        private void getStartOfFlightData()
        {
            Logger.WriteLine("Getting start of flight data");
            endDisabled = 1;
            gbEndInfos.Enabled = false;

            _startPosition = data.GetPosition();

            double lat = _startPosition.Location.Latitude;
            double lon = _startPosition.Location.Longitude;

            localAirport = Aeroport.FindClosestAirport(data.aeroports, lat, lon);
            if (localAirport != null)
            {
                string startAirportname = localAirport.name;
                lbStartPosition.Text = startAirportname;
                lbStartIata.Text = localAirport.ident;

                //send the SETDEPARTURE event so that METAR can load local weather
                SimEventArg startEvent = new SimEventArg();
                startEvent.reason = SimEventArg.EventType.SETDEPARTURE;
                startEvent.value = localAirport.ident;
                SimEvent(startEvent);
            }

            _startFuel = data.GetFuelWeight();
            _startTime = data.GetSimDateTimeUTC();  //DateTime.Now;
            this.lbStartTime.Text = _startTime.ToShortTimeString();
            //0.00 => only keep 2 decimals for the fuel

            //Update the google sheet database indicating that this plane is being used
            UpdatePlaneStatus(1);
            //start the timer which will update the plane status
            updatePlaneStatusTimer.Start();

            this.lbStartFuel.Text = _startFuel.ToString("0.00");
            isRecording = true;
        }

        private void getEndOfFlightData()
        {
            // disable start detection for 300 x 100 ms =30s  disable the start text boxes.
            startDisabled = 100;
            gbStartInfos.Enabled = false;

            //change the mouse cursor to wait
            Cursor = Cursors.WaitCursor;


            Logger.WriteLine("Getting end of flight data");


            //on recupere les etats de fin de vol : heure, carbu, position.
            _endPosition = _currentPosition; // data.GetPosition();
            double lat = _endPosition.Location.Latitude;
            double lon = _endPosition.Location.Longitude;

            localAirport = Aeroport.FindClosestAirport(data.aeroports, lat, lon);
            if (localAirport != null)
            {
                string endAirportname = localAirport.name;
                lbEndPosition.Text = endAirportname;
                lbEndIata.Text = localAirport.ident;

                //send the SETDEPARTURE event so that METAR can load local weather
                SimEventArg startEvent = new SimEventArg();
                startEvent.reason = SimEventArg.EventType.SETDESTINATION;
                startEvent.value = localAirport.ident;
                SimEvent(startEvent);
            }

            //store the end of flight fuel value
            _endFuel = _currentFuel;
            DateTime now = data.GetSimDateTimeUTC();  //DateTime.Now;
            _endTime = now;

            this.lbEndTime.Text = _endTime.ToShortTimeString();
            //0.00 => only keep 2 decimals for the fuel
            this.lbEndFuel.Text = _endFuel.ToString("0.00");
            _endPayload = data.GetPayload();
            //compute the note of the flight
            _note = AnalyseFlight();
            isRecording = false;

            updatePlaneStatusTimer.Stop();
            //Update the google sheet database indicating that this plane is no more used
            UpdatePlaneStatus(0);

            //save optimized GPS trace
            GPSRecorder.OptimizeTraceRamerDouglasPeucker(0.0001);

            List<GPSPoint> gpsTrace = GPSRecorder.GPSPoints;
            string localFlightBookFile = Properties.Settings.Default.LocalFlightbookFile;
            List<FLightParamsSample> flightData = new List<FLightParamsSample>(flightParamsRecorder.GetRecordedFlightParams());

            if (localFlightBookFile == string.Empty)
            {
                localFlightBookFile = "local_flightbook.json";
            }
            //save the flight to the local flightbook
            localFlightBook.loadFromJson(localFlightBookFile);
            Flight newFlight = new Flight
            {
                immatriculation = cbImmat.Text,
                departureICAO = lbStartIata.Text,
                departureAirportName = lbStartPosition.Text,
                departureFuel = _startFuel,
                departureTime = _startTime,
                arrivalICAO = lbEndIata.Text,
                arrivalAirportName = lbEndPosition.Text,
                arrivalFuel = _endFuel,
                arrivalTime = _endTime,
                noteDuVol = _note,
                mission = cbMission.Text,
                commentaire = tbCommentaires.Text,
                payload = _endPayload,
                GPSData = gpsTrace,
                FlightParamsData = flightData,
                SimPlane = lbLibelleAvion.Text
            };

            localFlightBook.AddFlight(newFlight);
            localFlightBook.saveToJson(localFlightBookFile);
            string flightParamsFilename = Properties.Settings.Default.FlightParamsRecord;
            flightParamsFilename+="_"+lbStartIata.Text+"-"+lbEndIata.Text+"_"+_startTime.ToString("yyyyMMdd_HHmmss");

            Logger.WriteLine("End of flight data updated");
        }

        private bool CheckBeforeSave()
        {
            if (tbCallsign.Text == string.Empty)
            {
                throw new Exception("Please indicate callsign and click 'Apply'.");
            }

            // Define the regular expression pattern
            //string pattern = @"^SKY\d{4}$";
            //the pattern must be 3 letters followed by 4 digits
            string pattern = @"^[A-Z]{3}\d{4}$";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);
            // Check if the input string matches the pattern
            if (!regex.IsMatch(tbCallsign.Text))
            {
                throw new Exception("The string starts with 3 letters followed by 4 numbers.");
            }

            if (reservationStatus == ReservationMgr.ReservationStatus.Accepted)
            {
                //there is a reservation for this callsign at this airport
                if ((localAirport != null) && (localAirport.ident != reservation.ArrivalIcao))
                {
                    cbMission.Enabled = true;
                    //unselect the mission if the arrival airport does not match the reservation
                    cbMission.SelectedItem = null;

                    throw new Exception("The airport does not match the reservation arrival airport (" + reservation.ArrivalIcao + ").");
                }
            }

            return true;
        }

        private async Task<bool> saveFlight()
        {
            bool saveOK = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //if end of flight is not detected, get the data
                if (atLeastOneEngineFiring)
                {
                    Logger.WriteLine("Forcing end of flight detection before save");
                    getEndOfFlightData();
                }

                //cet appel declenche une exception si une condition de save n'est pas remplie.
                CheckBeforeSave();

                string fullComment = tbCommentaires.Text;
                //crée un dictionnaire des valeurs à envoyer
                SaveFlightDialog saveFlightDialog = new SaveFlightDialog(this, data);
                saveFlightDialog.IsReservationLocked = (reservationStatus == ReservationMgr.ReservationStatus.Accepted);

                saveFlightDialog.Callsign = tbCallsign.Text;
                saveFlightDialog.Immat = cbImmat.Text;
                saveFlightDialog.Comment = fullComment;
                saveFlightDialog.Cargo = _endPayload;
                saveFlightDialog.DepartureICAO = lbStartIata.Text;
                saveFlightDialog.DepartureTime = _startTime;
                saveFlightDialog.DepartureFuel = _startFuel;
                saveFlightDialog.ArrivalICAO = lbEndIata.Text;
                saveFlightDialog.ArrivalTime = _endTime;
                saveFlightDialog.ArrivalFuel = _endFuel;
                saveFlightDialog.Note = _note;
                saveFlightDialog.Mission = cbMission.Text;
                saveFlightDialog.GPSTrace = GPSRecorder.GetTraceJSON();
                saveFlightDialog.SimPlane = lbLibelleAvion.Text;

                bool isTopMost = false;
                Form parentForm = (Form)this.TopLevelControl;
                //en cas de "always on top"
                if (parentForm.TopMost)
                {
                    //temporairement desactive la always on top
                    isTopMost = true;
                    parentForm.TopMost = false;
                }

                if (saveFlightDialog.ShowDialog() == DialogResult.OK)
                {
                    // On grise le bouton save flight pour éviter les doubles envois
                    submitFlightToolStripMenuItem.Enabled = false;

                    string returnMessage = saveFlightDialog.returnMessage;
                    //store last plane used
                    string SimPlane = lbLibelleAvion.Text;
                    LocalPlanesDB.SetPlane(SimPlane, saveFlightDialog.Immat);

                    Properties.Settings.Default.lastSimPlane = SimPlane;
                    // #34 sauvegarder la derniere immat utilisée
                    Settings.Default.lastImmat = saveFlightDialog.Immat;
                    Settings.Default.Save();

                    //send the ENDOFFLIGHT event
                    SimEventArg endEvent = new SimEventArg();
                    endEvent.reason = SimEventArg.EventType.ENDOFFLIGHT;
                    endEvent.value = lbEndIata.Text;
                    SimEvent(endEvent);

                    //si tout va bien...
                    ShowMsgBox("Flight saved. " + returnMessage, "Flight Recorder", MessageBoxButtons.OK);

                    //cleanup after save
                    if (reservationStatus == ReservationMgr.ReservationStatus.Accepted)
                    {
                        Logger.WriteLine("Saving reservation as completed");
                        //mark the reservation as completed
                        data.CompleteReservation(Settings.Default.callsign, reservation);
                        reservation.Reserved = false;
                        //reset the reservation status to unknown for the next flight
                        reservationStatus = ReservationMgr.ReservationStatus.Unknown;

                        //clear the mission selection
                        cbMission.SelectedItem = null;
                        //re-enable the missions
                        cbMission.Enabled = true;
                        cbMission.Refresh();

                    }

                    //reset le vol sans demande de confirmation
                    resetFlight(true);
                    currentState = STATE.WAITING;
                    UpdateStatus("Waiting for engine start");

                    saveOK = true;
                }
                else
                {
                    //save canceled
                    saveOK = false;
                }

                if (isTopMost)
                {
                    //reactive le always on top
                    parentForm.TopMost = true;
                }

            }
            catch (Exception ex)
            {
                //in case if check error, or exception durung save, show a messagebox containing the error message
                ShowMsgBox(ex.Message, "Exception caught ", MessageBoxButtons.OK);
            }
            this.Cursor = Cursors.Default;
            return saveOK;

        }


        private void resetFlight(bool force) //force ==true => pas de demande de confirmation
        {
            Logger.WriteLine("Resetting flight");
            DialogResult res = DialogResult.Yes;
            if (!force)
            {
                res = ShowMsgBox("Confirm flight RESET ?", "Flight Recorder", MessageBoxButtons.YesNo);
            }

            if (res == DialogResult.Yes)
            {
                localAirport = null;

                clearStartOfFLightData();

                clearEndOfFlightData();

                lbFret.Visible = true;

                //reset flight infos.
                flightPerfs.reset();
                //reset the GPS recorder
                GPSRecorder.reset();
                //reset the flight params recorder
                flightParamsRecorder.ClearRecordedFlightParams();

                atLeastOneEngineFiring = false;
                stopEngineConfirmed = false;

                //libère l'avion dans la base de données
                updatePlaneStatusTimer.Stop();
                Logger.WriteLine("Freeing the airplane on the database");
                UpdatePlaneStatus(0);

                //reenable start detection at next timer tick
                startDisabled = 1;
                endDisabled = 1;
                _refuelDetected = false;
                _endPayload = 0;

                //on peut préparer un nouveau vol
                tbEndICAO.Enabled = true;
                lbPayload.Enabled = true;

                //btnSubmit.Enabled = false;
                submitFlightToolStripMenuItem.Enabled = false;
                cbMission.Enabled = true;

                pauseTime = TimeSpan.Zero;
                isPaused = false;

                switch (reservationStatus)
                {
                    case ReservationMgr.ReservationStatus.Accepted:
                        Logger.WriteLine("ResetFlight: reservation was accepted");
                        //ask the user if he wants to cancel the reservation
                        DialogResult resCancel = ShowMsgBox("Do you want to CANCEL the reservation ?", "Flight Recorder", MessageBoxButtons.YesNo);
                        if (resCancel == DialogResult.Yes)
                        {
                            Logger.WriteLine("User chose to cancel the reservation");
                            //mark the reservation as completed
                            data.CompleteReservation(Settings.Default.callsign, reservation);

                            reservation.Reserved = false;
                            reservationStatus = ReservationMgr.ReservationStatus.Unknown;
                            //reactivate the mission selection 
                            cbMission.SelectedItem = null;
                            cbMission.Enabled = true;
                            cbMission.Refresh();
                            cbImmat.Enabled = true;
                            cbImmat.Refresh();
                        }
                        break;
                    case ReservationMgr.ReservationStatus.Ignored:
                        Logger.WriteLine("ResetFlight: reservation was ignored");
                        //reset the reservation status to unknown for the next flight
                        reservationStatus = ReservationMgr.ReservationStatus.Unknown;
                        cbImmat.Enabled = true;
                        break;
                    case ReservationMgr.ReservationStatus.Unknown:
                        Logger.WriteLine("ResetFlight: reservation status unknown");
                        cbImmat.Enabled = true;
                        break;
                }
                //reset the reservation checked flag
                if (reservation != null)
                {
                    reservation.checkedOnce = false;
                }

                Logger.WriteLine("Flight reset");
            }
            else
            {
                Logger.WriteLine("Flight reset canceled");
            }
        }

        private void clearStartOfFLightData()
        {
            lbStartFuel.Text = "Waiting start";
            lbStartIata.Text = "Waiting start";
            lbStartPosition.Text = "Waiting start";
            lbStartTime.Text = "--:--";
        }

        private void BlackBoxCtrl_Load(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            //si l'utilisateur a modifié le texte du callsign, active le bouton pour le sauvrgarder
            btnSaveSettings.Enabled = true;
        }

        private void SaveCallSign()
        {
            //met le callsign en majuscules
            tbCallsign.Text = tbCallsign.Text.ToUpper();
            //recupere le texte dans le setting
            Settings.Default.callsign = tbCallsign.Text;
            //sauve le setting
            Settings.Default.Save();
            //desactive le bouton de sauvegarde.
            this.btnSaveSettings.Enabled = false;

        }
        //sauvegarde du callsign comme setting de l'application
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            SaveCallSign();

            SimEventArg eventArg = new SimEventArg();
            eventArg.reason = SimEventArg.EventType.SETCALLSIGN;
            eventArg.value = tbCallsign.Text;
            SimEvent(eventArg);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            btnSubmit.Enabled = false;
            saveFlight();
            btnSubmit.Enabled = true;
        }

        private void CbNote_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Flight details";
            string tipText = flightPerfs.getFlightNoteDetails();

            toolTip1.SetToolTip((Control)sender, tipText);

            toolTip1.Show(tipText, this, 5000);
        }

        private void tbEndICAO_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "destination airport";
            if (null != data)
            {
                Aeroport dest = data.aeroports.FirstOrDefault(a => a.ident == tbEndICAO.Text);
                if (null != dest)
                {
                    toolTip1.SetToolTip((Control)sender, dest.name);
                    toolTip1.Show(dest.name, this, 5000);
                }
            }
            else
            {
                toolTip1.SetToolTip((Control)sender, "Loading database...");
                toolTip1.Show("Loading database...", this, 5000);
            }

        }

        public async void UpdatePlaneStatus(int isFlying, string forceImmat = null)
        {
            try
            {
                //crée un dictionnaire des valeurs à envoyer
                Dictionary<string, string> values = new Dictionary<string, string>();


                //check that cbImmat is not null or empty. Only then we can send the update
                Avion plane = (Avion)cbImmat.SelectedItem;

                if (forceImmat != null)
                {
                    plane = data.avions.Where(a => a.Immat == forceImmat).FirstOrDefault();
                }

                if (plane == null)
                {
                    Logger.WriteLine("Cannot update plane status: selected plane is null");
                    return;
                }

                values["callsign"] = tbCallsign.Text;
                values["plane"] = forceImmat == null ? cbImmat.Text : forceImmat;
                values["departure_icao"] = lbStartIata.Text;
                values["flying"] = isFlying.ToString();
                values["arrival_icao"] = tbEndICAO.Text;

                if (_currentPosition != null)
                {
                    //if the current position is not null, use it to update the position
                    values["latitude"] = _currentPosition.Location.Latitude.ToString("0.00000", CultureInfo.InvariantCulture);
                    values["longitude"] = _currentPosition.Location.Longitude.ToString("0.00000", CultureInfo.InvariantCulture);
                }
                else
                {
                    //if the current position is null, use 0 as altitude
                    values["latitude"] = "0";
                    values["longitude"] = "0";
                }

                if (data != null)
                {

                    bool result = await (data.UpdatePlaneStatus(isFlying, values));
                    if (result)
                    {
                        //si tout va bien...
                        if (isFlying == 1)
                        {
                            _planeReserved = true;
                            plane.EnVol = 1;
                        }
                        else
                        {
                            _planeReserved = false;
                            plane.EnVol = 0;
                        }
                    }
                    else
                    {
                        //si tout va mal ...
                        Logger.WriteLine("Error updating plane status on the database");
                    }
                }
                else
                {
                    //data is null ! we're not connected to a flight simulator
                }
            }
            catch (Exception ex)
            {
                //in case if check error, or exception during save, show a messagebox containing the error message
                ShowMsgBox("Exception caught", ex.Message, MessageBoxButtons.OK);
            }
        }


        private void tbEndICAO_TextChanged(object sender, EventArgs e)
        {
            // Obtenir le texte actuel
            string text = tbEndICAO.Text;

            // Transformer en majuscules
            text = text.ToUpper();

            // Garder uniquement les lettres majuscules et les chiffres
            text = new string(text.Where(c => char.IsUpper(c) || char.IsDigit(c)).ToArray());

            // Limiter la longueur du texte à 4 caractères
            if (text.Length > 4)
            {
                text = text.Substring(0, 4);
            }

            // Mettre à jour le texte de la TextBox si nécessaire
            if (text != tbEndICAO.Text)
            {
                int selectionStart = tbEndICAO.SelectionStart; // Sauvegarder la position du curseur
                tbEndICAO.Text = text;
                tbEndICAO.SelectionStart = selectionStart > text.Length ? text.Length : selectionStart; // Restaurer la position du curseur
            }

            //only send the update if the text is long enough
            if (text.Length == 4)
            {
                //push this event, so that other plugins are notified that the destination was set
                SimEventArg eventArg = new SimEventArg();
                eventArg.reason = SimEventArg.EventType.SETDESTINATION;
                eventArg.value = tbEndICAO.Text;
                SimEvent(eventArg);
            }
        }

        private void cbImmat_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            if (e.Index >= 0)
            {
                Avion item = (Avion)cbImmat.Items[e.Index];
                switch (item.Status)
                {
                    case Avion.PlaneStatus.Disponible:
                        myBrush = Brushes.Black;
                        break;
                    case Avion.PlaneStatus.Maintenance:
                    case Avion.PlaneStatus.Maintenance2:
                        myBrush = Brushes.LightGray;
                        break;
                    case Avion.PlaneStatus.Reserved:
                        if ((item.DernierUtilisateur == tbCallsign.Text) && (reservationStatus == ReservationMgr.ReservationStatus.Accepted))
                            myBrush = Brushes.Blue; // réservataire
                        else
                            myBrush = Brushes.LightGray; // réservé, non sélectionnable
                        break;
                }
                e.Graphics.DrawString(item.Immat, e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
            }
            e.DrawFocusRectangle();
        }

        private void CbImmat_SelectedIndexChanged(object sender, EventArgs e)
        {
            //free the previous plane if it was marked as in flight
            if (currentPlane != null)
            {
                if ((currentPlane.EnVol == 1) && (currentPlane.DernierUtilisateur == tbCallsign.Text))
                {
                    Logger.WriteLine("Freeing the previous airplane on the database");
                    UpdatePlaneStatus(0, currentPlane.Immat);
                }
            }

            //reset the atLeastOneEngineFiring flag to force the detection of engine start for the new plane
            atLeastOneEngineFiring = false;

            Avion selectedPlane = this.data.avions.Where(a => a.Immat == cbImmat.Text).FirstOrDefault();
            if (selectedPlane != null)
            {
                if (!selectedPlane.IsSelectable(Settings.Default.callsign, reservationStatus))
                {
                    cbImmat.SelectedItem = null;
                    lbDesignationAvion.Text = "<no plane selected>";
                    return;
                }
                string planeDesign = selectedPlane.Designation;
                lbDesignationAvion.Text = planeDesign;

                //mettre a jour l'icone suivant le type_aeroport d'avion.

                switch (selectedPlane.Type)
                {
                    case ("Monomoteur"):
                        {
                            panelAircraftTypeIcon.BackgroundImage = Properties.Resources.monomoteur;
                        }
                    ; break;
                    case ("Bimoteur"):
                        {
                            panelAircraftTypeIcon.BackgroundImage = Properties.Resources.bimoteur;
                        }
                    ; break;
                    case ("Liner"):
                        {
                            panelAircraftTypeIcon.BackgroundImage = Properties.Resources.liner;
                        }
                    ; break;
                    case ("Helico"):
                        {
                            panelAircraftTypeIcon.BackgroundImage = Properties.Resources.helico;
                        }
                    ; break;
                }

                //vérifie les paramètres de l'avion selectionné (ex si l'avion est dans la base locale, si l'immat est correcte, etc)
                checkParameters();

                ////si cet avion est marqué comme deja en vol, c'est par l'utilisateur courant. 
                ////marque cet avion comme n'etant plus en vol.
                //if ((selectedPlane.EnVol == 1) && selectedPlane.DernierUtilisateur == tbCallsign.Text)
                //{
                //    Logger.WriteLine("Freeing the airplane on the database");
                //    UpdatePlaneStatus(0);
                //}

                //store last plane used
                currentPlane = selectedPlane;

                //push this event, so that other plugins are notified that the plane was selected
                SimEventArg eventArg = new SimEventArg();
                eventArg.reason = SimEventArg.EventType.SETAIRCRAFT;
                eventArg.value = selectedPlane.Designation;
                SimEvent(eventArg);
            }
        }


        private void BtnReset_Click(object sender, EventArgs e)
        {
            //reset flight avec demande de confirmation
            resetFlight(false);
            currentState = STATE.WAITING;
            UpdateStatus("Waiting for engine start");
        }

        private void resetFlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //reset flight avec demande de confirmation
            resetFlight(false);
            currentState = STATE.WAITING;
            UpdateStatus("Waiting for engine start");
        }

        private void submitFlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFlight();
        }

        private void engineStopTimer_Tick(object sender, EventArgs e)
        {
            cbImmat.Enabled = true;
            tbEndICAO.Enabled = true;
            //stop this timer
            engineStopTimer.Stop();
            Logger.WriteLine("Engine stop confirmed");
            stopEngineConfirmed = true;
        }

        private void checkParameters()
        {
            string planeNomComplet = data.GetAircraftType();

            if (tbCallsign.Text == string.Empty)
            {
                ledCheckCallsign.Color = Color.Red;
            }
            else
            {
                ledCheckCallsign.Color = Color.LightGreen;
            }

            List<string> knownImmat = LocalPlanesDB.GetPlaneName(planeNomComplet);
            string immat = cbImmat.Text;

            bool foundError = false;

            if ((knownImmat != null) && (knownImmat.Count != 0))
            {
                if (!knownImmat.Contains(immat))
                {
                    foundError = true;
                }
            }
            else
            {
                //si l'avion n'est pas dans la base locale, on ne peut pas verifier l'immatriculation.  
                foundError = true;
            }

            if (foundError)
            {
                lbLibelleAvion.ForeColor = Color.Red;
                lbDesignationAvion.ForeColor = Color.Red;
                ledCheckAircraft.Color = Color.Red;
                ledCheckImmat.Color = Color.Red;
            }
            else
            {
                lbLibelleAvion.ForeColor = Color.White;
                lbDesignationAvion.ForeColor = Color.White;
                ledCheckAircraft.Color = Color.LightGreen;
                ledCheckImmat.Color = Color.LightGreen;
            }

        }

        public async void ReadStaticValues()
        {
            try
            {

                if (data.isConnectedToSim && data.GetReadyToFly())
                {
                    Logger.WriteLine("Reading static values");
                    //commence à lire qq variables du simu : fuel & cargo, immat avion...
                    this.lbPayload.Text = data.GetPayload().ToString("0.00");
                    ledCheckPayload.Color = Color.LightGreen;

                    //Recupere le libellé de l'avion
                    string planeNomComplet = data.GetAircraftType();
                    lbLibelleAvion.Text = planeNomComplet;

                    checkParameters();

                    //recupere l'emplacement courant :
                    _currentPosition = data.GetPosition();

                    double lat = _currentPosition.Location.Latitude;
                    double lon = _currentPosition.Location.Longitude;

                    if ((data.aeroports != null) && (data.aeroports.Count > 0))
                    {
                        Aeroport currentAirport = Aeroport.FindClosestAirport(data.aeroports, lat, lon);
                        if ((currentAirport != null) && (currentAirport != localAirport))
                        {
                            localAirport = currentAirport;
                            string startAirportname = localAirport.name;

                            if ((data.aeroports != null) && (startAirportname != null))
                            {
                                // Votre code pour utiliser les avions et les aéroports
                                float fretOnAirport = await data.GetFretOnAirport(localAirport.ident);
                                //lbFret.Text = fretOnAirport.ToString() + " Kg available " + startAirportname;

                                if ((localAirport != null) && (fretOnAirport > 0))
                                {
                                    lbFret.Text = "Available freight at " + localAirport.ident + " : " + fretOnAirport.ToString();
                                    Logger.WriteLine(lbFret.Text);
                                    ledCheckFreight.Color = Color.LightGreen;
                                    ledCheckFreight.On = true;
                                }
                                else
                                {
                                    lbFret.Text = "No freight here";
                                    ledCheckFreight.Color = Color.LightGreen;
                                    ledCheckFreight.On = false;
                                }

                            }
                        }
                    }
                    staticValuesReadOnce = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Exception caught : " + ex.Message);
            }
        }

        private void timerUpdateStaticValues_Tick(object sender, EventArgs e)
        {
            //si on est au sol, et moteur arretés, alors on continue de rafraichir les données statiques.
            //sinon (en vol, ou des que les moteurs sont allumés, on ne change plus ça).
            if ((onGround && !atLeastOneEngineFiring) || (!staticValuesReadOnce))
            {
                ReadStaticValues();
            }
        }

        private void tbCallsign_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                SaveCallSign();
            }
        }

        private void lbDesignationAvion_Click(object sender, EventArgs e)
        {

        }

        private void lbEndICAO_Click(object sender, EventArgs e)
        {

        }

        private void screenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cbNote_SelectedIndexChanged(object sender, EventArgs e)
        {
            _note = short.Parse(cbNote.Text);
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dbg = new DebugForm();
            if (dbg.Visible) return;
            dbg.Show();
        }

        void ISimAddonPluginCtrl.SetExecutionFolder(string path)
        {
            Logger.WriteLine("Setting execution folder to: " + path);
            executionFolder = path;
        }

        public void ManageSimEvent(object sender, SimEventArg eventArg)
        {
            if (eventArg.reason == SimEventArg.EventType.SETCALLSIGN)
            {
                tbCallsign.Text = eventArg.value;
                SaveCallSign();
            }

            if (eventArg.reason == SimEventArg.EventType.SETDESTINATION)
            {
                tbEndICAO.Text = eventArg.value;
            }
        }

        private void btnFlightbook_Click(object sender, EventArgs e)
        {
            LocalFlightbookForm localFlightbookForm = new LocalFlightbookForm(this, data);
            localFlightbookForm.loadFlightbook(Properties.Settings.Default.LocalFlightbookFile);
            //localFlightbookForm.ShowDialog(this);
            if (OnShowDialog != null)
            {
                OnShowDialog(this, localFlightbookForm);
            }
        }

        private void updatePlaneStatusTimer_Tick(object sender, EventArgs e)
        {
            UpdatePlaneStatus(_planeReserved ? 1 : 0);
        }

        private void ledCheckAircraft_DoubleClick(object sender, EventArgs e)
        {
            string simPlane = lbLibelleAvion.Text;
            List<string> knownImmat = LocalPlanesDB.GetPlaneName(simPlane);
            string immat = cbImmat.Text;
            if ((knownImmat != null) && (knownImmat.Count != 0))
            {
                if (knownImmat.Contains(immat))
                {
                    Logger.WriteLine("Removing plane from local database: " + simPlane + " / " + immat);
                    LocalPlanesDB.RemovePlane(simPlane, immat);
                }
                else
                {
                    Logger.WriteLine("Adding new plane to local database: " + simPlane + " / " + immat);
                    LocalPlanesDB.SetPlane(simPlane, immat);
                }
            }
            else
            {
                Logger.WriteLine("Adding new plane to local database: " + simPlane + " / " + immat);
                LocalPlanesDB.SetPlane(simPlane, immat);
            }
            checkParameters();
        }

        private void ledCheckAircraft_MouseHover(object sender, EventArgs e)
        {
            toolTip1.ToolTipTitle = "Aircraft registration";
            string tipText = "Double click to add/remove this registration from the local database";
            toolTip1.SetToolTip((Control)sender, tipText);
            toolTip1.Show(tipText, this, 5000);
        }

        private void ledCheckAircraft_MouseEnter(object sender, EventArgs e)
        {
            //change the cursor to a hand
            this.Cursor = Cursors.Hand;
        }

        private void ledCheckAircraft_MouseLeave(object sender, EventArgs e)
        {
            //change the cursor to default
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Apply a reservation: select immatriculation if present, set departure ICAO and set comment to "vol régulier".
        /// Public so Form1 can call it via reflection.
        /// </summary>
        public void ApplyReservation(string immat, string departureIcao, string arrivalIcao = "")
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ApplyReservation(immat, departureIcao, arrivalIcao)));
                return;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(immat) && (cbImmat != null))
                {
                    // helper to normalize immatriculation strings for comparison
                    static string NormalizeImmat(string s)
                    {
                        if (string.IsNullOrWhiteSpace(s)) return string.Empty;
                        return new string(s.ToUpperInvariant().Where(c => char.IsLetterOrDigit(c)).ToArray());
                    }

                    string targetNorm = NormalizeImmat(immat);

                    Avion foundAvion = null;
                    foreach (var item in cbImmat.Items.Cast<object>())
                    {
                        if (item == null) continue;
                        var t = item.GetType();
                        var imProp = t.GetProperty("Immat");
                        string itemImmat = imProp != null ? imProp.GetValue(item)?.ToString() ?? string.Empty : item.ToString();
                        if (NormalizeImmat(itemImmat) == targetNorm)
                        {
                            foundAvion = item as Avion;
                            break;
                        }
                    }

                    // Met à jour le statut et le réservataire
                    foundAvion.Status = SimDataManager.Avion.PlaneStatus.Reserved;
                    foundAvion.DernierUtilisateur = tbCallsign.Text;

                    // Force la sélection
                    //_suppressSelectionValidation = true;
                    cbImmat.SelectedItem = foundAvion;

                    cbImmat.Refresh();
                    // Empêche la modification par l'utilisateur
                    cbImmat.Enabled = false;

                    lbDesignationAvion.Text = foundAvion.Designation;
                    // send the SETAIRCRAFT event
                    SimEventArg eventArg = new SimEventArg();
                    eventArg.reason = SimEventArg.EventType.SETAIRCRAFT;
                    eventArg.value = foundAvion.Designation;
                    SimEvent(eventArg);

                    checkParameters();

                    // Marque l'avion comme réservé dans la base de données
                    data.ApplyReservation(Settings.Default.callsign, reservation);
                }

                if (!string.IsNullOrWhiteSpace(departureIcao) && lbStartIata != null)
                {
                    lbStartIata.Text = departureIcao.ToUpper();

                    //send the SETDEPARTURE event so that METAR can load local weather
                    SimEventArg startEvent = new SimEventArg();
                    startEvent.reason = SimEventArg.EventType.SETDEPARTURE;
                    startEvent.value = departureIcao.ToUpper();
                    SimEvent(startEvent);
                }

                // set arrival ICAO in ACARS (tbEndICAO or tbArrival equivalent)
                try
                {
                    if (!string.IsNullOrWhiteSpace(arrivalIcao))
                    {
                        var tbEnd = this.GetType().GetField("tbEndICAO", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(this) as TextBox;
                        if (tbEnd != null)
                        {
                            tbEnd.Text = arrivalIcao.ToUpper();
                        }
                        else
                        {
                            // maybe property
                            var prop = this.GetType().GetProperty("tbEndICAO", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                            if (prop != null)
                            {
                                var tb = prop.GetValue(this) as TextBox;
                                if (tb != null) tb.Text = arrivalIcao.ToUpper();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ApplyReservation: setting arrival ICAO failed: " + ex.Message);
                }
                // Sélectionne la mission "LIGNES REGULIERES" si présente
                try
                {
                    cbMission.SelectedItem = data.missions.Single(m => m.Libelle == "LIGNES REGULIERES");
                }
                catch (Exception)
                {
                    // mission non trouvée, on ignore
                }

                if (tbCommentaires != null)
                {
                    tbCommentaires.Text = "Vol régulier.";
                }

                Logger.WriteLine($"ApplyReservation applied immat={immat} dep={departureIcao}");
            }
            catch (Exception ex)
            {
                Logger.WriteLine("ApplyReservation error: " + ex.Message);
            }
        }

        private void cbMission_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mission selectedMission = (Mission)cbMission.SelectedItem;
            if (selectedMission != null)
            {
                if (!selectedMission.IsSelectable(reservationStatus == ReservationMgr.ReservationStatus.Accepted))
                {
                    string message = "No reservation found for this mission\n";
                    message += "Please make a reservation on the website before selecting this mission.";
                    ShowMsgBox(message, "Flight Recorder", MessageBoxButtons.OK);
                    cbMission.SelectedItem = null;
                    return;
                }
            }
        }

        private void cbMission_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background of the item.
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            // Determine the color based on the item status
            if (e.Index >= 0)
            {
                Mission item = (Mission)cbMission.Items[e.Index];
                if (item.IsSelectable(reservationStatus == ReservationMgr.ReservationStatus.Accepted))
                {
                    myBrush = Brushes.Black;
                }
                else
                {
                    myBrush = Brushes.LightGray;
                }
                e.Graphics.DrawString(item.Libelle, e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void timerUpdateFleetStatus_Tick(object sender, EventArgs e)
        {
            //si on est au sol, et moteur arretés, alors on continue de rafraichir les données statiques.
            if (onGround && !atLeastOneEngineFiring)
            {
                if (data.avions.Count > 0)
                {
                    data.UpdatePlaneFromSheet();
                }
            }
        }

        public string getFlightReport(REPORTFORMAT format)
        {
            string report = string.Empty;
            Flight lastFlight = localFlightBook.GetLastFLight();
            if (lastFlight != null)
            {
                //create a flight report in markdown format
                switch(format)
                {
                    case REPORTFORMAT.MD:
                        report = lastFlight.GenerateMarkdownReport();
                        break;
                    case REPORTFORMAT.JSON:
                        //return a json structure
                        report = lastFlight.GenerateJSONReport();
                        break;
                    case REPORTFORMAT.HTML:
                        report = lastFlight.GenerateHTMLReport();
                        break;
                    default:
                        report = "{ \"error\": \"Unknown format requested\" }";
                        break;
                }
                return report;
            }
            else
            {
                //return a basic json structure with an error message
                return "<H2>No flight found !</H2>";
            }
        }
    }
}
