using RobotServer.ClientData;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RobotServer.SQLDataObjects {
    public class Performance {
        public string Id { get; set; }
        public int TeamNumber { get; set; }
        public Team Team { get; set; }
        public string MatchId { get; set; }
        public Match Match { get; set; }
        public virtual MatchType MatchTyp { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
        public List<RobotEvent> Events { get; set; }

        public ClientPerformance getClient() {
            return new ClientPerformance() {
                //Events = Events.Select(x => x.getClient()).ToList(),
                Id = Id,
                LastUpdated = UpdatedAt,
                EventCode = Match.Event.EventCode,
                MatchNumber = Match.MatchNubmer,
                TeamId = TeamNumber,
                MatchType = MatchTyp
            };
        }
    }
}