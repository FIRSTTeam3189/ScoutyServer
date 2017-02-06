using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RobotServer.ClientData
{
    public class ClientEvent
    {

        public string Location { get; set; }

        public int Year { get; set; }

        public string EventCode { get; set; }

        public string Website { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool Official { get; set; }

    }
}