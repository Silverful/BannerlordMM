using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BL.API.Services.Matches.Commands
{
    public class UploadMatchCommand : UploadMatchRequest, IRequest<Guid> 
    {
        public class UploadMatchCommandHandler: IRequestHandler<UploadMatchCommand, Guid>
        {
            private readonly IRepository<Match> _matchRepository;
            private readonly ILogger<UploadMatchCommandHandler> _logger;
            private readonly ISeasonResolverService _seasonService;
            private readonly IMediator _mediator;

            public UploadMatchCommandHandler(IRepository<Match> matchRepository, 
                ISeasonResolverService seasonService,
                IMediator mediator,
                ILogger<UploadMatchCommandHandler> logger)
            {
                _matchRepository = matchRepository;
                _seasonService = seasonService;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<Guid> Handle(UploadMatchCommand request, CancellationToken cancellationToken)
            {
                var season = await _seasonService.GetCurrentSeasonAsync();

                var match = new Match()
                {
                    ScreenshotLink = request.ScreenshotLink,
                    SeasonId = season.Id,
                    MatchDate = request.MatchDate,
                    RoundsPlayed = request.RoundsPlayed,
                    TeamWon = (byte)(request.Team1Records.First().RoundsWon > request.Team2Records.First().RoundsWon ? 1 : 2)
                };

                match.PlayerRecords = request.Team1Records.Select(t1 => t1.ToPlayerMatchRecord(1))
                    .Concat(request.Team2Records.Select(t2 => t2.ToPlayerMatchRecord(2)))
                    .ToList();


                foreach (var record in match.PlayerRecords)
                {
                    record.Match = match;
                }
                
                await _matchRepository.CreateAsync(match);

                foreach (var rec in match.PlayerRecords)
                {
                    await _mediator.Send(new ReloadPlayersRecordsCommand.Query(rec));
                }

                _logger?.LogInformation($"Match created {JsonSerializer.Serialize(match, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return match.Id;
            }
        }
    }
}
