using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RobotServer.SQLDataObjects.DataSheet;

namespace RobotServer.ClientData
{
    public class ClientDataSheet
    {
        public string Drivetrain { get; set; }
        public string Autonomous { get; set; }
        public string RobotSpeed { get; set; }
        public string ClimbSpeed { get; set; }
        public ExLevel DriverEx { get; set; }
        public ExLevel CoDriverEx { get; set; }
        public ExLevel CoachEx { get; set; }
        public ExLevel HumanPlayer { get; set; }
        public int ExpectedGears { get; set; }
        public int ExpectedBalls { get; set; }
        public List<ClientNote> Notes { get; set; }
        public List<String> Pictures { get; set; }
        public int TeamNumber { get; set; }
        public int Year { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
