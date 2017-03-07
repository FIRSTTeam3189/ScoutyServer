using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace RobotServer.ClientData
{
    public class ClientEvent
    {

        public string Name { get; set; }
        public string EventId { get; set; }
        public string Location { get; set; }
        public List<ClientTeam> Teams { get; set; }
        public List<ClientMatch> Matches { get; set; }
        public int Week { get; set; }

    }
}