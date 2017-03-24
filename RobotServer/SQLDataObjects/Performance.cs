using RobotServer.ClientData;
using ScoutingServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RobotServer.SQLDataObjects {
    public class Performance {
        public int TeamNumber { get; set; }
        public Team Team { get; set; }
        public string MatchId { get; set; }
        public Match Match { get; set; }
        public AllianceColor Color { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        public Performance()
        {

        }

        public Performance(ClientPerformance cp)
        {
            TeamNumber = cp.TeamNumber;
            MatchId = cp.MatchId;
            Color = cp.Color;
        }

        public ClientPerformance getClient() {
            return new ClientPerformance() {
                //Events = Events.Select(x => x.getClient()).ToList(),
                MatchId = MatchId,
                Color = Color,
                TeamNumber = TeamNumber
            };
        }
    }
}