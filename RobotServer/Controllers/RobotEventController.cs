using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotServer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using RobotServer.ClientData;
using RobotServer.SQLDataObjects;
using ScoutingServer.Controllers;

namespace RobotServer.Controllers
{
    public class RobotEventController : Controller {
        private readonly RoboContext context;
        private readonly ILogger logger;

        public RobotEventController(ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("RobotEvent");
        }

        [Route("Post")]
        [ActionName("Post")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostRobotEvents(List<ClientRobotEvent> request) {
            var user = await AccountController.GetAccount(context, User, Request);

            foreach(var r in request) {
                if(context.RobotEvents.Any(y => y.MatchId == r.MatchId && y.PosterId != user.Id)) {
                    context.RobotEvents.Add(RobotEvent.FromClient(r, user.Id));
                }
            }

            context.SaveChanges();

            return Ok();
        }

        [Route("GetTeamStats")]
        [ActionName("GetTeamStats")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostRobotEvents() {
            //TODO DO SOMETHING HERE

            return Ok();
        }
    }
}
