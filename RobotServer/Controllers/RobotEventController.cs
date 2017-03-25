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
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace RobotServer.Controllers
{
    [Route("api/[Controller]")]
    public class RobotEventController : Controller {
        private readonly RoboContext context;
        private readonly ILogger logger;
        private readonly UserManager<Account> userManager;

        public RobotEventController(ILoggerFactory loggerFactory, RoboContext context, UserManager<Account> um) {
            this.userManager = um;
            this.context = context;
            logger = loggerFactory.CreateLogger("RobotEvent");
        }

        [Authorize]
        [HttpPost("Sheets")]
        public async Task<IActionResult> Sheets([FromBody]List<ClientDataSheet> requests) {
            foreach (var s in requests) {
                logger.LogInformation($"{s.DirtyBoy}");
            }

            foreach (var sheet in requests) {
                var dbSheet = await context.DataSheets.Include(x => x.Notes).FirstOrDefaultAsync(x => x.TeamNumber == sheet.TeamNumber && x.Year == sheet.Year);
                if (dbSheet == null) {
                    await context.DataSheets.AddAsync(DataSheet.GetDataSheet(sheet));
                    continue;
                }

                // Add new notes and stuff to datasheet
                if (dbSheet.Notes == null)
                    dbSheet.Notes = new List<Note>();
                dbSheet.Notes.AddRange(sheet.Notes.Select(x => Note.GetNote(x)));

                if (sheet.DirtyBoy) {
                    // Copy over attributes
                    dbSheet.Autonomous = sheet.Autonomous;
                    dbSheet.ClimbSpeed = sheet.ClimbSpeed;
                    dbSheet.CoachEx = sheet.CoachEx;
                    dbSheet.CoDriverEx = sheet.CoDriverEx;
                    dbSheet.DriverEx = sheet.DriverEx;
                    dbSheet.Drivetrain = sheet.Drivetrain;
                    dbSheet.ExpectedBalls = sheet.ExpectedBalls;
                    dbSheet.ExpectedGears = sheet.ExpectedGears;
                    dbSheet.HumanPlayer = sheet.HumanPlayer;
                    dbSheet.Year = sheet.Year;
                }

                context.DataSheets.Update(dbSheet);
            }

            await context.SaveChangesAsync();

            return Ok();
            /*
            logger.LogError("Fuck you you piece of shit I hope you die in a mother fucking fire. Fucking work already I hope you have a fucking horrible day");
            foreach (var request in requests)
            {
                var temp = context.DataSheets.Where(x => x.TeamNumber == request.TeamNumber && x.Year == request.Year).Include(x => x.Notes);
                if (temp.Count() <= 0)
                {
                    context.DataSheets.Add(DataSheet.GetDataSheet(request));
                    context.SaveChanges();
                }
                var real = context.DataSheets.Where(x => x.TeamNumber == request.TeamNumber && x.Year == request.Year).Include(x => x.Notes).First();
                List<Note> notes = new List<Note>();
                if (real.Notes == null)
                    real.Notes = new List<Note>();
                foreach (var n in request.Notes)
                {
                    var asdf = real.Notes.Where(x => x.Id == n.Id);
                    if (asdf.Count() <= 0)
                    {
                        Note s = Note.GetNote(n);
                        s.DataSheetId = real.Id;
                        real.Notes.Add(s);
                        continue;
                    }
                    var note = real.Notes.Where(x => x.Id == n.Id).FirstOrDefault();
                    note.Data = n.Data;
                    note.PerformenceId = n.PerformenceId;
                    note.URI = n.URI;
                    context.Notes.Update(note);
                }
                if (request.DirtyBoy)
                {
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
                }


                context.SaveChanges();
            }
            return Ok();

            return Ok();*/
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult DoTheThing([FromBody]List<ClientDataSheet> requests)
        {
            logger.LogError("Fuck you you piece of shit I hope you die in a mother fucking fire. Fucking work already I hope you have a fucking horrible day");
            foreach (var request in requests)
            {
                var temp = context.DataSheets.Where(x => x.TeamNumber == request.TeamNumber && x.Year == request.Year);
                if (temp.Count() <= 0)
                {
                    context.DataSheets.Add(DataSheet.GetDataSheet(request));
                    context.SaveChanges();
                }
                var real = temp.FirstOrDefault();
                List<Note> notes = new List<Note>();
                foreach (var n in request.Notes)
                {
                    var asdf = real.Notes.Where(x => x.Id == n.Id);
                    if (asdf.Count() <= 0)
                    {
                        Note s = Note.GetNote(n);
                        s.DataSheetId = real.Id;
                        real.Notes.Add(s);
                        continue;
                    }
                    var note = asdf.FirstOrDefault();
                    note.Data = n.Data;
                    note.PerformenceId = n.PerformenceId;
                    note.URI = n.URI;
                    context.Notes.Update(note);
                }
                if (request.DirtyBoy)
                {
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
                }


                context.SaveChanges();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("Post")]
        public async Task<IActionResult> PostRobotEvents([FromBody] List<ClientRobotEvent> requests) {
            var user = await userManager.GetUserAsync(HttpContext.User);
            logger.LogInformation($"Username is: {user?.UserName}");

            if(!requests.Any())
                throw new HttpResponseException(System.Net.HttpStatusCode.NoContent);

            var temp = new List<string>();
            foreach (var request in requests)
            {
                var thing = request.MatchId.Substring(0, request.MatchId.IndexOf('_'));
                if (temp.Contains(thing)) {
                    await EventController.GetEvent(logger, context, thing, int.Parse(thing.Substring(0, 4)));
                    temp.Add(thing);
                }
            }
            context.RobotEvents.AddRange(requests.Select(r => RobotEvent.FromClient(r, user)));

            context.SaveChanges();

            return Ok();
        }
    }
}
