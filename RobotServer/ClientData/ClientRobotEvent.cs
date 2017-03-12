using RobotServer.SQLDataObjects;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RobotServer.ClientData {
    public class ClientRobotEvent {
        public int TeamId { get; set; }
        public string MatchId { get; set; }
        public ActionType Action { get; set; }
        public ActionPeriod Period { get; set; }
        public int Time { get; set; }
    }

    public enum ActionType {
        Mobility,
        MakeHigh,
        MakeLow,
        MissHigh,
        MissLow,
        GearCollected,
        GearPickedUp,
        GearDropped,
        GearHung,
        ClimbAttempted,
        ClimbSuccessful,
        RobotDisabled,
        SpilledBalls,
        Foul,
        TechnicalFoul
    }

    public enum ActionPeriod {
        Auto,
        Teleop
    }
}