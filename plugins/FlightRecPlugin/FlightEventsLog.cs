using System;
using System.Collections.Generic;
using System.Linq;
using SimAddonLogger;
using SimAddonPlugin;

namespace FlightRecPlugin
{
    /// <summary>Un événement de vol daté (démarrage/arrêt/coupure/redémarrage moteur, décrochage, overspeed).</summary>
    public class FlightEvent
    {
        public DateTime When { get; set; }
        public string Text { get; set; } = "";

        /// <summary>Ligne prête à afficher, horodatage au format français JJ/MM/AAAA HH:mm.</summary>
        public string ToDisplayString()
        {
            return When.ToString("dd/MM/yyyy HH:mm") + " — " + Text;
        }
    }

    /// <summary>
    /// Enregistre les événements du vol détectés par transitions d'état :
    /// moteurs (démarrage / arrêt / coupure en vol / redémarrage), décrochage (avec vitesse
    /// et configuration lisse vs volets sortis) et overspeed. Les événements restent dans
    /// l'historique même une fois l'alerte retombée.
    /// </summary>
    public class FlightEventsLog
    {
        private readonly List<FlightEvent> _events = new List<FlightEvent>();
        public IReadOnlyList<FlightEvent> Events => _events;

        private int[] _prevEngRunning;
        private bool _prevAnyRunning;
        private bool _prevAnyCut;
        private bool _prevStall;
        private bool _prevOver;
        private bool _initialized;

        public void Reset()
        {
            _events.Clear();
            _prevEngRunning = null;
            _prevAnyRunning = false;
            _prevAnyCut = false;
            _prevStall = false;
            _prevOver = false;
            _initialized = false;
        }

        private void Add(string text, DateTime when)
        {
            _events.Add(new FlightEvent { When = when, Text = text });
            Logger.WriteLine("FlightEvent: " + when.ToString("dd/MM/yyyy HH:mm") + " — " + text);
        }

        /// <summary>
        /// Met à jour le log à partir de la situation courante.
        /// engRunning : état par moteur via le pont SimBridge, ou null en FSUIPC/XUIPC
        /// (auquel cas on retombe sur les agrégats isAtLeastOneEngineFiring / isAnyEngineCut).
        /// </summary>
        public void Update(situation s, int[] engRunning)
        {
            if (s == null) return;
            bool inFlight = s.onGround == 0;
            DateTime when = s.timestamp == default ? DateTime.Now : s.timestamp;

            if (engRunning != null)
            {
                if (!_initialized)
                {
                    _prevEngRunning = engRunning.ToArray();
                    _initialized = true;
                }
                else
                {
                    for (int i = 0; i < engRunning.Length && i < _prevEngRunning.Length; i++)
                    {
                        if (inFlight && _prevEngRunning[i] == 1 && engRunning[i] == 0)
                            Add("Coupure moteur " + (i + 1), when);
                        if (inFlight && _prevEngRunning[i] == 0 && engRunning[i] == 1)
                            Add("Redémarrage moteur " + (i + 1), when);
                    }
                    int runningNow = engRunning.Count(e => e == 1);
                    int runningPrev = _prevEngRunning.Count(e => e == 1);
                    if (runningPrev == 0 && runningNow > 0) Add("Démarrage moteurs", when);
                    if (runningPrev > 0 && runningNow == 0) Add("Arrêt moteurs", when);
                    _prevEngRunning = engRunning.ToArray();
                }
            }
            else
            {
                bool anyRunning = s.isAtLeastOneEngineFiring;
                bool anyCut = s.isAnyEngineCut;
                if (!_initialized) { _initialized = true; }
                else
                {
                    if (!_prevAnyRunning && anyRunning) Add("Démarrage moteurs", when);
                    if (_prevAnyRunning && !anyRunning) Add("Arrêt moteurs", when);
                    if (inFlight && !_prevAnyCut && anyCut) Add("Coupure moteur", when);
                    if (inFlight && _prevAnyCut && !anyCut && anyRunning) Add("Redémarrage moteur", when);
                }
                _prevAnyRunning = anyRunning;
                _prevAnyCut = anyCut;
            }

            if (inFlight && !_prevStall && s.stallWarning != 0)
            {
                double flapsRatio = s.flapsPosition / 16383.0; // échelle FSUIPC 0..16383
                string config = flapsRatio < 0.1 ? "lisse" : "volets sortis";
                Add(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "Décrochage — Vs {0:0} kt ({1})", s.airSpeed, config), when);
            }
            if (inFlight && !_prevOver && s.overSpeedWarning != 0)
            {
                Add("Overspeed (Vne dépassée)", when);
            }

            _prevStall = s.stallWarning != 0;
            _prevOver = s.overSpeedWarning != 0;
        }

        /// <summary>Alertes actuellement actives (pour la page web) : STALL / OVERSPEED / COUPURE MOTEUR.</summary>
        public List<string> GetActiveAlerts(situation s)
        {
            var alerts = new List<string>();
            if (s == null) return alerts;
            if (s.stallWarning != 0) alerts.Add("STALL");
            if (s.overSpeedWarning != 0) alerts.Add("OVERSPEED");
            if (s.isAnyEngineCut) alerts.Add("COUPURE MOTEUR");
            return alerts;
        }
    }
}
