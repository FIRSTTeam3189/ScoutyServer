using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotServer.Interfaces {
    public class EventTeamsRequest {
        public string EventId { get; set; }
        public int Year { get; set; }
    }
}