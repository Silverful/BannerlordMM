using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using BL.API.Services.Extensions;
using BL.API.Services.Regions.Queries;
using BL.API.Services.Stats.Model;
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
        public record Query(string RegionShortName) : IRequest<AllStatsResponse>;

        public class GetAllStatsQueryHandler : IRequestHandler<Query, AllStatsResponse>
        {
            private readonly IRepository<Player> _players;
            private readonly StatsProps _statsProps;
            private readonly IRepository<Match> _matches;
            private readonly ISeasonResolverService _seasonResolver;
            private readonly IMediator _mediator;

            public GetAllStatsQueryHandler(IRepository<Player> players,
                IRepository<Match> matches,
                IOptions<StatsProps> statsProps,
                ISeasonResolverService seasonResolver,
                IMediator mediator
                )
            {
                _players = players;
                _matches = matches;
                _seasonResolver = seasonResolver;
                _statsProps = statsProps.Value;
                _mediator = mediator;
            }

            private int MinimumMatchesPlayed { get => _statsProps.MinimumMatchesPlayed; }

            public async Task<AllStatsResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = await _players.GetAllAsync();
                var region = await _mediator.Send(new GetRegionByShortName.Query(request.RegionShortName));

                if (region == null) throw new NotFoundException();

                var season = await _seasonResolver.GetCurrentSeasonAsync(region.Id);
                var matches = await _matches.GetWhereAsync(m => m.SeasonId == season.Id && m.Region.Id == region.Id, true, m => m.PlayerRecords);
                var matchRecords = matches.Select(x => x.PlayerRecords).SelectMany(x => x);

                var calibratedPlayers = matchRecords.GroupBy(x => x.PlayerId).Where(x => x.Count() >= 10).Select(x => x.First()?.Player);
                var rankTable = await _mediator.Send(new GetRanksQuery.Query(calibratedPlayers, region.Id));

                var playerStats = (await _mediator.Send(new GetPlayersStatsQuery.Query(players, matchRecords, rankTable, region.ShortName)))
                    .Where(ps => ps.Played > 1);

                var iglStats = (from mr in matchRecords
                                where mr.PlayerId.HasValue
                                group mr by mr.PlayerId into gmr
                                join player in players on gmr.Key equals player.Id
                                where gmr.Count() >= MinimumMatchesPlayed && player.IsIGL
                                select new KeyValuePair<string, double>(player.Nickname, (double)gmr.Where(mr => mr.TeamIndex == mr.Match.TeamWon).Count() / gmr.Count()))
                                .OrderByDescending(x => x.Value)
                                .Take(10);

                var factionStats = (from mr in matchRecords
                                   where mr.Faction.HasValue
                                   group mr by mr.Faction into gmr
                                   select new KeyValuePair<string, double>(gmr.Key.ToString(), (double)gmr.Where(mr => mr.TeamIndex == mr.Match.TeamWon).Count() / gmr.Count()))
                                   .OrderByDescending(x => x.Value);

                var infStats = (from mr in matchRecords
                                where mr.PlayerId.HasValue
                                group mr by mr.PlayerId into gmr
                                join player in players on gmr.Key equals player.Id
                                where gmr.Count() >= MinimumMatchesPlayed && player.MainClass == PlayerClass.Infantry
                                select new KeyValuePair<string, double>(player.Nickname, (double)gmr.Sum(x => x.Score) / gmr.Sum(x => x.Match.RoundsPlayed)))
                                .OrderByDescending(x => x.Value)
                                .Take(15);

                var archerStats = (from mr in matchRecords
                                where mr.PlayerId.HasValue
                                group mr by mr.PlayerId into gmr
                                join player in players on gmr.Key equals player.Id
                                where gmr.Count() >= MinimumMatchesPlayed && player.MainClass == PlayerClass.Archer
                                select new KeyValuePair<string, double>(player.Nickname, (double)gmr.Sum(x => x.Score) / gmr.Sum(x => x.Match.RoundsPlayed)))
                                .OrderByDescending(x => x.Value)
                                .Take(15);

                var cavStats = (from mr in matchRecords
                                where mr.PlayerId.HasValue
                                group mr by mr.PlayerId into gmr
                                join player in players on gmr.Key equals player.Id
                                where gmr.Count() >= MinimumMatchesPlayed && player.MainClass == PlayerClass.Cavalry
                                select new KeyValuePair<string, double>(player.Nickname, (double)gmr.Sum(x => x.Score) / gmr.Sum(x => x.Match.RoundsPlayed)))
                                .OrderByDescending(x => x.Value)
                                .Take(15);

                var playerByFactionStats =
                    from mr in matchRecords
                    where mr.PlayerId.HasValue && mr.Faction.HasValue
                    group mr by mr.PlayerId into gmr
                    join player in players on gmr.Key equals player.Id
                    orderby player.Nickname
                    select new PlayerByFactionWRItem
                    {
                        Nickname = player.Nickname,
                        AseraiCount = gmr.Where(mr => mr.Faction == Faction.Aserai).Count(),
                        AseraiWR = gmr.Where(mr => mr.Faction == Faction.Aserai && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDouble(gmr.Where(mr => mr.Faction == Faction.Aserai).Count()),
                        BattaniaCount = gmr.Where(mr => mr.Faction == Faction.Battania).Count(),
                        BattaniaWR = gmr.Where(mr => mr.Faction == Faction.Battania && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDouble(gmr.Where(mr => mr.Faction == Faction.Battania).Count()),
                        EmpireCount = gmr.Where(mr => mr.Faction == Faction.Empire).Count(),
                        EmpireWR = gmr.Where(mr => mr.Faction == Faction.Empire && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDouble(gmr.Where(mr => mr.Faction == Faction.Empire).Count()),
                        KhuzaitCount = gmr.Where(mr => mr.Faction == Faction.Khuzait).Count(),
                        KhuzaitWR = gmr.Where(mr => mr.Faction == Faction.Khuzait && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDouble(gmr.Where(mr => mr.Faction == Faction.Khuzait).Count()),
                        SturgiaCount = gmr.Where(mr => mr.Faction == Faction.Sturgia).Count(),
                        SturgiaWR = gmr.Where(mr => mr.Faction == Faction.Sturgia && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDouble(gmr.Where(mr => mr.Faction == Faction.Sturgia).Count()),
                        VlandiaCount = gmr.Where(mr => mr.Faction == Faction.Vlandia).Count(),
                        VlandiaWR = gmr.Where(mr => mr.Faction == Faction.Vlandia && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDouble(gmr.Where(mr => mr.Faction == Faction.Vlandia).Count())
                    };

                return new AllStatsResponse
                {
                    PlayerStats = playerStats,
                    IGLStats = iglStats.ToDictionary(),
                    FactionStats = factionStats.ToDictionary(),
                    DivisionStats = rankTable,
                    PlayersByFactionStats = playerByFactionStats,
                    TopPlayerByClassStats = new TopPlayersByClassStats
                    {
                        Infantry = infStats.ToDictionary(),
                        Archer = archerStats.ToDictionary(),
                        Cavalry = cavStats.ToDictionary()
                    }
                };
            }
        }
    }
}
