using System;

namespace BL.API.Core.Domain.Match
{
    public class TeamPlayer : BaseEntity
    {
        public Guid PlayerId { get; protected set; }
        public Guid MatchId { get; protected set; }
        public Faction Faction { get; protected set; }
    }
}
