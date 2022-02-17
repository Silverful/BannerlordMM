using BL.API.Core.Domain.Player;
using System;

namespace BL.API.UnitTests.Builders
{
    public class PlayerBuilder
    {
        private Guid _id;
        private string _nickname;
        private string _country;
        private string _clan;
        private PlayerClass _mainClass;
        private PlayerClass _secondaryClass;
        private int _discordId;
        private int _playerMMR;

        public PlayerBuilder WithId(Guid? id)
        {
            _id = id ?? Guid.NewGuid();
            return this;
        }

        public PlayerBuilder WithNickname(string nickname)
        {
            _nickname = nickname;
            return this;
        }

        public PlayerBuilder WithCountry(string country)
        {
            _country = country;
            return this;
        }

        public PlayerBuilder WithId(string clan)
        {
            _clan = clan;
            return this;
        }

        public PlayerBuilder WithMainClass(PlayerClass cl)
        {
            _mainClass = cl;
            return this;
        }

        public PlayerBuilder WithSecondaryClass(PlayerClass cl)
        {
            _secondaryClass = cl;
            return this;
        }

        public PlayerBuilder WithDiscordId(int discordId)
        {
            _discordId = discordId;
            return this;
        }

        public PlayerBuilder WithMainClass(int mmr)
        {
            _playerMMR = mmr;
            return this;
        }

        public Player Build()
        {
            return new Player
            {
                Id = _id,
                Nickname = _nickname,
                Country = _country,
                Clan = _clan,
                MainClass = _mainClass,
                SecondaryClass = _secondaryClass,
                DiscordId = _discordId,
                //PlayerMMR = _playerMMR
            };
        }
    }
}
