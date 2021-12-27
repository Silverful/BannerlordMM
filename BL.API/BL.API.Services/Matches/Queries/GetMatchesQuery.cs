using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Queries
{
    public static class GetMatchesQuery 
    {
        public record Query() : IRequest<IEnumerable<MatchResponse>>;

        public class GetMatchesQueryHandler : IRequestHandler<Query, IEnumerable<MatchResponse>>
        {
            private readonly IRepository<Match> _repository;

            public GetMatchesQueryHandler(IRepository<Match> repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<MatchResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var matchRecords = await _repository.GetAllAsync();

                var response = matchRecords.Select(m => MatchResponse.FromMatch(m)).ToList();

                return response;
            }
        }
    }
}
