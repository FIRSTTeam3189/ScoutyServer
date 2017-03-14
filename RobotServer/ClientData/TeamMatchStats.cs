using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.ClientData
{
    public class TeamMatchStat
    {
        public string MatchId { get; set; }
        public List<int> Teams { get; set; }
        public List<ClientRobotEvent> Events { get; set; }
        public TeamMatchStat() { }
        public TeamMatchStat(string matchId, IEnumerable<int> teams, IEnumerable<ClientRobotEvent> robotEvents) {
            MatchId = matchId;
            Teams = teams.ToList();
            Events = robotEvents.ToList();
        }
    }
}
