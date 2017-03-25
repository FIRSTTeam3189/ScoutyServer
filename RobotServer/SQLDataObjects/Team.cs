using BlueAllianceClient;
using RobotServer.ClientData;
using RobotServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ScoutingServer.SQLDataObjects
{
    public class Team
    {
        public int RookieYear { get; set; }
        public string NickName { get; set; }
        public string TeamLocation { get; set; }
        [Key]
        public int TeamNumber { get; set; }
        public virtual List<Performance> Performances { get; set; }
        public List<TeamEvent> Events { get; set; }
        public List<RobotEvent> RobotEvents { get; set; }

        public Team() {

        }

        public Team(BATeam team) {
            NickName = team.Name;
            TeamNumber = team.TeamNumber;
            TeamLocation = team.Location;
        }

        public Team(ClientTeam team)
        {
            NickName = team.Name;
            TeamNumber = team.TeamNumber;
        }

        public ClientTeam GetClientTeam()
        {
            return new ClientTeam()
            {
                TeamNumber = TeamNumber,
                RookieYear = RookieYear,
                Name = NickName
            };
        }

        public static bool operator ==(Team a, Team b) {
            return a?.TeamNumber == b?.TeamNumber;
        }

        public static bool operator !=(Team a, Team b) {
            return a?.TeamNumber != b?.TeamNumber;
        }

        public override bool Equals(object obj) {
            if(obj is Team)
                return (obj as Team)?.TeamNumber == TeamNumber;
            return false;
        }

        public override int GetHashCode() {
            return TeamNumber.GetHashCode();
        }
    }
}