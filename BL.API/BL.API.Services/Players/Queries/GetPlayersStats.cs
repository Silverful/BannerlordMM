﻿using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayersStats
    {
        public record Query() : IRequest<IEnumerable<PlayerStatItemResponse>>;

        public class GetPlayersStatsHandler : IRequestHandler<Query, IEnumerable<PlayerStatItemResponse>>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;

            public GetPlayersStatsHandler(IRepository<PlayerMatchRecord> matchRecords)
            {
                _matchRecords = matchRecords;
            }

            public async Task<IEnumerable<PlayerStatItemResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var matchRecords = await _matchRecords.GetAllAsync();

                var stats =
                    from record in matchRecords
                    group record by record.PlayerId into g
                    select new PlayerStatItemResponse()
                    {
                        Nickname = g.First().Player?.Nickname,
                        Country = g.First().Player?.Country,
                        Clan = g.First().Player?.Clan,
                        MainClass = g.First().Player.MainClass.ToString(),
                        SecondaryClass = g.First().Player?.SecondaryClass.ToString(),
                        DiscordId = g.First().Player?.DiscordId,
                        MMR = g.First().Player?.PlayerMMR,
                        MatchesPlayed = g.Count(),
                        MatchesWon = g.Where(x => x.Faction == x.Match.FactionWon).Count(),
                        WR = g.Where(x => x.Faction == x.Match.FactionWon).Count() == 0 ? 0 : g.Count() / g.Where(x => x.Faction == x.Match.FactionWon).Count(), //TODO make default view with premade params
                        RoundsPlayed = g.Sum(x => x.Match.RoundsPlayed),
                        KR = g.Sum(x => x.Kills) / g.Sum(x => x.Match.RoundsPlayed),
                        Assists = g.Sum(x => x.Assists),
                        AR = g.Sum(x => x.Assists) / g.Sum(x => x.Match.RoundsPlayed),
                        KAR = (g.Sum(x => x.Kills) + g.Sum(x => x.Assists)) / g.Sum(x => x.Match.RoundsPlayed),
                        TotalScore = g.Sum(x => x.Score),
                        SR = g.Sum(x => x.Score) / g.Sum(x => x.Match.RoundsPlayed),
                        MVP = g.Sum(x => x.MVPs),
                        MVPR = g.Sum(x => x.MVPs) / g.Sum(x => x.Match.RoundsPlayed)
                    };

                return stats;
            }
        }
    }
}
