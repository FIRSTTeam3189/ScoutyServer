using System;
using System.Linq;
using System.Collections.Generic;
using BlueAllianceClient;
using RobotServer.SQLDataObjects;
using ScoutingServer.SQLDataObjects;
using RobotServer.ClientData;

namespace BlueAllianceClient {
	public static class BlueAllianceExtensions
	{
		public static Event FromBAEvent(this BAEvent e) {
            return new Event(e);
		}

		public static Match FromBAMatch(this BAMatch match, Event ev) {
			return new Match
			{
				MatchInfo = match.MatchInfo(),
				Event = ev,
				EventId = ev.EventCode
			};
		}

		public static string MatchInfo(this BAMatch match) {
			return match.Level == "qm"
							? $"{match.Level}{match.MatchNumber}"
							: $"{match.Level}{match.SetNumber}m{match.MatchNumber}";
		}

		public static Team FromBATeam(this BATeam team) {
			return new Team { 
				NickName = string.IsNullOrWhiteSpace(team.Nickname) ? team.Name : team.Nickname,
				TeamNumber = team.TeamNumber,
				RookieYear = team.RookieYear ?? 1970,
			};
		}

		public static Performance PerformanceFromMatch(this Match match, Team team, AllianceColor color) {
			return new Performance
			{
				Color = color,
				Match = match,
				MatchId = match.MatchId,
				Team = team,
				TeamNumber = team.TeamNumber
			};
		}
	}
}
