using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Extensions;
using BL.API.Services.Stats.Model;
using BL.API.Services.Stats.Utility;
using MediatR;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetAllStatsQuery
    {
        public record Query() : IRequest<AllStatsResponse>;

        public class GetAllStatsQueryHandler : IRequestHandler<Query, AllStatsResponse>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;
            private readonly IRepository<Match> _matches;
            private readonly StatsProps _statsProps;

            public GetAllStatsQueryHandler(IRepository<PlayerMatchRecord> matchRecords, 
                IRepository<Player> players,
                IRepository<Match> matches,
                IOptions<StatsProps> statsProps
                )
            {
                _matchRecords = matchRecords;
                _players = players;
                _matches = matches;
                _statsProps = statsProps.Value;
            }

            private int MinimumMatchesPlayed { get => _statsProps.MinimumMatchesPlayed; }

            public async Task<AllStatsResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = await _players.GetAllAsync();
                var matchRecords = await _matchRecords.GetAllAsync();

                var playerStats = StatsQueryHelper.GetPlayersStats(players, matchRecords).OrderByDescending(s => s.MMR).ToList();

                var iglStats = (from mr in matchRecords
                                where mr.PlayerId.HasValue
                                group mr by mr.PlayerId into gmr
                                join player in players on gmr.Key equals player.Id
                                where gmr.Count() > MinimumMatchesPlayed && player.IsIGL
                                select new KeyValuePair<string, decimal>(player.Nickname, (decimal)gmr.Where(mr => mr.TeamIndex == mr.Match.TeamWon).Count() / gmr.Count()))
                                .OrderByDescending(x => x.Value)
                                .ToDictionary();

                var factionStats = (from mr in matchRecords
                                   where mr.Faction.HasValue
                                   group mr by mr.Faction into gmr
                                   select new KeyValuePair<string, decimal>(gmr.Key.ToString(), (decimal)gmr.Where(mr => mr.TeamIndex == mr.Match.TeamWon).Count() / gmr.Count()))
                                   .OrderByDescending(x => x.Value)
                                   .ToDictionary();

                var infStats = (from mr in matchRecords
                                where mr.PlayerId.HasValue
                                group mr by mr.PlayerId into gmr
                                join player in players on gmr.Key equals player.Id
                                where gmr.Count() > MinimumMatchesPlayed && player.MainClass == PlayerClass.Infantry
                                select new KeyValuePair<string, decimal>(player.Nickname, (decimal)gmr.Sum(x => x.Score) / gmr.Sum(x => x.Match.RoundsPlayed)))
                                .OrderByDescending(x => x.Value)
                                .ToDictionary();

                var archerStats = (from mr in matchRecords
                                where mr.PlayerId.HasValue
                                group mr by mr.PlayerId into gmr
                                join player in players on gmr.Key equals player.Id
                                where gmr.Count() > MinimumMatchesPlayed && player.MainClass == PlayerClass.Archer
                                select new KeyValuePair<string, decimal>(player.Nickname, (decimal)gmr.Sum(x => x.Score) / gmr.Sum(x => x.Match.RoundsPlayed)))
                                .OrderByDescending(x => x.Value)
                                .ToDictionary();

                var cavStats = (from mr in matchRecords
                                where mr.PlayerId.HasValue
                                group mr by mr.PlayerId into gmr
                                join player in players on gmr.Key equals player.Id
                                where gmr.Count() > MinimumMatchesPlayed && player.MainClass == PlayerClass.Cavalry
                                select new KeyValuePair<string, decimal>(player.Nickname, (decimal)gmr.Sum(x => x.Score) / gmr.Sum(x => x.Match.RoundsPlayed)))
                                .OrderByDescending(x => x.Value)
                                .ToDictionary();

                return new AllStatsResponse
                {
                    PlayerStats = playerStats,
                    IGLStats = iglStats,
                    FactionStats = factionStats,
                    TopPlayerByClassStats = new TopPlayersByClassStats
                    {
                        Infantry = infStats,
                        Archer = archerStats,
                        Cavalry = cavStats
                    }
                };
            }
        }
    }
}
