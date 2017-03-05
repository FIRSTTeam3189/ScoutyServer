﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotServer.SQLDataObjects
{
    public class ClientMatch
    {
        public MatchType MatchType { get; set; }
        public int MatchNumber { get; set; }
        public string EventCode { get; set; }
        public int RedOne { get; set; }
        public int RedTwo { get; set; }
        public int RedThree { get; set; }
        public int BlueOne { get; set; }
        public int BlueTwo { get; set; }
        public int BlueThree { get; set; }
        public int Time { get; set; }
    }
}