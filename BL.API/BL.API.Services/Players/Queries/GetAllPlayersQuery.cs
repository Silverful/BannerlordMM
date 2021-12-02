using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetAllPlayersQuery
    {
        public record Query() : IRequest<IEnumerable<Player>>;

        public class GetAllPlayersQueryHandler : IRequestHandler<Query, IEnumerable<Player>>
        {
            private readonly IRepository<Player> _repository;

            public GetAllPlayersQueryHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<Player>> Handle(Query request, CancellationToken cancellationToken)
            {
                var players = await _repository.GetAllAsync();
                return players;
            }
        }
    }
}
