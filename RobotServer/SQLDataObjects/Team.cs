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

        public ClientTeam GetClientTeam()
        {
            return new ClientTeam()
            {
                TeamNumber = TeamNumber,
                RookieYear = RookieYear,
                NickName = NickName,
                TeamLocation = TeamLocation
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