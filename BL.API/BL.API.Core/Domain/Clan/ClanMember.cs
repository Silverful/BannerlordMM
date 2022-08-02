using System;
using System.ComponentModel.DataAnnotations.Schema;
using PlayerMM = BL.API.Core.Domain.Player.Player;

namespace BL.API.Core.Domain.Clan
{
    public class ClanMember : BaseEntity
    {
        public Guid ClanId { get;set; }
        [ForeignKey("ClanId")]
        public virtual Clan Clan { get; set; }
        public Guid PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public virtual PlayerMM Player { get; set; }
        public ClanMemberType MemberType { get; set; }
    }
}
