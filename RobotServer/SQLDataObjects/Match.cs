using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RobotServer.SQLDataObjects
{
    public class Match
    {
        public string Id { get; set; }
        public MatchType MatchType { get; set; }
        public int MatchNubmer { get; set; }
        public string eventid { get; set; }
        public virtual int RedOne { get; set; }
        public virtual int RedTwo { get; set; }
        public virtual int RedThree { get; set; }
        public virtual int BlueOne { get; set; }
        public virtual int BlueTwo { get; set; }
        public virtual int BlueThree { get; set; }
        public int time { get; set; }

        public ClientMatch GetClientMatch() {
            return new ClientMatch() {
                MatchType = MatchType,
                MatchNumber = MatchNubmer,
                BlueOne = BlueOne,
                BlueTwo = BlueTwo,
                BlueThree = BlueThree,
                RedOne = RedOne,
                RedTwo = RedTwo,
                RedThree = RedThree,
                Time = time
            };
        }

        public static bool operator ==(Match a, Match b) {
            return a?.Id == b?.Id;
        }

        public static bool operator !=(Match a, Match b) {
            return a?.Id != b?.Id;
        }

        public override bool Equals(object obj) {
            if(obj is Account)
                return (obj as Account)?.Id == Id;
            return false;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

    }

    public enum MatchType {
        Practice = 1,
        Qualification = 2,
        OctoFinal = 3,
        QuarterFinal = 4,
        SemiFinal = 5,
        Final = 6
    }
}