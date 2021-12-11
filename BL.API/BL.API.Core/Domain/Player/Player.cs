using System.ComponentModel.DataAnnotations;

namespace BL.API.Core.Domain.Player
{
    public class Player : BaseEntity
    {
        [MaxLength(64)]
        public string Nickname { get; set; }

        [MaxLength(32)]
        public string Country { get; set; }

        [MaxLength(32)]
        public string Clan { get; set; }

        public PlayerClass MainClass { get; set; }

        public PlayerClass SecondaryClass { get; set; }

        public int DiscordId { get; set; }

        public int PlayerMMR { get; set; }
    }
}
