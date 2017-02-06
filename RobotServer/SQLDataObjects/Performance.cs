using RobotServer.ClientData;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RobotServer.SQLDataObjects {
    public class Performance {
        public string Id { get; set; }
        public int TeamId { get; set; }
        public int MatchNumber { get; set; }
        public virtual MatchType MatchTyp { get; set; }
        public string EventCode { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
        public virtual List<RobotEvent> Events { get; set; }

        public ClientPerformance getClient() {
            return new ClientPerformance() {
                EventCode = EventCode,
                //Events = Events.Select(x => x.getClient()).ToList(),
                Id = Id,
                LastUpdated = UpdatedAt,
                MatchNumber = MatchNumber,
                MatchType = MatchTyp,
                TeamId = TeamId
            };
        }
    }
}