using System;
using System.Collections.Generic;
using System.Text;

namespace RobotServer.Models
{
    public static class BlueAllianceConstants
    {
        /// <summary>
        /// Header key to use when requesting header
        /// </summary>
        public static string HeaderKey => "X-TBA-App-Id";

        /// <summary>
        /// Header value to use for the key
        /// </summary>
        public static string HeaderValue => "frc3189:scouting_app:v01";

        /// <summary>
        /// The API Path to Blue Alliance
        /// </summary>
        public static string ApiPath => "http://www.thebluealliance.com/api/v2/";

        /// <summary>
        /// Request path to get pages of teams, Parameter 1 is the page
        /// Gets 500 teams at a time
        /// </summary>
        public static string Teams => "teams/{0}";

        /// <summary>
        /// Request Path to get the information of a team, Parameter 1 is the team number
        /// </summary>
        public static string Team => "team/frc{0}";

        /// <summary>
        /// Request path to get the events a team went to in a year, Parameter 1 is the team number, 2 is the event
        /// </summary>
        public static string TeamEventList => "team/frc{0}/{1}/events";

        /// <summary>
        /// Request path to get the matches a team participated at an event, Paramater 1 is the team number, 2 is the year, 3 is the event code for that year
        /// </summary>
        public static string TeamEventMatchesList => "team/frc{0}/event/{1}{2}/matches";

        /// <summary>
        /// Request path to get the events for a year, Parameter 1 is the year
        /// </summary>
        public static string EventsList => "events/{0}";

        /// <summary>
        /// Request path to get the info for an event for a year, Parameter 1 is the year, Parameter 2 is the event code
        /// </summary>
        public static string EventInfo => "event/{0}{1}";

        /// <summary>
        /// Request path to get the info for the teams at an event for a year, Parameter 1 is the year, Parameter 2 is the event code
        /// </summary>
        public static string EventTeams => "event/{0}{1}/teams";

        /// <summary>
        /// Request path to get the info for the matches at an event for a year, Parameter 1 is the year, Parameter 2 is the event Code
        /// </summary>
        public static string EventMatches => "event/{0}{1}/matches";

        /// <summary>
        /// Request path to get the info for the stats at an event for a year, Parameter 1 is the year, Parameter 2 is the Event Code
        /// </summary>
        public static string EventStats => "event/{0}{1}/stats";

        /// <summary>
        /// Request path to get the info for the rankings at an event for a year, Parameter 1 is the year, Parameter 2 is the Event Code
        /// </summary>
        public static string EventRanking => "event/{0}{1}/rankings";
    }
}
