using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.Interfaces
{
    public class TeamStatRequest
    {
        public int TeamNumber { get; set; }
        public string EventId { get; set; }
    }
}
