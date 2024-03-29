﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RobotServer.ClientData;
using RobotServer.Interfaces;
using RobotServer.Models;
using RobotServer.SQLDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BlueAllianceClient;
using System.Net;

namespace ScoutingServer.Controllers {

    [Route("api/events")]
    public class EventController : Controller {

        private readonly RoboContext context;
        private readonly ILogger logger;

        public EventController(ILoggerFactory loggerFactory, RoboContext context) {
            this.context = context;
            logger = loggerFactory.CreateLogger("Event");
        }

        [Route("Refresh")]
        [ActionName("Refresh")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<List<ClientEvent>> Refresh([FromBody]RefreshEventRequest request) {
            BlueAllianceContext refresher = new BlueAllianceContext();

            var events = await refresher.GetEvents(request.Year);

            var dbEvents = context.Events.ToList();

            foreach(var e in dbEvents) {
                var ev = events.FirstOrDefault(x => x.Key == e.EventId);
                if(ev != null) {
                    e.Location = ev.Location;
                    e.EventId = ev.Key;
                    events.Remove(ev);
                } else {
                    context.Events.Remove(e);
                }
            }

            foreach(var e in events) {
                context.Events.Add(new Event(e));
            }

            context.SaveChanges();
            var list = context.Events.ToList();
            return list.Select(x => x.ClientEvent()).ToList();
        }

        [Route("GetTeams")]
        [ActionName("GetTeams")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<List<ClientTeam>> GetTeams(EventTeamsRequest request) {
            BlueAllianceContext refresher = new BlueAllianceContext();
            var teams = (await refresher.GetEvent(request.Year, request.EventId)).Teams;
            var even =
                context.Events.Include(x => x.TeamEvents)
                    .ThenInclude(c => c.Team)
                    .FirstOrDefault(x => x.EventId == request.EventId && x.GetYear() == request.Year);
            if(even == null) {
                await Refresh(new RefreshEventRequest() { Year = request.Year });
                even = context.Events.FirstOrDefault(x => x.EventId == request.EventId);
            }
            if(even != null) {
                foreach(var team in teams) {
                    TeamController.GetTeam(team.TeamNumber, context, team);
                    if(context.TeamEvents.Any(m => m.EventId == even.EventId && m.TeamNumber == team.TeamNumber)) {
                        TeamEvent et = new TeamEvent() {
                            Event = even,
                            Team = new SQLDataObjects.Team(team)
                        };
                        context.TeamEvents.Add(et);
                    }
                }
                context.SaveChanges();

                return even.Teams.Select(x => x.GetClientTeam()).ToList();
            } else {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
        }

        [Route("GetMatches")]
        [ActionName("GetMatches")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetMatchs([FromBody]EventMatchesRequest request) {
            if (GetEvent(logger,context,request.EventId,request.Year) == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return Ok();
        }


        public static async Task<Event> GetEvent(ILogger logger, RoboContext context, string EventId, int Year)
        {
            BlueAllianceContext refresher = new BlueAllianceContext();
            Event baEv;
            try
            {
                logger.LogInformation($"Requesting {EventId} which is {Year} and {EventId.Substring(4)}");
                baEv = new Event(await refresher.GetEvent(Year, EventId.Substring(4).Trim()));
            }
            catch (Exception ex)
            {
                logger.LogError("GetMatchs", ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            var existEvent = await context.Events
                .Include(x => x.Matchs)
                .Include(x => x.Teams)
                .FirstOrDefaultAsync(x => x.EventId == EventId);
            
            if (existEvent == null)
            {
                logger.LogInformation($"Adding new event {EventId}");
                await context.Database.OpenConnectionAsync();
                try
                {
                    await context.Events.AddAsync(baEv);
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Teams ON");
                    await context.SaveChangesAsync();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Teams OFF");
                }
                finally
                {
                    context.Database.CloseConnection();
                }
                return baEv;
            }
            else
            {
                logger.LogInformation($"Updating existing event {EventId}");
                if (existEvent.Teams == null)
                    existEvent.Teams = new List<SQLDataObjects.Team>();
                if (existEvent.Matchs == null)
                    existEvent.Matchs = new List<Match>();
                if (existEvent.TeamEvents == null)
                    existEvent.TeamEvents = new List<TeamEvent>();

                var notHereTeams = baEv.Teams.Where(x => !existEvent.Teams.Contains(x)).ToList();
                if (notHereTeams.Count != 0)
                {
                    // See if team is already in DB
                    foreach (var t in notHereTeams)
                        if (!(await context.Teams.ContainsAsync(t)))
                        {
                            await context.Teams.AddAsync(t);
                            existEvent.TeamEvents.Add(new TeamEvent(t, existEvent));
                        }
                }
                existEvent.Matchs.AddRange(baEv.Matchs.Where(x => !existEvent.Matchs.Contains(x)));
                context.Events.Update(existEvent);
                foreach (var cm in existEvent.Matchs)
                {
                    if (!context.Matches.Any(a => a.MatchId == cm.MatchId))
                    {
                        context.Matches.Add(cm);
                        context.SaveChanges();
                    }
                }
                await context.Database.OpenConnectionAsync();
                try
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Teams ON");
                    await context.SaveChangesAsync();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Teams OFF");
                }
                finally
                {
                    context.Database.CloseConnection();
                }
                return baEv;
            }
        }
    }
}