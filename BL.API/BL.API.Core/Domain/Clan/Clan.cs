using BL.API.Core.Domain.Settings;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BL.API.Core.Domain.Clan
{
    public class Clan : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string AvatarURL { get; set; }
        public virtual ICollection<ClanMember>? ClanMembers { get; set; }
        public ClanEnterType EnterType { get; set; }
        public Guid? RegionId { get; set; }
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }

        public ClanMember GetLeader() => ClanMembers?.ToArray().FirstOrDefault(cm => cm.MemberType == ClanMemberType.Leader);
    }
}
