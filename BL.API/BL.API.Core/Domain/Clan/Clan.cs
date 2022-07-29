using PlayerMM = BL.API.Core.Domain.Player.Player;

namespace BL.API.Core.Domain.Clan
{
    public class Clan : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string AvatarURL { get; set; }
        public PlayerMM Leader { get; set; }  

        public PlayerMM[] Officers { get; set; }
        public PlayerMM[] Soldiers { get; set; }
        public ClanEnterType EnterType { get; set; }
    }
}
