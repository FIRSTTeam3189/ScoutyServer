using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using ScoutingServer.Models;
using ScoutingServer.SQLDataObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotServer.Models;
using Microsoft.AspNetCore.Authorization;
using RobotServer.ClientData;
using RobotServer.Interfaces;

namespace ScoutingServer.Controllers {

    [Route("api/teams")]
    public class TeamController : Controller {

        private readonly RoboContext context;
        private readonly ILogger logger;

        public TeamController(ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("Team");
        }

        [Route("GetTeam")]
        [ActionName("GetTeam")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ClientTeam> GetTeam(TeamInfoRequest request) {
            return (await GetTeam(request.TeamNumber, context)).GetClientTeam();
        }

        public static async Task<Team> GetTeam(int TeamNumber, RoboContext context) {

            Team team = context.Teams.FirstOrDefault(a => a.TeamNumber == TeamNumber);

            if(team == null) {

                BlueAllianceClient client = new BlueAllianceClient();

                team = await client.GetTeam(TeamNumber);
                if(team != null) {
                    context.Teams.Add(team);
                    context.SaveChanges();
                } else {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }

            return team;
        }
    }
}