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
using System.Web.Http;

namespace RobotServer.Controllers
{
    [Route("api/[Controller]")]
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
        public async Task<IActionResult> PostRobotEvents([FromBody] List<ClientRobotEvent> request) {
            //var user = await AccountController.GetAccount(context, User, Request);
            if(!request.Any())
                throw new HttpResponseException(System.Net.HttpStatusCode.NoContent);              

            context.RobotEvents.AddRange(request.Select(r => RobotEvent.FromClient(r)));

            context.SaveChanges();

            return Ok();
        }

        [Route("GetTeamStats")]
        [ActionName("GetTeamStats")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetTeamStats() {
            //TODO DO SOMETHING HERE

            return Ok();
        }
    }
}
