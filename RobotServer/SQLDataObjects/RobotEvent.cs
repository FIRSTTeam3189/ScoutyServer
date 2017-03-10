using RobotServer.ClientData;
using ScoutingServer.SQLDataObjects;

namespace RobotServer.SQLDataObjects {
    public class RobotEvent {
        public string RobotEventId { get; set; }
        public int TeamNumber { get; set; }
        public string MatchId { get; set; }
        public Match Match { get; set; }
        public Team Team { get; set; }
        public ActionType Action { get; set; }
        public ActionPeriod Period { get; set; }
        public int Time { get; set; }
        public string PosterId { get; set; }
        public Account Poster { get; set; }

        public ClientRobotEvent getClient() {
            return new ClientRobotEvent() {
                Action = Action,
                MatchId = MatchId,
                Period = Period,
                TeamId = TeamNumber,
                Time = Time
            };
        }

        public static RobotEvent FromClient(ClientRobotEvent re, string posterId = "Unknown") {
            return new RobotEvent() {
                Action = re.Action,
                MatchId = re.MatchId,
                Period = re.Period,
                PosterId = posterId,
                TeamNumber = re.TeamId,
                Time = re.Time
            };
        }
    }

    public enum EventTime {
        Auto = 1,
        Teleop = 2,
        Final = 3
    }

    public enum EventType {
        // Making Goals
        MakeLow = 1,
        MakeLowUnderPressure = 2,
        MakeHigh = 3,
        MakeHighUnderPressure = 4,

        // Missing Goals
        MissLow = 5,
        MissLowUnderPressure = 6,
        MissHigh = 7,
        MissHighUnderPressure = 8,

        // Crossing Defenses
        CrossLowBar = 9,
        CrossPortcullis = 10,
        CrossChevalDeFrise = 11,
        CrossMoat = 12,
        CrossRamparts = 13,
        CrossSallyPort = 14,
        CrossDrawBridge = 15,
        CrossRockWall = 16,
        CrossRoughTerrain = 17,

        // Assisting Defenses
        AssistPortcullis = 18,
        AssistChevalDeFrise = 19,
        AssistMoat = 20,
        AssistRamparts = 21,
        AssistSallyPort = 22,
        AssistDrawBridge = 23,
        AssistRockWall = 24,
        AssistRoughTerrain = 25,
        AssistLowBar = 26,
        AssistedCross = 27,

        // Other Offensive Categories
        ReachDefense = 28,
        Challenge = 29,
        Hang = 30,
        FailedHang = 31,

        // Robot Bad Things
        Foul = 32,
        TechnicalFoul = 33,
        RobotFailure = 34,

        // Block Shots
        BlockedShotOne = 35,
        BlockedShotTwo = 36,
        BlockedShotThree = 37,
        FailedBlockShotOne = 38,
        FailedBlockShotTwo = 39,
        FailedBlockShotThree = 40,
        StealBall = 41,
    }
}