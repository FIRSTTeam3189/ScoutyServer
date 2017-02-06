using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotServer.Interfaces {
    public class CustomRegistrationRequest {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
    }
}