using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BL.API.Services.Matches.Commands
{
    public class DeleteMatchCommand
    {
        public record Query(Guid MatchId) : IRequest<Task>;

        public class DeleteMatchCommandHandler : IRequestHandler<Query, Task>
        {
            private readonly IRepository<Match> _matches;
            private readonly IMediator _mediator;
            private readonly ILogger<DeleteMatchCommandHandler> _logger;

            public DeleteMatchCommandHandler(IRepository<Match> matches, IMediator mediator, ILogger<DeleteMatchCommandHandler> logger)
            {
                _matches = matches;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<Task> Handle(Query request, CancellationToken cancellationToken)
            {
                var match = await _matches.GetByIdAsync(request.MatchId, false);

                if (match == null) throw new NotFoundException();

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                foreach (var record in match.PlayerRecords)
                {
                    await _mediator.Send(new DeleteMatchRecordCommand.Query(record.Id));
                }

                await _matches.DeleteAsync(request.MatchId);

                scope.Complete();

                _logger.LogInformation($"Match deleted: {JsonSerializer.Serialize(match, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return Task.CompletedTask;
            }
        }
    }
}
