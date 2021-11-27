using System;

namespace BL.API.Core.Domain.Match
{
    public class PlayerMatchRecord : BaseEntity
    {
        public Guid PlayerId { get; protected set; }
        public int Kills { get; protected set; }
        public int Assists { get; protected set; }
        public int? Deaths { get; protected set; }
        public int Score { get; protected set; }
        public int MVPs { get; protected set; }
        public int MMRChange { get; protected set; }
    }
}
