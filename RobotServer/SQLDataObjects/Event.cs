using BlueAllianceClient;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public Event()
        {

        }

        public Event(BAEvent ev)
        {
            EventId = ev.Key;
            Name = ev.Name;
            EventCode = ev.EventCode;
            Location = ev.Location;
            Week = Week;

            if (ev.Teams != null)
            {
                TeamEvents = ev.Teams.Select(x => new TeamEvent()
                {
                    EventId = this.EventId,
                    TeamNumber = x.TeamNumber
                }).ToList();
                Teams = ev.Teams.Select(x => new Team(x)).ToList();
            }

            if (ev.Matches != null)
            {
                Matchs = ev.Matches.Select(x => new Match(x)).ToList();
            }
        }

        public int GetYear()
        {
            return int.Parse(EventId.Substring(0, 4));
        }

        public ClientData.ClientEvent ClientEvent()
        {
            return new ClientData.ClientEvent
            {
                EventId = EventId,
                Location = Location,
                Week = Week,
                Name = Name
            };
        }
    }
}