using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;

namespace BL.API.UnitTests.Builders
{
    public class PlayerRecordBuilder
    {
        private Player _player;
        private Match _match;
        private byte _teamIndex;
        private byte _roundsWon;
        private Faction _faction;
        private sbyte? _kills;
        private sbyte? _assists;
        private byte? _deaths;
        private int? _score;
        private byte? _mvps;
        private int _mmrChange;
        private byte _calibrationIndex;

        public PlayerRecordBuilder WithPlayer(Player player)
        {
            _player = player;
            return this;
        }

        public PlayerRecordBuilder WithMatch(Match match)
        {
            _match = match;
            return this;
        }

        public PlayerRecordBuilder WithTeamIndex(byte teamIndex)
        {
            _teamIndex = teamIndex;
            return this;
        }

        public PlayerRecordBuilder WithRoundsWon(byte roundsWon)
        {
            _roundsWon = roundsWon;
            return this;
        }

        public PlayerRecordBuilder WithFaction(Faction faction)
        {
            _faction = faction;
            return this;
        }
        public PlayerRecordBuilder WithKills(sbyte kills)
        {
            _kills = kills;
            return this;
        }

        public PlayerRecordBuilder WithAssists(sbyte assists)
        {
            _assists = assists;
            return this;
        }

        public PlayerRecordBuilder WithScore(int score)
        {
            _score = score;
            return this;
        }

        public PlayerRecordBuilder WithMVPs(byte mvps)
        {
            _mvps = mvps;
            return this;
        }

        public PlayerRecordBuilder WithMMRChange(int mmrChange)
        {
            _mmrChange = mmrChange;
            return this;
        }

        public PlayerRecordBuilder WithCalibrationIndex(byte calibrationIndex)
        {
            _calibrationIndex = calibrationIndex;
            return this;
        }

        public PlayerRecordBuilder WithDeaths(byte deaths)
        {
            _deaths = deaths;
            return this;
        }

        public PlayerMatchRecord Build()
        {
            return new PlayerMatchRecord
            {
                Player = _player,
                PlayerId = _player.Id,
                Match = _match,
                MatchId = _match.Id,
                TeamIndex = _teamIndex,
                RoundsWon = _roundsWon,
                Faction = _faction,
                Kills = _kills,
                Deaths = _deaths,
                Assists = _assists,
                Score = _score,
                MVPs = _mvps,
                MMRChange = _mmrChange,
                CalibrationIndex = _calibrationIndex
            };
        }
    }
}
