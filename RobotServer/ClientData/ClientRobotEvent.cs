using RobotServer.SQLDataObjects;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RobotServer.ClientData {
    public class ClientRobotEvent {
        public int TeamId { get; set; }
        public int MatchId { get; set; }
        public ActionType Action { get; set; }
        public ActionPeriod Period { get; set; }
        public int Time { get; set; }
    }

    public enum ActionType {
        MakeHigh,
        MakeLow,
        MissHigh,
        MissLow,
        GearCollected,
        GearDropped,
        GearHung,
        ClimbAttempted,
        ClimbSuccessful,
        RobotDisabled,
        SpilledBalls
    }

    public enum ActionPeriod {
        Auto,
        Teleop
    }
}