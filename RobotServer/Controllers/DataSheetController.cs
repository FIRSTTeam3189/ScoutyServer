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
using RobotServer.SQLDataObjects;

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
        [HttpGet]
        public void GetDataSheet(int teamNumber) {
            context.DataSheets.FirstOrDefault(x => x.TeamNumber == teamNumber && x.Year == 2017);
        }

        [Route("PutDataSheet")]
        [ActionName("PutDataSheet")]
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage PutDataSheet(ClientDataSheet request)
        {
            var temp = context.DataSheets.Where(x => x.TeamNumber == request.TeamNumber && x.Year == request.Year);
            if (temp.Count() <= 0) {
                context.DataSheets.Add(DataSheet.GetDataSheet(request));
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            var real = temp.FirstOrDefault();
            List<Note> notes = new List<Note>();
            foreach (var n in request.Notes)
            {
                var asdf = real.Notes.Where(x => x.Id == n.Id);
                if(asdf.Count() <= 0){
                    Note s = Note.GetNote(n);
                    s.DataSheetId = real.Id;
                    real.Notes.Add(s);
                    continue;
                }
                var note = asdf.FirstOrDefault();
                note.Data = n.Data;
                note.PerformenceId = n.PerformenceId;
                note.URI = n.URI;
            }
            real.Autonomous = request.Autonomous;
            real.ClimbSpeed = request.ClimbSpeed;
            real.CoachEx = request.CoachEx;
            real.CoDriverEx = request.CoDriverEx;
            real.DriverEx = request.DriverEx;
            real.Drivetrain = request.Drivetrain;
            real.ExpectedBalls = request.ExpectedBalls;
            real.ExpectedGears = request.ExpectedGears;
            real.HumanPlayer = request.HumanPlayer;
            real.RobotSpeed = request.RobotSpeed;

            context.DataSheets.Update(real);
            context.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}