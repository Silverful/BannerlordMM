using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Exceptions;
using BL.API.Services.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Commands
{
    public class DeleteClanCommand
    {
        public record Query(Guid ClanId) : IRequest<Task>;

        public class DeleteClanCommandHandler : IRequestHandler<Query, Task>
        {
            private readonly IRepository<Clan> _repository;
            private readonly IRepository<ClanJoinRequest> _requests;
            private readonly ILogger<DeleteClanCommandHandler> _logger;

            public DeleteClanCommandHandler(IRepository<Clan> repository, 
                IRepository<ClanJoinRequest> requests,
                ILogger<DeleteClanCommandHandler> logger)
            {
                _repository = repository;
                _requests = requests;
                _logger = logger;
            }

            public async Task<Task> Handle(Query request, CancellationToken cancellationToken)
            {
                var clan = await _repository.GetByIdAsync(request.ClanId);

                if (clan == null) throw new NotFoundException();

                var requests = await _requests.GetWhereAsync(r => r.ToClanId == clan.Id);
                await _requests.DeleteRangeAsync(requests);
                await _repository.DeleteAsync(request.ClanId);

                _logger?.LogInformationSerialized($"Clan deleted", clan);

                return Task.CompletedTask;
            }
        }
    }
}
