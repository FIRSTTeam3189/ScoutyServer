using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobotServer.Models;
using ScoutingServer.SQLDataObjects;
using Microsoft.Extensions.Logging;

namespace RobotServer.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly RoboContext context;
        private readonly ILogger logger;

        public HomeController(ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("Home");
        }

        [HttpGet]
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("About")]
        public IActionResult About()
        {
            //ViewData["Message"] = "Your application description page.";

            Team team;
            team = context.Teams.FirstOrDefault();

            ViewData["Message"] = team.NickName;
            logger.LogInformation(team.NickName);
            context.SaveChanges();
            return View();
        }

        [HttpGet("Contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
