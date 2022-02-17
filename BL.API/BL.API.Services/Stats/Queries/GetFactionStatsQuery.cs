using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Stats.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Stats.Queries
{
    public static class GetFactionStatsQuery
    {
        public record Query() : IRequest<AllStatsResponse>;

        public class GetFactionStatsQueryHandler : IRequestHandler<Query, AllStatsResponse>
        {
            private readonly IRepository<Player> _players;
            private readonly IRepository<Match> _matches;
            private readonly ISeasonResolverService _seasonResolver;
            private readonly IMediator _mediator;

            public GetFactionStatsQueryHandler(IRepository<Player> players,
                IRepository<Match> matches,
                ISeasonResolverService seasonResolver,
                IMediator mediator
                )
            {
                _players = players;
                _matches = matches;
                _seasonResolver = seasonResolver;
                _mediator = mediator;
            }

            public async Task<AllStatsResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }

    }
}
