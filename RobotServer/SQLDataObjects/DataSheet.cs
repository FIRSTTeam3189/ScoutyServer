using RobotServer.ClientData;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.SQLDataObjects
{
    public class DataSheet {
        [Key]
        public string Id { get; set; }
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
        public List<Note> Notes { get; set; }
        public int TeamNumber { get; set; }
        public Team Team { get; set; }
        public int Year { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        public DataSheet()
        {

        }

        public static DataSheet GetDataSheet(ClientDataSheet data)
        {
            return new DataSheet()
            {
                Autonomous = data.Autonomous,
                ClimbSpeed = data.ClimbSpeed,
                CoachEx = data.CoachEx,
                CoDriverEx = data.CoDriverEx,
                DriverEx = data.DriverEx,
                Drivetrain = data.Drivetrain,
                ExpectedBalls = data.ExpectedBalls,
                ExpectedGears = data.ExpectedGears,
                HumanPlayer = data.HumanPlayer,
                Notes = data.Notes.Select(x => Note.GetNote(x)).ToList(),
                RobotSpeed = data.RobotSpeed,
                TeamNumber = data.TeamNumber,
                Year = data.Year
            };
        }

        public enum ExLevel {
            NA,
            Little,
            Some,
            OneYear,
            TwoYears,
            ThreeYears,
            FourToSixYears,
            SevenPlusYears
        }
    }
}
