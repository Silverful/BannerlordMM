using System;
using System.ComponentModel.DataAnnotations;

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

        public int PlayerMMR { get; set; }
    }
}
