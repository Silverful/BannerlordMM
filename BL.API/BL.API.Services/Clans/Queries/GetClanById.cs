using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Clan;
using BL.API.Core.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Clans.Queries
{
    public static class GetClanByIdQuery
    {
        public record Query(string ClanId) : IRequest<Clan>;

        public class GetAllPlayersQueryHandler : IRequestHandler<Query, Clan>
        {
            private readonly IRepository<Clan> _repository;

            public GetAllPlayersQueryHandler(IRepository<Clan> repository)
            {
                _repository = repository;
            }

            public async Task<Clan> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!Guid.TryParse(request.ClanId, out Guid id)) throw new GuidCantBeParsedException();

                var clan = await _repository.GetByIdAsync(id);

                if (clan == null) throw new NotFoundException();

                return clan;
            }
        }
    }
}
