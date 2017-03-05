using RobotServer.SQLDataObjects;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotServer.ClientData
{
    public class ClientPerformance {
        public string Id { get; set; }
        public int TeamId { get; set; }
        public string EventCode { get; set; }
        public int MatchNumber { get; set; }
        public MatchType MatchType { get; set; }
        public List<ClientRobotEvent> Events { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}