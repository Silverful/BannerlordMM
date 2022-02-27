using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using BL.API.Services.Regions.Queries;
using BL.API.Services.Stats.Model;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayerStatsByDiscordIdQuery
    {
        public record Query(long discordId, string RegionShortName) : IRequest<PlayerStatItemResponse>;

        public class GetPlayerStatsByDiscordIdQueryHandler : IRequestHandler<Query, PlayerStatItemResponse>
        {
            private readonly IRepository<PlayerMatchRecord> _matchRecords;
            private readonly IRepository<Player> _players;
            private readonly ISeasonResolverService _seasonResolver;
            private readonly IMediator _mediator;

            public GetPlayerStatsByDiscordIdQueryHandler(IRepository<Player> players,
                IRepository<PlayerMatchRecord> matchRecords,
                ISeasonResolverService seasonResolver,
                IMediator mediator)
            {
                _matchRecords = matchRecords;
                _players = players;
                _mediator = mediator;
                _seasonResolver = seasonResolver;
            }

            public async Task<PlayerStatItemResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var player = await _players.GetFirstWhereAsync(p => p.DiscordId == request.discordId);
                var season = await _seasonResolver.GetCurrentSeasonAsync();
                var region = await _mediator.Send(new GetRegionByShortName.Query(request.RegionShortName));

                if (player == null) throw new NotFoundException();

                var players = await _players.GetAllAsync();
                var matchRecords = await _matchRecords.GetWhereAsync(m => m.PlayerId == player.Id && m.Match.SeasonId == season.Id && m.Match.RegionId == region.Id, false, mr => mr.Match);

                var records =
                    from record in matchRecords
                    group record by record.PlayerId.Value into g
                    select g;

                var rankTable = await _mediator.Send(new GetRanksQuery.Query(null, region.Id));
                var stats = PlayerStatItemResponse.FromMatchRecordGrouping(player, records.FirstOrDefault(), rankTable, region.Id);

                return stats;
            }
        }
    }
}
