using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public static class UpdatePlayerCommand
    {
        public record Command(Player Player) : IRequest<Task>;

        public class UpdatePlayerCommandHandler : IRequestHandler<Command, Task>
        {
            private readonly IRepository<Player> _repository;

            public UpdatePlayerCommandHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<Task> Handle(Command request, CancellationToken cancellationToken)
            {
                //TODO add check id logic
                await _repository.UpdateAsync(request.Player);
                return Task.CompletedTask;
            }
        }
    }
}
