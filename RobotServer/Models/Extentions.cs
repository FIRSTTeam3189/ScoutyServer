using System;
using System.Collections.Generic;
using System.Linq;
using ScoutingServer.SQLDataObjects;
using RobotServer.SQLDataObjects;

namespace RobotServer.Models
{
    public static class Extentions
    {
        public static bool Contains(this List<Event> events, Event e)
        {
            return events.Any(ev => e.EventId == ev.EventId);
        }
    }
}