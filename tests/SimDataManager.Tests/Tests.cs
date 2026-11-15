// Tests de non-regression SimAddon (branche poc/simbridge-client).
// A lancer AVANT toute distribution : dotnet run -c Release dans ce dossier.
// Les tests utilisent un port UDP isole (49195 par defaut, surchargeable via
// SIMBRIDGE_TEST_PORT) : ils passent meme si SimAddon ou X-Plane tournent encore.
//
// Garanties couvertes :
//  1. LocalPlanesDB : les cles sont normalisees (Trim) -> une ancienne association
//     faite sous XPUIPC ("Cessna 172 SP GTN REP " avec espace final) est retrouvee
//     quand le pont SimBridge envoie "Cessna 172 SP GTN REP" (sans espace). Ca marche
//     pour TOUS les utilisateurs, quel que soit le nom exact de l'avion.
//  2. Sans trame SimBridge (MSFS/FSUIPC, ou X-Plane sans le plugin) : UsingBridge
//     reste false -> 100% du code existant FSUIPC/XUIPC, AUCUNE regression possible.
//  3. Avec trames SimBridge (X-Plane 12 + plugin) : le pont devient la source en mode
//     auto, le stall reflete la trame AVEC le guard au sol (pas de faux stall au parking).
//  4. Mode "fsuipc" force : meme avec des trames, UsingBridge reste false (protection MSFS).
//
// Zero dependance NuGet (runner maison). Exit 0 = PASS, 1 = FAIL.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using FlightRecPlugin;
using SimDataManager;

static class Tests
{
    static int _fails = 0, _total = 0;
    static void Check(bool cond, string label)
    {
        _total++;
        Console.WriteLine((cond ? "  ok   " : "  FAIL ") + label);
        if (!cond) _fails++;
    }

    // ---------------------------------------------------------------- LocalPlanesDB
    static void LocalPlanesDBTests()
    {
        Console.WriteLine("== 1. LocalPlanesDB : normalisation des cles (compat historique XPUIPC) ==");
        string tmp = Path.Combine(Path.GetTempPath(), "simaddon-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tmp);
        try
        {
            // Simule une vieille base ecrite sous XPUIPC (espace final dans la cle)
            File.WriteAllText(Path.Combine(tmp, "planesdb.json"),
                "{ \"Cessna 172 SP GTN REP \": [\"OK-PRW\", \"F-GVHR\"] }");
            LocalPlanesDB.SetStorageFolderForTests(tmp);

            var a = LocalPlanesDB.GetPlaneName("Cessna 172 SP GTN REP");   // sans espace (pont SimBridge)
            Check(a != null && a.Contains("OK-PRW"), "cle historique avec espace final retrouvee sans espace");
            var b = LocalPlanesDB.GetPlaneName("Cessna 172 SP GTN REP ");  // avec espace (XPUIPC)
            Check(b != null && b.Contains("OK-PRW"), "cle avec espace final fonctionne toujours");

            LocalPlanesDB.SetPlane("Beechcraft Baron 58 REP ", "P2NTJ");
            Check(LocalPlanesDB.GetPlaneName("Beechcraft Baron 58 REP") != null, "SetPlane trimme la cle a l'ecriture");
            LocalPlanesDB.RemovePlane("Beechcraft Baron 58 REP", "P2NTJ");
            Check(LocalPlanesDB.GetPlaneName("Beechcraft Baron 58 REP") == null, "RemovePlane fonctionne apres trim");
        }
        finally
        {
            try { Directory.Delete(tmp, true); } catch { }
        }
    }

    // ---------------------------------------------------------------- simData bridge
    static int TestPort
    {
        get
        {
            var env = Environment.GetEnvironmentVariable("SIMBRIDGE_TEST_PORT");
            return int.TryParse(env, out var p) ? p : 49195;
        }
    }

    static string Frame(int onGround, double ias, int stall, double alpha)
    {
        return "{\"type\":\"simbridge\",\"v\":2,\"ts_ms\":" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() +
               ",\"on_ground\":" + onGround + ",\"ias_kt\":" + ias.ToString(System.Globalization.CultureInfo.InvariantCulture) +
               ",\"stall_warning\":" + stall + ",\"stall_warning_ratio\":0.0,\"alpha\":" +
               alpha.ToString(System.Globalization.CultureInfo.InvariantCulture) + ",\"stall_alpha\":12.0," +
               "\"num_engines\":1,\"eng_running\":[1],\"eng_n1_pct\":[0.0],\"eng_ff_kg_s\":[0.0]}";
    }

    static void SendLoop(UdpClient tx, System.Net.IPEndPoint dest, Func<string> frame, int ms, ref bool stop)
    {
        while (!stop)
        {
            byte[] b = Encoding.UTF8.GetBytes(frame());
            try { tx.Send(b, b.Length, dest); } catch { }
            Thread.Sleep(ms);
        }
    }

    static void SimDataBridgeTests()
    {
        Console.WriteLine("== 2. simData SANS trame SimBridge : reste sur FSUIPC/XUIPC (MSFS inchange) ==");
        var sd = new simData("https://skywings.ovh/", TestPort);
        Thread.Sleep(400); // laisse le récepteur UDP demarrer (aucune trame recue)
        Check(!sd.IsSimBridgeActive, "sans trame : IsSimBridgeActive == false");
        Check(!sd.UsingBridge, "sans trame : UsingBridge == false (mode auto) -> FSUIPC/XUIPC, rien ne change");

        Console.WriteLine("== 3. AVEC trames SimBridge : pont actif en auto, stall avec guard au sol ==");
        using var tx = new UdpClient();
        var dest = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, TestPort);
        bool stop = false;
        // phase 1 : au sol, faux stall (alpha wrappe, flag parasite) -> GetStallWarning doit etre 0
        var th = new Thread(() => SendLoop(tx, dest, () => Frame(1, 0.0, 1, 179.1), 100, ref stop)) { IsBackground = true };
        th.Start();
        Thread.Sleep(1200); // laisse arriver quelques trames
        Check(sd.IsSimBridgeActive, "avec trames : IsSimBridgeActive == true");
        Check(sd.UsingBridge, "mode auto + trames : UsingBridge == true (pont actif)");
        Check(sd.GetStallWarning() == 0, "au sol avec stall brut=1 (alpha=179) : GetStallWarning == 0 (guard)");
        stop = true; th.Join(500);

        // phase 2 : en vol, vrai stall -> 1
        stop = false;
        th = new Thread(() => SendLoop(tx, dest, () => Frame(0, 68.0, 1, 16.5), 100, ref stop)) { IsBackground = true };
        th.Start();
        Thread.Sleep(1200);
        Check(sd.GetStallWarning() == 1, "en vol avec stall=1 : GetStallWarning == 1");
        stop = true; th.Join(500);

        Console.WriteLine("== 4. Mode 'fsuipc' force : meme avec trames, on reste sur FSUIPC (protection MSFS) ==");
        stop = false;
        th = new Thread(() => SendLoop(tx, dest, () => Frame(0, 68.0, 1, 16.5), 100, ref stop)) { IsBackground = true };
        th.Start();
        Thread.Sleep(800);
        sd.SetSimConnector("fsuipc");
        Check(sd.IsSimBridgeActive, "trames reçues : IsSimBridgeActive == true");
        Check(!sd.UsingBridge, "mode fsuipc force : UsingBridge == false malgre les trames");
        stop = true; th.Join(500);

        sd.SetSimConnector("auto");
    }

