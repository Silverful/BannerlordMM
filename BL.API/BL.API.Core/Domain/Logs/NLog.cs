using System;
using System.ComponentModel.DataAnnotations;

namespace BL.API.Core.Domain.Logs
{
    public class NLog
    {
        public int ID { get; set; }
        [MaxLength(200)]
        public string MachineName { get; set; }
        public DateTime Logged { get; set; }
        [MaxLength(5)]
        public string Level { get; set; }
        public string Message { get; set; }
        [MaxLength(300)]
        public string Logger { get; set; }
        public string Properties { get; set; }
        [MaxLength(300)]
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
