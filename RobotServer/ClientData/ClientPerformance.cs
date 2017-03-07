using RobotServer.SQLDataObjects;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotServer.ClientData
{
    public class ClientPerformance {

        public string MatchId { get; set; }
        public int TeamNumber { get; set; }
        public Team Team { get; set; }
        public Match Match { get; set; }
        public AllianceColor Color { get; set; }
    }

    public enum AllianceColor {
        Red,
        Blue
    }
}