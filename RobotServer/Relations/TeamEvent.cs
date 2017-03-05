using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.SQLDataObjects
{
    public class TeamEvent
    {
        public int TeamNumber { get; set; }
        public Team Team { get; set; }

        public string EventId { get; set; }
        
        public Event Event { get; set; }
    }
}
