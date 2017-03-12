using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.SQLDataObjects
{
    public class TeamEvent
    {
        public TeamEvent() { }
        public TeamEvent(Team team, Event ev) {
            TeamNumber = team.TeamNumber;
            EventId = ev.EventId;
        }
        public int TeamNumber { get; set; }
        public Team Team { get; set; }

        public string EventId { get; set; }
        
        public Event Event { get; set; }
        
        public override bool Equals(object obj) {
            var other = obj as TeamEvent;
            if(other != null)
                return other.EventId == this.EventId && other.TeamNumber == this.TeamNumber;

            return false;
        }

        public static bool operator ==(TeamEvent a, TeamEvent b) {
            return a?.EventId == b?.EventId && a?.TeamNumber == b?.TeamNumber;
        }

        public static bool operator != (TeamEvent a, TeamEvent b) {
            return !(a == b);
        }
    }
}
