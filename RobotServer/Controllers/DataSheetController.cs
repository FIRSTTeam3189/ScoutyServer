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
using BlueAllianceClient;

namespace ScoutingServer.Controllers {

    [Route("api/[Controller]")]
    public class DataSheetController : Controller {

        private readonly RoboContext context;
        private readonly ILogger logger;

        public DataSheetController(ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("DataSheet");
        }

        [Route("GetDataSheet")]
        [ActionName("GetDataSheet")]
        [AllowAnonymous]
        [HttpPost]
        public void GetDataSheet(TeamInfoRequest request) {
            
        }

        [Route("PutDataSheet")]
        [ActionName("PutDataSheet")]
        [AllowAnonymous]
        [HttpPost]
        public void PutDataSheet(TeamInfoRequest request)
        {

        }
    }
}