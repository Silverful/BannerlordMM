using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Commands
{
    public class DeletePlayerCommand
    {
        public record Command(string PlayerId) : IRequest<Task>;

        public class DeletePlayerCommandHandler : IRequestHandler<Command, Task>
        {
            private readonly IRepository<Player> _repository;
            private readonly ILogger<DeletePlayerCommandHandler> _logger;

            public DeletePlayerCommandHandler(IRepository<Player> repository, ILogger<DeletePlayerCommandHandler> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<Task> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!Guid.TryParse(request.PlayerId, out Guid id)) throw new GuidCantBeParsedException();

                var player = await _repository.GetByIdAsync(id);

                if (player == null) throw new NotFoundException();

                await _repository.DeleteAsync(id);

                _logger?.LogInformation($"Player deleted: {JsonSerializer.Serialize(player)}");

                return Task.CompletedTask;
            }
        }
    }
}