    // ---------------------------------------------------------------- FlightEventsLog
    static SimAddonPlugin.situation Sit(int onGround, byte stall, byte over, double ias, uint flaps, bool anyFiring, bool anyCut)
    {
        var s = new SimAddonPlugin.situation();
        s.timestamp = new DateTime(2026, 11, 14, 15, 19, 52);
        s.onGround = (short)onGround;
        s.stallWarning = stall;
        s.overSpeedWarning = over;
        s.airSpeed = ias;
        s.flapsPosition = flaps;
        s.isAtLeastOneEngineFiring = anyFiring;
        s.isAnyEngineCut = anyCut;
        return s;
    }

    static void FlightEventsTests()
    {
        Console.WriteLine("== 5. FlightEventsLog : transitions moteurs / stall / overspeed ==");
        var log = new FlightEventsLog();
        log.Update(Sit(1, 0, 0, 0, 0, false, false), new[] { 0 });
        log.Update(Sit(1, 0, 0, 0, 0, true, false), new[] { 1 });
        Check(log.Events.Count == 1 && log.Events[0].Text.Contains("Démarrage"), "démarrage moteurs détecté");
        log.Update(Sit(0, 0, 0, 120, 0, true, false), new[] { 1 });
        log.Update(Sit(0, 0, 0, 110, 0, false, true), new[] { 0 });
        Check(log.Events.Any(e => e.Text.Contains("Coupure moteur 1")), "coupure moteur 1 en vol détectée");
        log.Update(Sit(0, 0, 0, 115, 0, true, false), new[] { 1 });
        Check(log.Events.Any(e => e.Text.Contains("Redémarrage moteur 1")), "redémarrage moteur 1 détecté");
        log.Update(Sit(0, 1, 0, 48, 0, true, false), new[] { 1 });
        Check(log.Events.Any(e => e.Text.Contains("Décrochage") && e.Text.Contains("48") && e.Text.Contains("lisse")), "stall lisse Vs=48 kt détecté");
        log.Update(Sit(0, 0, 0, 60, 0, true, false), new[] { 1 });
        log.Update(Sit(0, 1, 0, 42, 16383, true, false), new[] { 1 });
        Check(log.Events.Any(e => e.Text.Contains("volets sortis") && e.Text.Contains("42")), "stall volets sortis Vs=42 kt détecté");
        log.Update(Sit(0, 0, 1, 210, 0, true, false), new[] { 1 });
        Check(log.Events.Any(e => e.Text.Contains("Overspeed")), "overspeed détecté en vol");
        Check(System.Text.RegularExpressions.Regex.IsMatch(log.Events[0].ToDisplayString(), @"^\d{2}/\d{2}/\d{4} \d{2}:\d{2} — "), "timestamp au format JJ/MM/AAAA HH:mm (heure locale réelle)");

        var log2 = new FlightEventsLog();
        log2.Update(Sit(1, 0, 0, 0, 0, true, false), new[] { 1 });
        log2.Update(Sit(1, 1, 0, 0, 0, true, false), new[] { 1 });
        Check(!log2.Events.Any(e => e.Text.Contains("Décrochage")), "pas de stall enregistré au sol");

        var alerts = log.GetActiveAlerts(Sit(0, 1, 1, 200, 0, true, true));
        Check(alerts.Contains("STALL") && alerts.Contains("OVERSPEED") && alerts.Contains("COUPURE MOTEUR"), "alertes en cours STALL+OVERSPEED+COUPURE MOTEUR");

        // fallback FSUIPC (engRunning == null) via agrégats
        var log3 = new FlightEventsLog();
        log3.Update(Sit(1, 0, 0, 0, 0, false, false), null);
        log3.Update(Sit(1, 0, 0, 0, 0, true, false), null);
        log3.Update(Sit(0, 0, 0, 120, 0, true, false), null);
        log3.Update(Sit(0, 0, 0, 110, 0, true, true), null);
        Check(log3.Events.Any(e => e.Text == "Démarrage moteurs") && log3.Events.Any(e => e.Text == "Coupure moteur"), "fallback FSUIPC : démarrage + coupure via agrégats");
    }

