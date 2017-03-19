﻿using ScoutingServer.SQLDataObjects;
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
        public string RobotSpeed { get; set; }
        public string ClimbSpeed { get; set; }
        public ExLevel DriverEx { get; set; }
        public ExLevel CoDriverEx { get; set; }
        public ExLevel CoachEx { get; set; }
        public ExLevel HumanPlayer { get; set; }
        public int ExpectedGears { get; set; }
        public int ExpectedBalls { get; set; }
        public List<Note> Notes { get; set; }
        public int TeamNumber { get; set; }
        public Team Team { get; set; }
        public int Year { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        public DataSheet()
        {

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