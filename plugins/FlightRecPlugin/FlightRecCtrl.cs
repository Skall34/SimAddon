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

namespace FlightRecPlugin
{
    public partial class FlightRecCtrl : UserControl, ISimAddonPluginCtrl
    {
        const string name = "FlightRecorder";
        Version? version;

        DebugForm dbg;
        bool simReady;

        List<string> missions;
        List<string> immats;

        private bool atLeastOneEngineFiring;
        private int startDisabled; // if startDisabled==0, then start is possible, if not, start is disabled. each 100ms, the counter will be decremented
        private int endDisabled;

        PositionSnapshot _currentPosition;
        PositionSnapshot _startPosition;
        PositionSnapshot _endPosition;

        private Aeroport localAirport;

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

        private GPSRecorder GPSRecorder;

        private readonly FlightPerfs flightPerfs;

        private simData data;

        public event ISimAddonPluginCtrl.OnTalkHandler OnTalk;

        public event ISimAddonPluginCtrl.UpdateStatusHandler OnStatusUpdate;

        public event ISimAddonPluginCtrl.OnSimEventHandler OnSimEvent;

        public event ISimAddonPluginCtrl.OnShowMsgboxHandler OnShowMsgbox;

        private string executionFolder;

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

        private DialogResult ShowMsgBox(string title, string caption, MessageBoxButtons buttons)
        {
            if (OnShowMsgbox != null)
            {
                return OnShowMsgbox(this, title, caption, buttons);
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

            _startTime = DateTime.UnixEpoch;
            _endTime = DateTime.UnixEpoch;
            _airborn = DateTime.UnixEpoch;
            _notAirborn = DateTime.UnixEpoch;

            _startFuel = 0;
            _endFuel = 0;
            _currentFuel = 0;
            _refuelDetected = false;
            _endPayload = 0;

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

            missions = new List<string>();
            immats = new List<string>();

            flightPerfs = new FlightPerfs();
            GPSRecorder = new GPSRecorder();
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

            this.Enabled = true;

            UpdateStatus("FlightRecorder is ready");
            Logger.WriteLine("FlightRecorder is ready");

            timerUpdateStaticValues.Start();

            //send the callsign event
            SimEventArg eventArg = new SimEventArg();
            eventArg.reason = SimEventArg.EventType.SETCALLSIGN;
            eventArg.value = tbCallsign.Text;
            SimEvent(eventArg);
        }

        public async void FormClosing(object sender, FormClosingEventArgs e)
        {
            //need to ask for save before close
            string message = "Confirm close ACARS ?";
            if (e.CloseReason == CloseReason.UserClosing)
            {


                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult res = ShowMsgBox(message, "Closing", MessageBoxButtons.OKCancel);
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
                            cbImmat.Enabled = true;
                            //tbEndICAO.Enabled = true;
                            System.Threading.Thread.Sleep(2000);
                            this.Cursor = Cursors.Default;
                        }
                    }
                    else
                    {
                        // Si l'utilisateur clique sur Annuler, annule la fermeture de la fenêtre.
                        Logger.WriteLine("close canceled by user");
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                //close forced. (probably autostart of application by the simulator)
                if (btnSubmit.Enabled)
                {

                }
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


        public void updateSituation(situation currentFlightStatus)
        {
            try
            {
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
                        //lbStartFuel.Enabled = true;
                        //lbStartPosition.Enabled = true;
                        //lbStartTime.Enabled = true;
                        //lbStartIata.Enabled = true;
                    }
                }

                if (endDisabled > 0)
                {
                    endDisabled -= 1;

                    if (endDisabled <= 0)
                    {
                        //the start detection disable timer expired, restore the start textboxes.
                        gbEndInfos.Enabled = true;
                        //lbEndFuel.Enabled = true;
                        //lbEndPosition.Enabled = true;
                        //lbEndTime.Enabled = true;
                        //lbEndIata.Enabled = true;
                    }
                }

                if (simReady)
                {
                    _currentPosition = currentFlightStatus.position;

                    // Airspeed
                    double airspeedKnots = currentFlightStatus.airSpeed;
                    double currentFuel = currentFlightStatus.currentFuel;


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
                                //we just took off ! read the plane weight
                                flightPerfs.takeOffWeight = currentFlightStatus.planeWeight;
                                //keep memory that we're airborn
                                onGround = false;
                                // on veut afficher la date
                                _airborn = DateTime.Now;
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

                                SimEvent(SimEventArg.EventType.TAKEOFF);
                            }
                        }
                        //constamment mettre à jour la vrtical acceleration et airspeed pendant le vol.
                        flightPerfs.landingVerticalAcceleration = currentFlightStatus.verticalAcceleration;
                        flightPerfs.landingSpeed = currentFlightStatus.airSpeed;
                    }
                    else //we're on ground !
                    {
                        //we are on ground, check if we were airborn before
                        if (!onGround)
                        {
                            //we were airborn, and now we are on ground.
                            if (DateTime.Now - _airborn >= TimeSpan.FromSeconds(30))
                            {
                                Logger.WriteLine("Landing detected !");
                                UpdateStatus("On ground");
                                //only update the touchDownVSpeed if we've been airborn once
                                flightPerfs.landingVSpeed = currentFlightStatus.landingVerticalSpeed;
                                flightPerfs.landingWeight = currentFlightStatus.planeWeight;

                                _notAirborn = DateTime.Now;
                                flightPerfs.landingTime = _notAirborn;

                                if (lbTimeOnGround.Text == "--:--")
                                {
                                    this.lbTimeOnGround.Text = _notAirborn.ToString("HH:mm");
                                }

                                //check for crashes
                                if (currentFlightStatus.offRunwayCrashed != 0)
                                {
                                    Logger.WriteLine("off runway crashed detected");
                                    flightPerfs.overRunwayCrashed = true;
                                    getEndOfFlightData();
                                }
                                if (currentFlightStatus.crashedFlag != 0)
                                {
                                    Logger.WriteLine("crash detected");
                                    flightPerfs.crashed = true;
                                    getEndOfFlightData();
                                }

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
                            ////si on refuel pendant qu'un moteur tourne, c'est suspect. Envoie la popup pour proposer le reset !
                            //if (atLeastOneEngineFiring)
                            //{
                            //    if (DialogResult.OK == ShowMsgBox("Refuel detected ! do you want to reset the flight !", "Refuel detected", MessageBoxButtons.OKCancel))
                            //    {
                            //        resetFlight(true);
                            //    }
                            //}
                            //else
                            //{

                            //}
                        }

                    }

                    //keep new value of current fuel quantity
                    _currentFuel = currentFuel;

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

                            if (engineStopTimer.Enabled)
                            {
                                Logger.WriteLine("Engine stop canceled. Validation timer stopped");
                                engineStopTimer.Stop();
                            }
                            else
                            {
                                if (onGround)
                                {
                                    Logger.WriteLine("First engine start detected for plane" + cbImmat.Text);
                                    //this.WindowState = FormWindowState.Minimized;
                                    getStartOfFlightData();
                                    resetEndOfFlightData();

                                    //Update the google sheet database indicating that this plane is being used
                                    UpdatePlaneStatus(1);

                                    //start the time which will update the plane status
                                    updatePlaneStatusTimer.Start();

                                    cbImmat.Enabled = false;
                                    //tbEndICAO.Enabled = false;

                                    SimEvent(SimEventArg.EventType.ENGINESTART);
                                }
                                else
                                {
                                    //demarrage des moteur en vol (redémarrage)... ne rien faire.
                                    Logger.WriteLine("Engine start during flight. Do nothing");
                                }
                            }
                        }


