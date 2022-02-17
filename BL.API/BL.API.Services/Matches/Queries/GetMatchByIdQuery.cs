using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Match;
using BL.API.Core.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Matches.Queries
{
    public static class GetMatchByIdQuery
    {
        public record Query(Guid matchId) : IRequest<MatchResponse>;

        public class GetMatchByIdQueryHandler : IRequestHandler<Query, MatchResponse>
        {
            private readonly IRepository<Match> _repository;

            public GetMatchByIdQueryHandler(IRepository<Match> repository)
            {
                _repository = repository;
            }

            public async Task<MatchResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var match = await _repository.GetByIdAsync(request.matchId);

                if (match == null) throw new NotFoundException();

                var response = MatchResponse.FromMatch(match);

                return response;
            }
        }
    }
}
