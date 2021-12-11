using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public class DeletePlayerCommand
    {
        public record Command(Guid PlayerId) : IRequest<Task>;

        public class DeletePlayerCommandHandler : IRequestHandler<Command, Task>
        {
            private readonly IRepository<Player> _repository;

            public DeletePlayerCommandHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<Task> Handle(Command request, CancellationToken cancellationToken)
            {
                await _repository.DeleteAsync(request.PlayerId);
                return Task.CompletedTask;
            }
        }
    }
}
