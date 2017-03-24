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

        [Route("PutPerformances")]
        [ActionName("PutPerformances")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult PutPerformances(List<ClientPerformance> cms)
        {
            foreach (var cm in cms) {
                if (!context.Performances.Any(a => a.MatchId == cm.MatchId && a.TeamNumber == cm.TeamNumber))
                {
                    context.Performances.Add(new Performance(cm));
                    context.SaveChanges();
                }
            }
            return Ok();
        }

        [Route("GetPerformances")]
        [ActionName("GetPerformances")]
        [AllowAnonymous]
        [HttpGet]
        public List<ClientPerformance> GetPerformances(string EventCode) {
            return context.Performances.Where(x => x.Match.Event.EventCode == EventCode).ToList().Select(x => x.getClient()).ToList();
        }
    }
}