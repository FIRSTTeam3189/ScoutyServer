using BlueAllianceClient;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobotServer.SQLDataObjects
{
    public class Event
    {
        public string Name { get; set; }
        [Key]
        public string EventId { get; set; }
        public string EventCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; }
        public List<TeamEvent> TeamEvents { get; set; }
        public List<Team> Teams { get; set; }
        public List<Match> Matchs { get; set; }
        public int Week { get; set; }

        public Event(BAEvent ev) {
            EventId = ev.Key;
            Name = ev.Name;
            EventCode = ev.EventCode;
            Location = ev.Location;
            Week = Week;
        }

        public int GetYear() {
            return int.Parse(EventId.Substring(0,4));
        }

        public ClientData.ClientEvent ClientEvent() {
            return new ClientData.ClientEvent {
                EventId = EventId,
                Location = Location,
                Week = Week,
                Name = Name
            };
        }
    }
}