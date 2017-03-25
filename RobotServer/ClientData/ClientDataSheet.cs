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
        public double RobotSpeed { get; set; }
        public double ClimbSpeed { get; set; }
        public ExLevel DriverEx { get; set; }
        public ExLevel CoDriverEx { get; set; }
        public ExLevel CoachEx { get; set; }
        public ExLevel HumanPlayer { get; set; }
        public double ExpectedGears { get; set; }
        public double ExpectedBalls { get; set; }
        public List<ClientNote> Notes { get; set; }
        public int TeamNumber { get; set; }
        public int Year { get; set; }
        public bool DirtyBoy { get; set; }
    }
}
