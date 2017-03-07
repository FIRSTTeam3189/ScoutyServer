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
    }
}