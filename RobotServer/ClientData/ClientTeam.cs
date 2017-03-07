using System;
using System.Collections.Generic;
using System.Linq;
using ScoutingServer.SQLDataObjects;
using System.ComponentModel.DataAnnotations;

namespace RobotServer.ClientData
{
    public class ClientTeam {
        public int TeamNumber { get; set; }
        public string Name { get; set; }
        public int RookieYear { get; set; }
        public List<ClientEvent> Events { get; set; }
        public List<ClientMatch> Matches { get; set; }
        public List<ClientRobotEvent> RobotEvents { get; set; }
    }
}