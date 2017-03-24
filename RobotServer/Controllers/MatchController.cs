using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotServer.ClientData;
using RobotServer.Models;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using RobotServer.SQLDataObjects;

namespace ScoutingServer.Controllers
{

    public class MatchController : ApiController
    {

        private readonly RoboContext context;
        private readonly ILogger logger;

        public MatchController(ILoggerFactory loggerFactory, RoboContext context)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger("Performance");
        }

        [Route("GetPerformances")]
        [ActionName("GetPerformances")]
        [AllowAnonymous]
        [HttpGet]
        public List<ClientPerformance> GetPerformances(string EventCode)
        {
            return context.Performances.Where(x => x.Match.Event.EventCode == EventCode).ToList().Select(x => x.getClient()).ToList();
        }

        [Route("PutMatches")]
        [ActionName("PutMatches")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult PutMatches(List<ClientMatch> cms)
        {
            foreach (var cm in cms)
            {
                if (!context.Matches.Any(a => a.MatchId == cm.MatchId))
                {
                    context.Matches.Add(new Match(cm));
                    context.SaveChanges();
                }
            }
            return Ok();
        }

        [Route("GetMatches")]
        [ActionName("GetMatches")]
        [AllowAnonymous]
        [HttpGet]
        public List<ClientMatch> GetMatches(string EventId)
        {
            return context.Matches.Where(x => x.MatchId.Contains($"{EventId}")).ToList().Select(x => x.GetClientMatch()).ToList();
        }
    }
}
