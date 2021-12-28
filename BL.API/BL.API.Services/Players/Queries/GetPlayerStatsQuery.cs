﻿using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using BL.API.Services.Stats.Model;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayerStatsQuery
    {
        public record Query(string PlayerId) : IRequest<PlayerStatItemResponse>;

        public class GetPlayerStatsHandler : IRequestHandler<Query, PlayerStatItemResponse>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;
            private readonly IMediator _mediator;
            private readonly IRepository<PlayerMMR> _mmrs;

            public GetPlayerStatsHandler(IRepository<Player> players, 
                IRepository<PlayerMMR> mmrs,
                IRepository<PlayerMatchRecord> matchRecords,
                IMediator mediator)
            {
                _matchRecords = matchRecords;
                _players = players;
                _mmrs = mmrs;
                _mediator = mediator;
            }

            public async Task<PlayerStatItemResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!Guid.TryParse(request.PlayerId, out Guid id)) throw new GuidCantBeParsedException();
                
                var player = await _players.GetByIdAsync(id);

                if (player == null) throw new NotFoundException();

                var players = await _players.GetAllAsync();

                var matchRecords = await _matchRecords.GetWhereAsync(m => m.PlayerId == id, false, mr => mr.Match);

                var records =
                    from record in matchRecords
                    group record by record.PlayerId.Value into g
                    select g;

                var rankTable = await _mediator.Send(new GetRanksQuery.Query(players));
                var stats = PlayerStatItemResponse.FromMatchRecordGrouping(player, records.FirstOrDefault(), rankTable);

                var pos = (await _mmrs.GetAllAsync())
                    .Where(m => m.Season.OnGoing)
                    .OrderByDescending(m => m.MMR)
                    .Select((m, i) => new { m.PlayerId, i })
                    .Where(mi => mi.PlayerId == player.Id)
                    .Select(mi => mi.i)
                    .First();

                stats.Position = pos + 1;

                return stats;
            }
        }
    }
}
