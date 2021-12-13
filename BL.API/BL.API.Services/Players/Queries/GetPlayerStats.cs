using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayerStats
    {
        public record Query(string PlayerId) : IRequest<IEnumerable<PlayerStatItemResponse>>;

        public class GetPlayerStatsHandler : IRequestHandler<Query, IEnumerable<PlayerStatItemResponse>>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;


            public GetPlayerStatsHandler(IRepository<Player> players, IRepository<PlayerMatchRecord> matchRecords)
            {
                _matchRecords = matchRecords;
                _players = players;
            }

            public async Task<IEnumerable<PlayerStatItemResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!Guid.TryParse(request.PlayerId, out Guid id)) throw new GuidCantBeParsedException();

                var player = await _players.GetByIdAsync(id);

                if (player == null) throw new NotFoundException();

                var matchRecords = await _matchRecords.GetWhereAsync(m => m.PlayerId == id);

                var stats =
                    from record in matchRecords
                    group record by record.PlayerId into g
                    select new PlayerStatItemResponse()
                    {
                        Nickname = g.First().Player?.Nickname,
                        Country =  g.First().Player?.Country,
                        Clan =  g.First().Player?.Clan,
                        MainClass =  g.First().Player.MainClass.ToString(),
                        SecondaryClass =  g.First().Player?.SecondaryClass.ToString(),
                        DiscordId =  g.First().Player?.DiscordId,
                        MMR =  g.First().Player?.PlayerMMR,
                        MatchesPlayed =  g.Count(),
                        MatchesWon =  g.Where(x => x.TeamIndex == x.Match.TeamWon).Count(),
                        WR =  g.Where(x => x.TeamIndex == x.Match.TeamWon).Count() == 0 ? 0 : g.Count() / g.Where(x => x.TeamIndex == x.Match.TeamWon).Count(), //TODO make default view with premade params
                        RoundsPlayed =  g.Sum(x => x.Match.RoundsPlayed),
                        KR =  g.Sum(x => x.Kills) / g.Sum(x => x.Match.RoundsPlayed),
                        Assists =  g.Sum(x => x.Assists),
                        AR =  g.Sum(x => x.Assists) / g.Sum(x => x.Match.RoundsPlayed),
                        KAR =  (g.Sum(x => x.Kills) + g.Sum(x => x.Assists)) / g.Sum(x => x.Match.RoundsPlayed),
                        TotalScore =  g.Sum(x => x.Score),
                        SR =  g.Sum(x => x.Score) / g.Sum(x => x.Match.RoundsPlayed),
                        MVP =  g.Sum(x => x.MVPs),
                        MVPR =  g.Sum(x => x.MVPs) / g.Sum(x => x.Match.RoundsPlayed)
                    };

                return stats;
            }
        }
    }
}