                    // si on detecte un arret moteur

                        if (_previousEngineStatus && !currentFlightStatus.isAtLeastOneEngineFiring)
                        {
                            //garde le nouvel etat.
                            atLeastOneEngineFiring = currentFlightStatus.isAtLeastOneEngineFiring;

                            // si on est au sol, et qu'on autorise la detection de l'arret moteur
                            if (onGround && (endDisabled == 0))
                            {
                                Logger.WriteLine("Potential engine stop detected. Start validation timer");
                                engineStopTimer.Start();
                            }
                            else
                            {
                                //si on est en vol, OU si la detection est desactivée, ne rien faire.
                                Logger.WriteLine("Potential engine stop detected during flight. Do nothing");
                            }
                        }
                        else
                        {
                            //no change on engine status.
                        }


                    if (atLeastOneEngineFiring)
                    {
                        GPSRecorder.AddPoint(
                            _currentPosition.Location.Latitude,
                            _currentPosition.Location.Longitude,
                            _currentPosition.Altitude,
                            DateTime.Now);
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

        private void RemplirComboImmat()
        {
            lbFret.Text = "Acars initializing ..... please wait";
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
                        immats.Add(avion.Immat);
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
                        cbImmat.SelectedItem = selected;

                        //send the SETAIRCRAFT event
                        SimEventArg eventArg = new SimEventArg();
                        eventArg.reason = SimEventArg.EventType.SETAIRCRAFT;
                        eventArg.value = selected.Designation;
                        SimEvent(eventArg);
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
                cbMission.Items.AddRange(data.missions.Select(mission => mission.Libelle).Where(mission => !string.IsNullOrEmpty(mission)).ToArray());
                missions.AddRange(data.missions.Select(mission => mission.Libelle).Where(mission => !string.IsNullOrEmpty(mission)).ToArray());
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
            }

            _startFuel = data.GetFuelWeight();
            _startTime = DateTime.Now;
            this.lbStartTime.Text = _startTime.ToShortTimeString();
            //0.00 => only keep 2 decimals for the fuel

            this.lbStartFuel.Text = _startFuel.ToString("0.00");
           
        }

