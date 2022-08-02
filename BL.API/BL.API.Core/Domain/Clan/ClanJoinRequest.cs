using System;

namespace BL.API.Core.Domain.Clan
{
    public class ClanJoinRequest : BaseEntity
    {
        public Guid FromPlayerId { get; set; }
        public virtual Player.Player FromPlayer { get; set; }
        public Guid ToClanId { get; set; }
        public virtual Clan ToClan { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDismissed { get; set; }
        public DateTime ApprovedTimestamp { get; set; }
    }
}
