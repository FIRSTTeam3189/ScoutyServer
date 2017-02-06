using RobotServer.SQLDataObjects;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotServer.ClientData {
    public class ClientRobotEvent {
        public EventTime EventTime { get; set; }
        public EventType EventType { get; set; }
    }
}