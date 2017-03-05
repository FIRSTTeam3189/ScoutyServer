using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using ScoutingServer.SQLDataObjects;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using RobotServer.SQLDataObjects;

namespace RobotServer.Models
{
    public static class JTokenExtensions
    {
        private const string EventLocation = "location";
        private const string EventType = "event_type";
        private const string EventCode = "event_code";
        private const string EventYear = "year";
        private const string EventWebsite = "website";
        private const string EventMatchs = "matchs";
        private const string EventKey = "key";

        private const string StartDate = "start_date";
        private const string EndDate = "end_date";
        private const string DateFormat = "yyyy-MM-dd";
        private const string EventName = "name";
        private const string EventOfficial = "official";

        private const string MatchKey = "key";
        private const string MatchCompLevel = "comp_level";
        private const string MatchNumber = "match_number";
        private const string MatchAlliences = "alliances";
        private const string MatchTeam = "teams";
        private const string MatchBlue = "blue";
        private const string MatchRed = "red";

        /// <summary>
        /// Gets an Event from the JToken provided
        /// </summary>
        /// <param name="obj">Object to get it from</param>
        /// <returns>The Event</returns>
        public static Event GetEventFromJToken(this JToken obj)
        {
            obj.IsNotNull();

            DateTime? start = null;
            DateTime? end = null;
            DateTime t;

            // Get the DateTimes for start, end
            if (DateTime.TryParseExact(obj[StartDate].ToObject<string>(), DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out t))
                start = DateTime.ParseExact(obj[StartDate].ToObject<string>(), DateFormat,
                    CultureInfo.InvariantCulture);

            if (DateTime.TryParseExact(obj[EndDate].ToObject<string>(), DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out t))
                end = DateTime.ParseExact(obj[EndDate].ToObject<string>(), DateFormat,
                    CultureInfo.InvariantCulture);

            var matchs = new List<Match>();

            var ev = new Event {
                Id = obj[EventKey].ToObject<string>(),
                Location = obj[EventLocation].ToObject<string>(),
                Year = obj[EventYear].ToObject<int>(),
                EventCode = obj[EventCode].ToObject<string>(),
                Website = obj[EventWebsite].ToObject<string>() ?? "No EventWebsite",
                StartDate = start,
                EndDate = end,
                Name = obj[EventName].ToObject<string>(),
                Official = obj[EventOfficial].ToObject<bool>()
            };

            return ev;
        }

        /// <summary>
        /// Gets a Team's Information from JToken provided
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Team GetTeamFromJToken(this JToken obj)
        {
            obj.IsNotNull();

            var te = new Team
            {
                TeamNumber = obj["team_number"].ToObject<int>(),
                NickName = obj["nickname"].ToObject<string>(),
                RookieYear = obj["rookie_year"].ToObject<int?>() ?? -1,
                TeamLocation = obj["location"].ToObject<string>(),
                Performances = new List<Performance >()
            };

            return te;
        }

        public static int GetTeam(this JToken obj, string color, int slot) {
            var thing = obj[MatchAlliences][color][MatchTeam].ToArray()[slot].ToObject<string>();
            return int.Parse(thing.Substring(3));
        }

        public static MatchType GetMatchType(string code) {
            switch(code.Trim().ToLower()) {
                case "qm":
                    return MatchType.Qualification;
                case "ef":
                    return MatchType.OctoFinal;
                case "qf":
                    return MatchType.QuarterFinal;
                case "sf":
                    return MatchType.SemiFinal;
                case "f":
                    return MatchType.Final;
                default:
                    return MatchType.Practice;
            }
        }

        public static Match GetMatchFromJToken(this JToken obj) {
            obj.IsNotNull();

            var ev = new Match {
                Id = obj[MatchKey].ToObject<string>(),
                MatchNubmer = obj[MatchNumber].ToObject<int>(),
                BlueOne = obj.GetTeam(MatchBlue, 0),
                BlueTwo = obj.GetTeam(MatchBlue, 1),
                BlueThree = obj.GetTeam(MatchBlue, 2),
                RedOne = obj.GetTeam(MatchRed, 0),
                RedTwo = obj.GetTeam(MatchRed, 1),
                RedThree = obj.GetTeam(MatchRed, 2),
                MatchType = GetMatchType(obj[MatchCompLevel].ToObject<string>()),
                time = obj["time"].ToObject<int>()
            };

            return ev;
        }

        /// <summary>
        /// Assertion test to check if an object of type T is not null
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="obj">Object to test</param>
        /// <returns>The object its self</returns>
        public static T IsNotNull<T>(this T obj) where T : class
        {
            Contract.Requires(obj != null);
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Contract.EndContractBlock();

            return obj;
        }
    }
}
