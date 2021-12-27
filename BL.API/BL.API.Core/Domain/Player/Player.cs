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

        [MaxLength(32)]
        public string Clan { get; set; }

        public bool IsIGL { get; set; }

        public PlayerClass MainClass { get; set; }

        public PlayerClass SecondaryClass { get; set; }

        public long? DiscordId { get; set; }

        public ICollection<PlayerMMR> PlayerMMRs { get; set; }

        [NotMapped]
        public PlayerMMR PlayerMMR { get => PlayerMMRs.FirstOrDefault(m => m.Season?.OnGoing ?? false) ?? null; }
    }
}
