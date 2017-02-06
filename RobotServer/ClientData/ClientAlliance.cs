using RobotServer.ClientData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotServer.SQLDataObjects
{
    public class ClientAlliance
    {
        public virtual ClientPerformance TeamOne { get; set; }

        public virtual ClientPerformance TeamTwo { get; set; }

        public virtual ClientPerformance TeamThree { get; set; }
    }
}