using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotServer.Interfaces {
    public class EventMatchesRequest {
        public string EventCode { get; set; }
        public int Year { get; set; }
    }
}
