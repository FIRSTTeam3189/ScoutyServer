using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.ClientData
{
    public class ClientNote
    {
        public int Id { get; set; }
        public string PerformenceId { get; set; }
        public string DataSheetId { get; set; }
        public string Data { get; set; }
        public string URI { get; set; }
    }
}
