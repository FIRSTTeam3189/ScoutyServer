using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.SQLDataObjects
{
    public class Note
    {
        public int Id { get; set; }
        public string PerformenceId { get; set; }
        public string DataSheetId { get; set; }
        public DataSheet DataSheet { get; set; }
        public string Data { get; set; }
        public string URI { get; set; }
    }
}
