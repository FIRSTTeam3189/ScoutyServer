using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobotServer.SQLDataObjects
{
    public class Picture
    {
        public int Id { get; set; }
        public string DataSheetId { get; set; }
        public DataSheet DataSheet { get; set; }
        public byte[] Data { get; set; }
        public string Note { get; set; }
    }
}
