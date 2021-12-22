﻿using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Extensions;
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
        public record Query() : IRequest<AllStatsResponse>;

        public class GetAllStatsQueryHandler : IRequestHandler<Query, AllStatsResponse>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;
            private readonly StatsProps _statsProps;
            private readonly IMediator _mediator;

            public GetAllStatsQueryHandler(IRepository<PlayerMatchRecord> matchRecords, 
                IRepository<Player> players,
                IOptions<StatsProps> statsProps,
                IMediator mediator
                )
            {
                _matchRecords = matchRecords;
                _players = players;
                _statsProps = statsProps.Value;
                _mediator = mediator;
            }

            private int MinimumMatchesPlayed { get => _statsProps.MinimumMatchesPlayed; }

            public async Task<AllStatsResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = await _players.GetAllAsync();
                var matchRecords = await _matchRecords.GetAllAsync();
                var rankTable = await _mediator.Send(new GetRanksQuery.Query(players));

                var playerStats = await _mediator.Send(new GetPlayersStats.Query(players, matchRecords, rankTable));

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
                        AseraiWR = gmr.Where(mr => mr.Faction == Faction.Aserai && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDecimal(gmr.Where(mr => mr.Faction == Faction.Aserai).Count()),
                        BattaniaCount = gmr.Where(mr => mr.Faction == Faction.Battania).Count(),
                        BattaniaWR = gmr.Where(mr => mr.Faction == Faction.Battania && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDecimal(gmr.Where(mr => mr.Faction == Faction.Battania).Count()),
                        EmpireCount = gmr.Where(mr => mr.Faction == Faction.Empire).Count(),
                        EmpireWR = gmr.Where(mr => mr.Faction == Faction.Empire && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDecimal(gmr.Where(mr => mr.Faction == Faction.Empire).Count()),
                        KhuzaitCount = gmr.Where(mr => mr.Faction == Faction.Khuzait).Count(),
                        KhuzaitWR = gmr.Where(mr => mr.Faction == Faction.Khuzait && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDecimal(gmr.Where(mr => mr.Faction == Faction.Khuzait).Count()),
                        VlandiaCount = gmr.Where(mr => mr.Faction == Faction.Vlandia).Count(),
                        VlandiaWR = gmr.Where(mr => mr.Faction == Faction.Vlandia && mr.TeamIndex == mr.Match.TeamWon).Count().SafeDivisionToDecimal(gmr.Where(mr => mr.Faction == Faction.Vlandia).Count())
                    };

                return new AllStatsResponse
                {
                    PlayerStats = playerStats,
                    IGLStats = iglStats,
                    FactionStats = factionStats,
                    DivisionStats = rankTable,
                    PlayersByFactionStats = playerByFactionStats,
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