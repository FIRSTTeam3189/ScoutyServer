using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ScoutingServer.SQLDataObjects;
using Newtonsoft.Json.Linq;
using RobotServer.SQLDataObjects;
using ScoutingServer.Models;

namespace RobotServer.Models
{
    public class BlueAllianceClient
    {
        HttpClient client;

        public BlueAllianceClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add(BlueAllianceConstants.HeaderKey, BlueAllianceConstants.HeaderValue);
        }

        /// <summary>
        /// Gets the events for specified year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<IList<Event>> GetEvents(int year)
        {
            var reqeustUri = string.Format(
                Path.Combine(BlueAllianceConstants.ApiPath, BlueAllianceConstants.EventsList), year);
            //System.Diagnostics.Trace.TraceError(reqeustUri.ToString());
            // Get the array from the client
            var stream = await client.GetStreamAsync(reqeustUri);
            var array = (stream).JArrayFromStream();

            return array.Select(x => x.GetEventFromJToken()).ToList();
        }

        /// <summary>
        /// Gets the Teams from a page on blue alliance
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Team>> GetTeams(int page)
        {
            var requestUri = string.Format(
                Path.Combine(BlueAllianceConstants.ApiPath, BlueAllianceConstants.Teams), page);

            var array = (await client.GetStreamAsync(requestUri)).JArrayFromStream();

            return array.Select(x => x.GetTeamFromJToken());
        }

        /// <summary>
        /// Gets the Team's Number from blue alliance
        /// </summary>
        /// <param name="teamnumber"></param>
        /// <returns></returns>
        public async Task<Team> GetTeam(int teamnumber)
        {
            var requestUri = string.Format(
                Path.Combine(BlueAllianceConstants.ApiPath, BlueAllianceConstants.Team), teamnumber);

            var array = (await client.GetStreamAsync(requestUri)).JTokenFromStream();

            return array.GetTeamFromJToken();
        }

        /// <summary>
        /// Gets the Teams for an event from a certain year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public async Task<IList<Team>> GetEventTeams(int year, string eventCode)
        {
            var requestUri = string.Format(
                Path.Combine(BlueAllianceConstants.ApiPath, BlueAllianceConstants.EventTeams), year, eventCode);

            var array = (await client.GetStreamAsync(requestUri)).JArrayFromStream();

            return array.Select(x => x.GetTeamFromJToken()).ToList();
        }

        public async Task<List<Match>> GetEventMatches(int year, string eventCode)
        {
            var requestUri = string.Format(
                Path.Combine(BlueAllianceConstants.ApiPath, BlueAllianceConstants.EventMatches), year, eventCode);

            var array = (await client.GetStreamAsync(requestUri)).JArrayFromStream();
            var thing = array.Select(x => x.GetMatchFromJToken()).ToList();
            //System.Diagnostics.Trace.TraceError(thing.Count + "");
            return thing;
        }
    }
}
