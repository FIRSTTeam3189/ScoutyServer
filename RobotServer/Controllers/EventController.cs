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
            BlueAllianceClient refresher = new BlueAllianceClient();

            var events = await refresher.GetEvents(request.Year);

            var dbEvents = context.Events.ToList();

            foreach(var e in dbEvents) {
                var ev = events.FirstOrDefault(x => x.Id == e.Id);
                if(ev != null) {
                    e.Location = ev.Location;
                    e.Matchs = ev.Matchs;
                    e.EndDate = ev.EndDate;
                    e.EventCode = ev.EventCode;
                    e.Official = ev.Official;
                    e.Year = ev.Year;
                    e.Website = ev.Website;
                    e.StartDate = ev.StartDate;
                    events.Remove(ev);
                } else {
                    context.Events.Remove(e);
                }
            }

            foreach(var e in events) {
                context.Events.Add(e);
            }
            context.SaveChanges();
            var list = context.Events.ToList();
            return (from e in list
                    select new ClientEvent() {
                        StartDate = e.StartDate,
                        Official = e.Official,
                        Year = e.Year,
                        EndDate = e.EndDate,
                        EventCode = e.EventCode,
                        Location = e.Location,
                        Website = e.Website
                    }).ToList();
        }

        [Route("GetTeams")]
        [ActionName("GetTeams")]
        [Authorize]
        [HttpPost]
        public async Task<List<ClientTeam>> GetTeams(EventTeamsRequest request) {
            BlueAllianceClient refresher = new BlueAllianceClient();
            var teams = await refresher.GetEventTeams(request.Year, request.EventCode);
            var even = context.Events.Include(x => x.Teams).ThenInclude(c => c.Team).Where(x => x.EventCode == request.EventCode && x.Year == request.Year)
                .FirstOrDefault();
            if(even == null) {
                await Refresh(new RefreshEventRequest() { Year = request.Year });
                even = context.Events.Where(x => x.EventCode == request.EventCode && x.Year == request.Year)
                    .FirstOrDefault();
            }
            if(even != null) {
                foreach(var team in teams) {
                    await TeamController.GetTeam(team.TeamNumber, context);
                    TeamEvent et = new TeamEvent() {
                        Event = even,
                        Team = team
                    };
                    even.Teams.Add(et);
                }
                context.SaveChanges();
                //System.Diagnostics.Trace.TraceError("asdf" + teamss.Count);

                return even.Teams.Select(x => x.Team.GetClientTeam()).ToList();
            } else {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
        }

        [Route("GetMatches")]
        [ActionName("GetMatches")]
        [Authorize]
        [HttpPost]
        public async Task<List<ClientMatch>> GetMatchs(EventMatchesRequest request) {
            BlueAllianceClient refresher = new BlueAllianceClient();

            var matchs = await refresher.GetEventMatches(request.Year, request.EventCode);
            var even = context.Events.Where(x => x.EventCode == request.EventCode && x.Year == request.Year)
                .FirstOrDefault();
            if(even == null) {
                await Refresh(new RefreshEventRequest() { Year = request.Year });
                even = context.Events.Where(x => x.EventCode == request.EventCode && x.Year == request.Year)
                    .FirstOrDefault();
            }
            if(even != null) {
                if(matchs != null) {
                    var matcheses = context.Matches.Where(x => x.EventId == even.Id).ToList();
                    foreach(var match in matcheses) {
                        context.Matches.Remove(match);
                    }
                    even.Matchs = new List<Match>();
                    foreach(var match in matchs) {
                        if(!even.Matchs.Contains(match)) {
                            match.EventId = even.Id;
                            even.Matchs.Add(match);
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