    // ---------------------------------------------------------------- FlightPerfs note
    static void FlightNoteTests()
    {
        Console.WriteLine("== 6. FlightPerfs : note de vol, vol de contrôle exclu de l'estimation ==");
        var fp = new FlightPerfs();
        fp.stallWarning = true;
        fp.engineCut = true;
        fp.overspeed = true;
        Check(fp.getFlightNote() < 10, "vol classique avec stall+overspeed : note < 10");
        Check(fp.getFlightNote(true) == 10, "vol de contrôle (même avec stall/coupure) : note = 10");
        var fp2 = new FlightPerfs();
        Check(fp2.getFlightNote() == 10 && fp2.getFlightNote(true) == 10, "vol clean : note = 10 dans les deux cas");
    }

    // ---------------------------------------------------------------- Mission.ParDefaut
    static void MissionDefautTests()
    {
        Console.WriteLine("== 7. Mission.ParDefaut : mission par défaut au lancement ==");
        var missions = new List<Mission>
        {
            new Mission { Libelle = "Op Liner", Active = 1, EstControle = 0, TypesAutorises = "Liner" },
            new Mission { Libelle = "Mission De Contrôle", Active = 1, EstControle = 1, TypesAutorises = "*" },
            new Mission { Libelle = "Lignes Regulieres", Active = 2, EstControle = 0, TypesAutorises = "*" },
            new Mission { Libelle = "Loisir", Active = 1, EstControle = 0, TypesAutorises = "*" },
            new Mission { Libelle = "Taxi", Active = 1, EstControle = 0, TypesAutorises = "" }, // champ absent côté serveur
        };
        var def = Mission.ParDefaut(missions);
        Check(def != null && def.Libelle == "Loisir", "par défaut = première mission active, non-contrôle, tous types ('Loisir')");
        Check(Mission.ParDefaut(null) == null, "liste null -> null (pas de crash)");
        var sansDefaut = new List<Mission> { new Mission { Libelle = "Op Liner", Active = 1, EstControle = 0, TypesAutorises = "Liner" } };
        Check(Mission.ParDefaut(sansDefaut) == null, "aucune mission tous-types -> null (mission laissée vide)");
    }

    static int Main()
    {
        Console.WriteLine("Tests SimAddon (poc/simbridge-client) — port UDP de test : " + TestPort);
        LocalPlanesDBTests();
        SimDataBridgeTests();
        FlightEventsTests();
        FlightNoteTests();
        MissionDefautTests();
        Console.WriteLine();
        Console.WriteLine(_fails == 0 ? $"PASS — {_total}/{_total} tests OK" : $"FAIL — {_fails}/{_total} en echec");
        return _fails == 0 ? 0 : 1;
    }
}
