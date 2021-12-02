using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayerByIdQuery 
    {
        public record Query(Guid PlayerId) : IRequest<Player>;

        public class GetAllPlayersQueryHandler : IRequestHandler<Query, Player>
        {
            private readonly IRepository<Player> _repository;

            public GetAllPlayersQueryHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<Player> Handle(Query request, CancellationToken cancellationToken)
            {
                var player = await _repository.GetByIdAsync(request.PlayerId);
                return player;
            }
        }
    }
}
