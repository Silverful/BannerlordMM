namespace BL.API.Core.Domain.Player
{
    public class Player : BaseEntity
    {
        public string Nickname { get; protected set; }
        public string Country { get; protected set; }
        public string Clan { get; protected set; }
        public PlayerClass MainClass { get; protected set; }
        public PlayerClass SecondaryClass { get; protected set; }
        public int DiscordId { get; protected set; }
        public virtual PlayerMMR PlayerMMR { get; protected set; }
    }
}
