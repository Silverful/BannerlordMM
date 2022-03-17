using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Domain.Settings;
using System;

namespace BL.API.UnitTests.Builders
{
    public class PlayerMMRBuilder
    {
        private Guid _id;
        private double _mmr;
        private Season _season;
        private Player _player;
        private Region _region;

        public PlayerMMRBuilder WithId(Guid? id)
        {
            _id = id ?? Guid.NewGuid();
            return this;
        }

        public PlayerMMRBuilder WithMMR(double mmr)
        {
            _mmr = mmr;
            return this;
        }

        public PlayerMMRBuilder WithRegion(Region region)
        {
            _region = region;
            return this;
        }

        public PlayerMMRBuilder WithSeason(Season season)
        {
            _season = season;
            return this;
        }

        public PlayerMMRBuilder WithPlayer(Player player)
        {
            _player = player;
            return this;
        }

        public PlayerMMR Build()
        {
            return new PlayerMMR
            {
                Id = _id,
                MMR = _mmr,
                Season = _season,
                Player = _player,
                Region = _region
            };
        }
    }
}
