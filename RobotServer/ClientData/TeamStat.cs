using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.ClientData
{
    public class TeamStat
    {
        public TeamStat() { }
        public TeamStat(int teamNumber, string eventId, IEnumerable<TeamMatchStat> matchStats) { }
    }
}
