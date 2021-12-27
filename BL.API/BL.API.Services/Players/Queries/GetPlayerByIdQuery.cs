using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetPlayerByIdQuery 
    {
        public record Query(string PlayerId) : IRequest<Player>;

        public class GetAllPlayersQueryHandler : IRequestHandler<Query, Player>
        {
            private readonly IRepository<Player> _repository;

            public GetAllPlayersQueryHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<Player> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!Guid.TryParse(request.PlayerId, out Guid id)) throw new GuidCantBeParsedException();

                var player = await _repository.GetByIdAsync(id);

                if (player == null) throw new NotFoundException();

                return player;
            }
        }
    }
}
