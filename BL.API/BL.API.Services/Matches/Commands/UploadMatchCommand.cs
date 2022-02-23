using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Settings;
using BL.API.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Commands
{
    public class UploadMatchCommand : UploadMatchRequest, IRequest<Guid> 
    {
        public class UploadMatchCommandHandler: IRequestHandler<UploadMatchCommand, Guid>
        {
            private readonly IRepository<Match> _matchRepository;
            private readonly IRepository<Region> _regionRepository;
            private readonly ILogger<UploadMatchCommandHandler> _logger;
            private readonly ISeasonResolverService _seasonService;
            private readonly IMediator _mediator;

            public UploadMatchCommandHandler(IRepository<Match> matchRepository,
                IRepository<Region> regionRepository,
                ISeasonResolverService seasonService,
                IMediator mediator,
                ILogger<UploadMatchCommandHandler> logger)
            {
                _matchRepository = matchRepository;
                _regionRepository = regionRepository;
                _seasonService = seasonService;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<Guid> Handle(UploadMatchCommand request, CancellationToken cancellationToken)
            {
                var season = await _seasonService.GetCurrentSeasonAsync();
                var region = await _regionRepository.GetFirstWhereAsync(r => r.ShortName == request.RegionShortName);

                if (region == null)
                {
                    throw new NotFoundException();
                }

                var match = new Match()
                {
                    ScreenshotLink = request.ScreenshotLink,
                    SeasonId = season.Id,
                    RegionId = region.Id,
                    MatchDate = request.MatchDate,
                    RoundsPlayed = request.RoundsPlayed,
                    TeamWon = (byte)(request.Team1Records.First().RoundsWon > request.Team2Records.First().RoundsWon ? 1 : 2)
                };

                match.PlayerRecords = request.Team1Records.Select(t1 => t1.ToPlayerMatchRecord(1))
                    .Concat(request.Team2Records.Select(t2 => t2.ToPlayerMatchRecord(2)))
                    .ToList();
                
                await _matchRepository.CreateAsync(match);

                match = await _matchRepository.GetByIdAsync(match.Id, false);

                foreach (var record in match.PlayerRecords)
                {
                    record.Match = match;
                }

                foreach (var rec in match.PlayerRecords)
                {
                    await _mediator.Send(new LoadPlayersRecordCommand.Query(rec));
                }

                _logger?.LogInformation($"Match created {JsonSerializer.Serialize(match, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                return match.Id;
            }
        }
    }
}
