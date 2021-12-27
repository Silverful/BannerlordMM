using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Player;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Players.Queries
{
    public static class GetNicknamesQuery
    {
        public record Query() : IRequest<IEnumerable<PlayerNickname>>;

        public class GetNicknamesQueryHandler : IRequestHandler<Query, IEnumerable<PlayerNickname>>
        {
            private readonly IRepository<Player> _repository;

            public GetNicknamesQueryHandler(IRepository<Player> repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<PlayerNickname>> Handle(Query request, CancellationToken cancellationToken)
            {
                var nicknames = (await _repository.GetAllAsync())
                    .Select(p => new PlayerNickname 
                    { 
                        PlayerId = p.Id,
                        Nickname = p.Nickname
                    });

                return nicknames;
            }
        }

        public class PlayerNickname
        {
            public Guid PlayerId { get; set; }
            public string Nickname { get; set; }
        }
    }
}