        private void resetEndOfFlightData()
        {

            lbEndFuel.Text = "Waiting end...";
            lbEndPosition.Text = "Waiting end...";
            lbEndTime.Text = "--:--";
            lbEndIata.Text = "Waiting end...";

            tbCommentaires.Text = string.Empty;
        }

        private void getEndOfFlightData()
        {
            // disable start detection for 300 x 100 ms =30s  disable the start text boxes.
            startDisabled = 300;
            gbStartInfos.Enabled = false;

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
            }

            //store the end of flight fuel value
            _endFuel = _currentFuel;

            _endTime = DateTime.Now;
            this.lbEndTime.Text = _endTime.ToShortTimeString();
            //0.00 => only keep 2 decimals for the fuel
            this.lbEndFuel.Text = _endFuel.ToString("0.00");
            _endPayload = data.GetPayload();
            //compute the note of the flight
            _note = AnalyseFlight();
            Logger.WriteLine("End of flight data updated");
        }

        private bool CheckBeforeSave()
        {
            if (tbCallsign.Text == string.Empty)
            {
                throw new Exception("Please indicate callsign and click 'Apply'.");
            }

            // Define the regular expression pattern
            string pattern = @"^SKY\d{4}$";
            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern);
            // Check if the input string matches the pattern
            if (!regex.IsMatch(tbCallsign.Text))
            {
                throw new Exception("The string starts with 'SKY' followed by four numbers.");
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

                //cet appel declenche une exception si une conditio nde save n'es tpas remplie.
                CheckBeforeSave();

                string fullComment = tbCommentaires.Text;
                //crée un dictionnaire des valeurs à envoyer
                SaveFlightDialog saveFlightDialog = new SaveFlightDialog(data);

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
                    //store last plane used
                    string SimPlane = lbLibelleAvion.Text;
                    Properties.Settings.Default.lastSimPlane = SimPlane;
                    // #34 sauvegarder la derniere immat utilisée
                    Settings.Default.lastImmat = saveFlightDialog.Immat;
                    Settings.Default.Save();

                    //si tout va bien...
                    ShowMsgBox("Flight saved. Thank you for flying with SKYWINGS :)", "Flight Recorder", MessageBoxButtons.OK);

                    //reset le vol sans demande de confirmation
                    resetFlight(true);
                    // On grise le bouton save flight pour éviter les doubles envois
                    //btnSubmit.Enabled = false;
                    submitFlightToolStripMenuItem.Enabled = false;
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
                //in case if check error, or exception durong save, show a messagebox containing the error message
                ShowMsgBox(ex.Message, "Exception caught ", MessageBoxButtons.OK);
            }
            this.Cursor = Cursors.Default;
            return saveOK;

        }


        private void resetFlight(bool force) //force ==true => pas de demande de confirmation
        {
            Logger.WriteLine("Reseting flight");
            DialogResult res = DialogResult.OK;
            if (!force)
            {
                res = ShowMsgBox("Confirm flight reset ?", "Flight Recorder", MessageBoxButtons.OKCancel);
            }

            if (res == DialogResult.OK)
            {
                localAirport = null;
                lbStartIata.Text = string.Empty;
                lbStartFuel.Text = string.Empty;
                lbStartPosition.Text = string.Empty;
                lbStartTime.Text = string.Empty;

                lbEndTime.Text = string.Empty;
                lbEndFuel.Text = string.Empty;
                lbEndIata.Text = string.Empty;
                lbEndPosition.Text = string.Empty;

                tbCommentaires.Text = string.Empty;
                cbMission.Text = string.Empty;

                tbEndICAO.Text = string.Empty;

                lbStartFuel.Text = "Waiting start";
                lbEndFuel.Text = "Waiting end ...";
                lbStartIata.Text = "Waiting start";
                lbEndIata.Text = "Waiting end ...";
                lbStartPosition.Text = "Waiting start";
                lbEndPosition.Text = "Waiting end ...";

                lbStartTime.Text = "--:--";

                _airborn = DateTime.UnixEpoch;
                _notAirborn = DateTime.UnixEpoch;
                _startTime = DateTime.UnixEpoch;
                _endTime = DateTime.UnixEpoch;

                lbEndTime.Text = "--:--";
                lbTimeAirborn.Text = "--:--";
                lbTimeOnGround.Text = "--:--";
                lbFret.Visible = true;
                cbNote.SelectedItem = 8;

                //reset flight infos.
                flightPerfs.reset();
                //reset the GPS recorder
                GPSRecorder.reset();

                atLeastOneEngineFiring = false;

                //libère l'avion dans la base de données
                if (cbImmat.SelectedItem != null)
                {
                    Avion selectedPlane = (Avion)cbImmat.SelectedItem;
                    if (selectedPlane != null)
                    {
                        //si l'avion est marqué comme en vol, on le libère.
                        //stop the timer
                        updatePlaneStatusTimer.Stop();

                        Logger.WriteLine("Freeing the airplane on the sheet");
                        UpdatePlaneStatus(0);
                    }
                }

                //reenable start detection at next timer tick
                startDisabled = 1;
                endDisabled = 1;
                _refuelDetected = false;
                _endPayload = 0;

                //on peut préparer un nouveau vol
                cbImmat.Enabled = true;
                tbEndICAO.Enabled = true;
                lbPayload.Enabled = true;

                //btnSubmit.Enabled = false;
                submitFlightToolStripMenuItem.Enabled = false;
                Logger.WriteLine("Flight reset");
            }
            else
            {
                Logger.WriteLine("Flight reset canceled");
            }
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

        public async void UpdatePlaneStatus(int isFlying)
        {
            try
            {
                //crée un dictionnaire des valeurs à envoyer
                Dictionary<string, string> values = new Dictionary<string, string>();
                //UrlDeserializer.PlaneUpdateQuery planedata = new UrlDeserializer.PlaneUpdateQuery
                //{
                //    query = "updatePlaneStatus",
                //    qtype = "json",
                //    cs = tbCallsign.Text,
                //    plane = cbImmat.Text,
                //    sicao = lbStartIata.Text,
                //    flying = isFlying,
                //    endIcao = tbEndICAO.Text
                //};

                values["callsign"] = tbCallsign.Text;
                values["plane"] = cbImmat.Text;
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
                    values["latitude"] = "";
                    values["longitude"] = "";
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
                        }
                        else
                        {
                            _planeReserved = false;
                        }
                    }
                    else
                    {
                        //si tout va mal ...

                    }
                }
                else
                {
                    //data is null ! we're not connected to a flight simulator
                }
            }
            catch (Exception ex)
            {
                //in case if check error, or exception durong save, show a messagebox containing the error message
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
                UpdatePlaneStatus(_planeReserved ? 1 : 0);
                //todo ! search for this airport in the database.
                //if found, udate the tooltip with the airport name.

                //push this event, so that other plugins are notified that the destination was set
                SimEventArg eventArg = new SimEventArg();
                eventArg.reason = SimEventArg.EventType.SETDESTINATION;
                eventArg.value = tbEndICAO.Text;
                SimEvent(eventArg);
            }
        }

        private void cbImmat_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Define the default color of the brush as black.
            Brush myBrush = Brushes.Black;

            // Draw the current item text based on the current Font 
            // and the custom brush settings.
            if (e.Index >= 0)
            {
                if (cbImmat.Items.Count > 1)
                {
                    Avion item = (Avion)cbImmat.Items[e.Index];
                    switch (item.Status)
                    {
                        case 0:
                            myBrush = Brushes.Black; //avion disponible
                            break;
                        case 1:
                            myBrush = Brushes.LightGray; //avion non disponible (en maintenance).
                            break;
                        case 2:
                            myBrush = Brushes.LightGray;//avion non disponible (en maintenance).
                            break;
                    }

                    if (item.EnVol == 1)
                    {
                        if (item.DernierUtilisateur != tbCallsign.Text)
                        {
                            myBrush = Brushes.LightGray; //avion non disponible (utilisé par qqun d'autre).
                        }
                        else
                        {
                            myBrush = Brushes.Blue; //avion non disponible (deja pris par moi).
                        }
                    }

                    e.Graphics.DrawString(item.Immat,
                        e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                }
            }
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void CbImmat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Avion selectedPlane = this.data.avions.Where(a => a.Immat == cbImmat.Text).FirstOrDefault();
            if (selectedPlane != null)
            {
                if ((selectedPlane.Status == 1) || (selectedPlane.Status == 2) || ((selectedPlane.EnVol == 1) && (selectedPlane.DernierUtilisateur != tbCallsign.Text)))
                {
                    cbImmat.SelectedItem = null;
                    lbDesignationAvion.Text = "<no plane selected>";
                }
                else
                {
                    string planeDesign = selectedPlane.Designation;
                    lbDesignationAvion.Text = planeDesign;

                    //mettre a jour l'icone suivant le type d'avion.

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



                    checkParameters();

                    //si cet avion est marqué comme deja en vol, c'est par l'utilisateur courant. 
                    //marque cet avion comme n'etant plus en vol.
                    if ((selectedPlane.EnVol == 1) && selectedPlane.DernierUtilisateur == tbCallsign.Text)
                    {
                        Logger.WriteLine("Freeing the airplane on the sheet");
                        UpdatePlaneStatus(0);
                    }

                    //push this event, so that other plugins are notified that the plane was selected
                    SimEventArg eventArg = new SimEventArg();
                    eventArg.reason = SimEventArg.EventType.SETAIRCRAFT;
                    eventArg.value = selectedPlane.Designation;
                    SimEvent(eventArg);
                }
            }
        }



        private void BtnReset_Click(object sender, EventArgs e)
        {
            //reset flight avec demande de confirmation
            resetFlight(false);
        }

        private void resetFlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //reset flight avec demande de confirmation
            resetFlight(false);
        }

        private void submitFlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFlight();
        }

        private void engineStopTimer_Tick(object sender, EventArgs e)
        {
            //stop the timer
            updatePlaneStatusTimer.Stop();

            //if this happen, then the engine are definitively stopped.
            getEndOfFlightData();

            //Update the google sheet database indicating that this plane is no more used
            UpdatePlaneStatus(0);
            cbImmat.Enabled = true;
            tbEndICAO.Enabled = true;
            //stop this timer
            engineStopTimer.Stop();

            SimEvent(SimEventArg.EventType.ENGINESTOP);

            //save the GPS trace
            GPSRecorder.SaveToJSON(Path.Combine(executionFolder, lbStartIata.Text + "_" + lbEndIata.Text + "_raw_trace.json"));

            //GPSRecorder.OptimizeTrace();
            GPSRecorder.OptimizeTraceRamerDouglasPeucker(0.0001);

            string gpsTrace = GPSRecorder.GetTraceJSON();

            string localFlightBookFile = Path.Combine(executionFolder, Properties.Settings.Default.LocalFlightbookFile);
            //save the flight to the local flightbook
            LocalFlightBook localFlightBook = new LocalFlightBook();
            localFlightBook.loadFromJson(localFlightBookFile);
            Flight newFlight = new Flight
            {
                immatriculation = cbImmat.Text,
                departureICAO = lbStartIata.Text,
                departureFuel = _startFuel,
                departureTime = _startTime,
                arrivalICAO = lbEndIata.Text,
                arrivalFuel = _endFuel,
                arrivalTime = _endTime,
                noteDuVol = _note,
                mission = cbMission.Text,
                commentaire = tbCommentaires.Text,
                payload = _endPayload,
                GPSData = gpsTrace
            };

            localFlightBook.AddFlight(newFlight);
            localFlightBook.saveToJson(localFlightBookFile);

            GPSRecorder.SaveToJSON(Path.Combine(executionFolder, lbStartIata.Text + "_" + lbEndIata.Text + "_trace.json"));

            GPSRecorder.SaveToKML(Path.Combine(executionFolder, lbStartIata.Text + "_" + lbEndIata.Text + "_trace.kml"));
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



            if (Properties.Settings.Default.lastSimPlane != string.Empty)
            {
                bool foundError = false;

                if (planeNomComplet == Properties.Settings.Default.lastSimPlane)
                {
                    if (Settings.Default.lastImmat != cbImmat.Text)
                    {
                        foundError = true;
                    }
                }
                else
                {
                    if (Settings.Default.lastImmat == cbImmat.Text)
                    {
                        foundError = true;
                    }
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
        }

        public async void ReadStaticValues()
        {
            try
            {
                if (data.isConnected && data.GetReadyToFly())
                {
                    Logger.WriteLine("Reading static values");
                    //commence à lire qq variables du simu : fuel & cargo, immat avion...
                    this.lbPayload.Text = data.GetPayload().ToString("0.00");
                    ledCheckPayload.Color = Color.LightGreen;

                    //recupere l'emplacement courant :
                    _currentPosition = data.GetPosition();

                    //Recupere le libellé de l'avion
                    string planeNomComplet = data.GetAircraftType();
                    lbLibelleAvion.Text = planeNomComplet;

                    checkParameters();

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

                                if (fretOnAirport > 0)
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
            if (onGround && !atLeastOneEngineFiring)
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
            LocalFlightbookForm localFlightbookForm = new LocalFlightbookForm(data);
            localFlightbookForm.loadFlightbook(Path.Combine(executionFolder,Properties.Settings.Default.LocalFlightbookFile));
            localFlightbookForm.ShowDialog(this);
        }

        private void updatePlaneStatusTimer_Tick(object sender, EventArgs e)
        {
            UpdatePlaneStatus(_planeReserved ? 1 : 0);
        }
    }
}
