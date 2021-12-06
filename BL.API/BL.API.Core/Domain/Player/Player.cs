using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.API.Core.Domain.Player
{
    public class Player : BaseEntity
    {
        [MaxLength(64)]
        public string Nickname { get; protected set; }

        [MaxLength(32)]
        public string Country { get; protected set; }

        [MaxLength(32)]
        public string Clan { get; protected set; }

        public PlayerClass MainClass { get; protected set; }

        public PlayerClass SecondaryClass { get; protected set; }

        public int DiscordId { get; protected set; }

        public Guid PlayerMMRId { get; protected set; }

        [ForeignKey("PlayerMMRId")]
        public virtual PlayerMMR PlayerMMR { get; protected set; }
    }
}
