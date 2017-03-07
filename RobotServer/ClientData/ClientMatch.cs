using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotServer.ClientData
{
    public class ClientMatch
    {
        public string MatchId { get; set; }
        public string MatchInfo { get; set; }
        public List<ClientTeam> Teams { get; set; }
        public ClientEvent Event { get; set; }
        public string EventId { get; set; }
        public List<ClientRobotEvent> RobotEvents { get; set; }
    }
}