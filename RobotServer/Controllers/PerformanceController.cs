using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotServer.ClientData;
using RobotServer.Models;
using RobotServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScoutingServer.Controllers {

    [Route("api/performances")]
    public class PerformanceController : Controller {

        private readonly RoboContext context;
        private readonly ILogger logger;

        public PerformanceController(ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("Performance");
        }

        [Route("GetPerformances")]
        [ActionName("GetPerformances")]
        [Authorize]
        [HttpGet]
        public List<ClientPerformance> GetPerformances(string EventCode) {
            return context.Performances.Where(x => x.Match.Event.EventCode == EventCode).ToList().Select(x => x.getClient()).ToList();
        }

        [Route("PostPerformances")]
        [ActionName("PostPerformances")]
        [Authorize]
        [HttpPost]
        public void PostPerformance(List<ClientPerformance> stuff) {
            
            foreach(var thing in stuff) {
                string id = thing.EventCode + thing.MatchNumber + thing.TeamId + thing.MatchType;
                var perf = context.Performances.Where(
                    x => x.Id == id).FirstOrDefault();
                if(perf == null) {
                    context.Performances.Add(new Performance() {
                        MatchTyp = thing.MatchType,
                        Match = context.Matches.FirstOrDefault(x => x.MatchNubmer == thing.MatchNumber),
                        Team = context.Teams.FirstOrDefault(x => x.TeamNumber == thing.TeamId),
                        Id = thing.EventCode + thing.MatchNumber + thing.TeamId + thing.MatchType,
                        Events = thing.Events.Select(x => new RobotEvent() {
                            EventTime = x.EventTime,
                            EventType = x.EventType,
                            Id = Guid.NewGuid().ToString()
                        }).ToList()
                    });
                }else if(perf.UpdatedAt < thing.LastUpdated) {
                    //System.Diagnostics.Trace.TraceError("asdf");
                    perf.MatchTyp = thing.MatchType;
                    
                    perf.Id = id;
                    perf.Events = thing.Events.Select(x => new RobotEvent() {
                        EventTime = x.EventTime,
                        EventType = x.EventType,
                        Id = Guid.NewGuid().ToString()
                    }).ToList();
                    perf.UpdatedAt = thing.LastUpdated;
                }
                context.SaveChanges();
            }
        }

        
    }
}