using BlueAllianceClient;
using RobotServer.ClientData;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RobotServer.SQLDataObjects
{
    public class Match {
        public string MatchId { get; set; }
        public string MatchInfo { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
        public List<RobotEvent> RobotEvents { get; set; }
        public List<Performance> Performances { get; set; }

        public Match() {

        }

        public Match(BAMatch match) {
            MatchId = match.Key;
            MatchInfo = match.MatchInfo();
            EventId = match.Event.Key;
        }

        public ClientMatch GetClientMatch() {
            return new ClientMatch() {
                MatchId = MatchId,
                EventId = EventId,
                MatchInfo = MatchInfo,
                RobotEvents = RobotEvents.Select(x => x.getClient()).ToList()
            };
        }

        public static bool operator ==(Match a, Match b) {
            return a?.MatchId == b?.MatchId;
        }

        public static bool operator !=(Match a, Match b) {
            return a?.MatchId != b?.MatchId;
        }

        public override bool Equals(object obj) {
            if(obj is Match)
                return (obj as Match)?.MatchId == MatchId;
            return false;
        }

        public override int GetHashCode() {
            return MatchId.GetHashCode();
        }

    }

    public enum MatchType {
        Practice = 1,
        Qualification = 2,
        OctoFinal = 3,
        QuarterFinal = 4,
        SemiFinal = 5,
        Final = 6
    }
}