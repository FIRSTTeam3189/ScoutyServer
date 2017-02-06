using System;
using System.Collections.Generic;
using System.Linq;
using ScoutingServer.SQLDataObjects;

namespace RobotServer.ClientData
{
    public class ClientTeam
    {
        public int TeamNumber { get; set; }

        public int RookieYear { get; set; }

        public string NickName { get; set; }

        public string TeamLocation { get; set; }

        public ICollection<ClientPerformance> TeamPerformance { get; set; }

    }
}