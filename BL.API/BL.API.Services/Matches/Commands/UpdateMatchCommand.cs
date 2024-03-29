﻿using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Exceptions;
using BL.API.Services.Utility;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace BL.API.Services.Matches.Commands
{
    public class UpdateMatchCommand : UpdateMatchRequest, IRequest<Task>
    {
        public class UpdateMatchCommandHandler : IRequestHandler<UpdateMatchCommand, Task>
        {
            private readonly IRepository<Match> _matchRepository;
            private readonly ILogger<UpdateMatchCommandHandler> _logger;
            private readonly ISeasonResolverService _seasonService;
            private readonly IMediator _mediator;
            private readonly ICacheProvider _cacheProvider;

            public UpdateMatchCommandHandler(IRepository<Match> matchRepository,
                ISeasonResolverService seasonService,
                IMediator mediator,
                ICacheProvider cacheProvider,
                ILogger<UpdateMatchCommandHandler> logger)
            {
                _matchRepository = matchRepository;
                _seasonService = seasonService;
                _mediator = mediator;
                _cacheProvider = cacheProvider;
                _logger = logger;
            }

            public async Task<Task> Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
            {
                var match = await _matchRepository.GetByIdAsync(request.MatchId, false, m => m.PlayerRecords);

                if (match == null) throw new NotFoundException();

                var matchSeason = await _seasonService.GetSeasonOnDateAsync(match.MatchDate, match.RegionId.Value);

                match.ScreenshotLink = request.ScreenshotLink;
                match.MatchDate = request.MatchDate;
                match.RoundsPlayed = request.RoundsPlayed;
                match.TeamWon = (byte)(request.Team1Records.First().RoundsWon > request.Team2Records.First().RoundsWon ? 1 : 2);

                //reduce old MMR changes

                var updatedRecords = request.Team1Records.Select(t1 => t1.ToPlayerMatchRecord(1))
                    .Concat(request.Team2Records.Select(t2 => t2.ToPlayerMatchRecord(2)))
                    .ToList();

                var oldRecords = match.PlayerRecords.ToList();

                updatedRecords.ForEach(r =>
                {
                    var sameRecord = oldRecords.Where(or => or.PlayerId == r.PlayerId && or.TeamIndex == r.TeamIndex).FirstOrDefault();

                    r.Match = match;
                    r.MatchId = match.Id;
                    r.MMRChange = sameRecord?.MMRChange;
                    r.Id = sameRecord?.Id ?? r.Id;

                    if (sameRecord != null)
                    {
                        r.Created = sameRecord.Created;
                        oldRecords.Remove(sameRecord);
                    }
                });

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //after match deletion all the next games should be recalculated
                    foreach (var oldRecord in oldRecords)
                    {
                        await _mediator.Send(new DeleteMatchRecordCommand.Query(oldRecord.Id));
                    }

                    match.PlayerRecords = updatedRecords;

                    await _matchRepository.UpdateAsync(match);

                    scope.Complete();
                }

                foreach (var rec in match.PlayerRecords)
                {
                    await _mediator.Send(new LoadPlayersRecordCommand.Query(rec));
                }

                _logger?.LogInformation($"Match updated {JsonSerializer.Serialize(match, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve })}");

                _cacheProvider.TryRemoveValue(CacheKeys.Stats + match.Region.ShortName);

                return Task.CompletedTask;
            }
        }
    }
}
