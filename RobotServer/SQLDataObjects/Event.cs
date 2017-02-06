using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;

namespace RobotServer.SQLDataObjects
{
    public class Event
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int Year { get; set; }

        public string Website { get; set; }

        public string EventCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool Official { get; set; }

        public List<Team> Teams { get; set; }

        public List<Match> Matchs { get; set; }

        public ClientData.ClientEvent GetClientEvent() {
            return new ClientData.ClientEvent {
                EndDate = EndDate,
                EventCode = EventCode,
                Location = Location,
                Official = Official,
                StartDate = StartDate,
                Website = Website,
                Year = Year
            };
        }
    }
}