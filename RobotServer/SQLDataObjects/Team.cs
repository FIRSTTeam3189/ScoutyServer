using RobotServer.ClientData;
using RobotServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ScoutingServer.SQLDataObjects
{
    public class Team
    {
        public string Id { get; set; }
        public int RookieYear { get; set; }

        public string NickName { get; set; }
  
        public string TeamLocation { get; set; }

        public int TeamNumber { get; set; }

        public virtual List<Performance> TeamPerformance { get; set; }

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
            return a?.Id == b?.Id;
        }

        public static bool operator !=(Team a, Team b) {
            return a?.Id != b?.Id;
        }

        public override bool Equals(object obj) {
            if(obj is Account)
                return (obj as Account)?.Id == Id;
            return false;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}