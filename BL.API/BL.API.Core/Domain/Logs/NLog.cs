using System;
using System.ComponentModel.DataAnnotations;

namespace BL.API.Core.Domain.Logs
{
    public class NLog
    {
        public int ID { get; protected set; }
        [MaxLength(200)]
        public string MachineName { get; protected set; }
        public DateTime Logged { get; protected set; }
        [MaxLength(5)]
        public string Level { get; protected set; }
        public string Message { get; protected set; }
        [MaxLength(300)]
        public string Logger { get; protected set; }
        public string Properties { get; protected set; }
        [MaxLength(300)]
        public string Callsite { get; protected set; }
        public string Exception { get; protected set; }
    }
}
