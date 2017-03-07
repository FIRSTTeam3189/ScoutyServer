using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RobotServer.ClientData;
using RobotServer.Interfaces;
using RobotServer.Models;
using RobotServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BlueAllianceClient;

namespace ScoutingServer.Controllers {

    [Route("api/events")]
    public class EventController : Controller {

        private readonly RoboContext context;
        private readonly ILogger logger;

        public EventController(ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("Event");
        }

        [Route("Refresh")]
        [ActionName("Refresh")]
        [Authorize]
        [HttpPost]
        public async Task<List<ClientEvent>> Refresh(RefreshEventRequest request) {
            BlueAllianceContext refresher = new BlueAllianceContext();

            var events = await refresher.GetEvents(request.Year);

            var dbEvents = context.Events.ToList();

            foreach(var e in dbEvents) {
                var ev = events.FirstOrDefault(x => x.Key == e.EventId);
                if(ev != null) {
                    e.Location = ev.Location;
                    e.EventCode = ev.EventCode;
                    events.Remove(ev);
                } else {
                    context.Events.Remove(e);
                }
            }

            foreach(var e in events) {
                context.Events.Add(new Event(e));
            }

            context.SaveChanges();
            var list = context.Events.ToList();
            return list.Select(x => x.ClientEvent()).ToList();
        }

        [Route("GetTeams")]
        [ActionName("GetTeams")]
        [Authorize]
        [HttpPost]
        public async Task<List<ClientTeam>> GetTeams(EventTeamsRequest request) {
            BlueAllianceContext refresher = new BlueAllianceContext();
            var teams = (await refresher.GetEvent(request.Year, request.EventCode)).Teams;
            var even = context.Events.Include(x => x.TeamEvents).ThenInclude(c => c.Team).Where(x => x.EventCode == request.EventCode && x.GetYear() == request.Year)
                .FirstOrDefault();
            if(even == null) {
                await Refresh(new RefreshEventRequest() { Year = request.Year });
                even = context.Events.Where(x => x.EventCode == request.EventCode && x.GetYear() == request.Year)
                    .FirstOrDefault();
            }
            if(even != null) {
                foreach(var team in teams) {
                    await TeamController.GetTeam(team.TeamNumber, context);
                    TeamEvent et = new TeamEvent() {
                        Event = even,
                        Team = new SQLDataObjects.Team(team)
                    };
                    even.TeamEvents.Add(et);
                }
                context.SaveChanges();
                //System.Diagnostics.Trace.TraceError("asdf" + teamss.Count);

                return even.Teams.Select(x => x.GetClientTeam()).ToList();
            } else {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
        }

        [Route("GetMatches")]
        [ActionName("GetMatches")]
        [Authorize]
        [HttpPost]
        public async Task<List<ClientMatch>> GetMatchs(EventMatchesRequest request) {
            BlueAllianceContext refresher = new BlueAllianceContext();

            var matchs = (await refresher.GetEvent(request.Year, request.EventCode)).Matches;
            var even = context.Events.Where(x => x.EventCode == request.EventCode && x.GetYear() == request.Year)
                .FirstOrDefault();
            if(even == null) {
                await Refresh(new RefreshEventRequest() { Year = request.Year });
                even = context.Events.Where(x => x.EventCode == request.EventCode && x.GetYear() == request.Year)
                    .FirstOrDefault();
            }
            if(even != null) {
                if(matchs != null) {
                    var matcheses = context.Matches.Where(x => x.EventId == even.EventId).ToList();
                    foreach(var match in matcheses) {
                        context.Matches.Remove(match);
                    }
                    even.Matchs = new List<Match>();
                    foreach(var match in matchs) {
                        if(!even.Matchs.Contains(new Match(match))) {
                            even.Matchs.Add(new Match(match));
                        }
                    }
                }
                
                context.SaveChanges();
                //System.Diagnostics.Trace.TraceError("bwoasdtch");
                if(even.Matchs != null) {
                    return even.Matchs.Select(x => x.GetClientMatch()).ToList();
                } else {
                    throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
                }
            } else {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}