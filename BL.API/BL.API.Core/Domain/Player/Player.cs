using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BL.API.Core.Domain.Player
{
    public class Player : BaseEntity
    {
        public Player() : base() { }

        [MaxLength(64)]
        public string Nickname { get; set; }

        [MaxLength(32)]
        public string Country { get; set; }

        public Guid ClanId { get; set; }
        [ForeignKey("ClanId")]
        public Clan.Clan Clan { get; set; }

        public bool IsIGL { get; set; }

        public PlayerClass MainClass { get; set; }

        public PlayerClass SecondaryClass { get; set; }

        public long? DiscordId { get; set; }

        public ICollection<PlayerMMR> PlayerMMRs { get; set; }

        public PlayerMMR GetPlayerMMR (string shortName)
        {
            return PlayerMMRs.Where(pm => pm.Season.OnGoing && pm.Region.ShortName == shortName).FirstOrDefault();
        }

        public PlayerMMR GetPlayerMMR(Guid regionId)
        {
            return PlayerMMRs.Where(pm => pm != null && pm.Season.OnGoing && pm.RegionId == regionId).FirstOrDefault();
        }
    }
}
