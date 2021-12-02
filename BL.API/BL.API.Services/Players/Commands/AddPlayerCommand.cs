using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public static class AddPlayerCommand
    {
        public record Command(Player Player) : IRequest<Guid>;

        public class AddPlayerCommandHandler : IRequestHandler<Command, Guid>
        {
            private readonly IRepository<Player> _repository;

            public AddPlayerCommandHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                await _repository.CreateAsync(request.Player);

                return request.Player.Id;
            }
        }
    }
}
