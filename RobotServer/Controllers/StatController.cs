using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotServer.Models;
using Microsoft.Extensions.Logging;
using RobotServer.Interfaces;
using RobotServer.ClientData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RobotServer.Controllers
{
    [Route("api/[controller]")]
    public class StatController : Controller
    {
        public RoboContext context { get; }
        public ILogger logger { get; }
        public StatController(RoboContext context, ILoggerFactory factory) {
            this.context = context;
            this.logger = factory.CreateLogger<StatController>();
        }

        [HttpPost("TeamStats")]
        [ActionName("TeamStats")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<TeamStat> GetStatsForTeam([FromBody]TeamStatRequest request) {
            var eventMatches = await (from x in context.Matches.Where(x => x.EventId == request.EventId).Include(x => x.Performances)
                                      join e in context.RobotEvents.Where(ev => ev.TeamNumber == request.TeamNumber) on x.MatchId equals e.MatchId into eventGroup
                                      select new TeamMatchStat(x.MatchId, x.Performances.Select(p => p.TeamNumber), eventGroup.Select(mev => mev.getClient()))).ToListAsync();

            return new TeamStat(request.TeamNumber, request.EventId, eventMatches);
        }

        public async Task<List<TeamStat>> GetStats([FromBody]StatRequest request) {
            var teamEvents = await (from x in context.Matches
                                    .Where(x => x.EventId == request.EventId)
                                    .Include(x => x.RobotEvents)
                                    .SelectMany(x => x.RobotEvents)
                                     group x by x.TeamNumber into teamEvs
                                     select new { Team = teamEvs.Key, Events = teamEvs }).ToListAsync();
            var matches = await (from x in context.Matches
                                                  .Where(m => m.EventId == request.EventId)
                                                  .Include(m => m.Performances)
                                                  .SelectMany(m => m.Performances)
                                 group x by x.MatchId into match
                                 select new { Match = match.Key, Teams = match.Select(x => x.TeamNumber) })
                                  .ToDictionaryAsync(x => x.Match, x => x.Teams.ToList());
            var teamStats = teamEvents.Select(x => new TeamStat(x.Team, request.EventId, (from ev in x.Events
                                                                                          group ev by ev.MatchId into matchEvs
                                                                                          select new TeamMatchStat(matchEvs.Key,
                                                                                                                   matches[matchEvs.Key],
                                                                                                                   matchEvs.Select(e => e.getClient())))))
                                                                                                                   .ToList();
            return teamStats;
                                     
        }
    }
}
