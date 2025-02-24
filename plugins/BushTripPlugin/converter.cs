using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flightplan;
using simbrief;

namespace BushTripPlugin
{
    public static class converter
    {
        public static LittleNavmap FlightplanFromOFP(OFP simbrief)
        {
            LittleNavmap result = new LittleNavmap();
            result.CurrentStep = 0;
            result.Item = new LittleNavmapFlightplan();
            result.Item.Header=new LittleNavmapFlightplanHeader();
            result.Item.Header.ProgramName = "simbrief";
            int nbWaypoints = 2 + simbrief.navlog.Length;
            result.Item.Waypoints = new LittleNavmapFlightplanWaypoint[nbWaypoints];
            LittleNavmapFlightplanWaypoint origin = new LittleNavmapFlightplanWaypoint();
            origin.Name = simbrief.origin.name;
            origin.Ident = simbrief.origin.icao_code;
            origin.Pos = new LittleNavmapFlightplanWaypointPos();
            origin.Pos.Lon = simbrief.origin.pos_long;
            origin.Pos.Lat = simbrief.origin.pos_lat;
            origin.Type = "AIRPORT";
            result.Item.Waypoints[0] = origin;

            for (int i = 1; i < nbWaypoints - 1; i++)
            {
                LittleNavmapFlightplanWaypoint wp = new LittleNavmapFlightplanWaypoint();
                wp.Name = simbrief.navlog[i - 1].name;
                wp.Ident = simbrief.navlog[i - 1].ident;
                wp.Pos = new LittleNavmapFlightplanWaypointPos();
                wp.Pos.Lon = simbrief.navlog[i - 1].pos_long;
                wp.Pos.Lat = simbrief.navlog[i - 1].pos_lat;
                wp.Type = "WAYPOINT";
                result.Item.Waypoints[i] = wp;
            }

            LittleNavmapFlightplanWaypoint dest = new LittleNavmapFlightplanWaypoint();
            dest.Name = simbrief.destination.name;
            dest.Ident = simbrief.destination.icao_code;
            dest.Pos = new LittleNavmapFlightplanWaypointPos();
            dest.Pos.Lon = simbrief.destination.pos_long;
            dest.Pos.Lat = simbrief.destination.pos_lat;
            dest.Type = "AIRPORT";
            result.Item.Waypoints[nbWaypoints-1] = dest;

            return result;
        }
    }
}
