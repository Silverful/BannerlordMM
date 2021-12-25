using BL.API.Core.Domain.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.API.Migration
{
    public class Player 
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
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
    }
}
