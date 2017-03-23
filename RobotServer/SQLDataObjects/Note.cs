using RobotServer.ClientData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [MaxLength]
        public string Data { get; set; }
        public string URI { get; set; }

        public static Note GetNote(ClientNote c)
        {
            return new Note()
            {
                Data = c.Data,
                DataSheetId = c.DataSheetId,
                PerformenceId = c.PerformenceId,
                URI = c.URI
            };
        }
    }
}
