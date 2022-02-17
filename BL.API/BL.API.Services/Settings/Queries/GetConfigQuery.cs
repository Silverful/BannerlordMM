using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Settings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Settings
{
    public static class GetConfigQuery
    {
        public record Query(string configName) : IRequest<string>;

        public class GetConfigQueryHandler : IRequestHandler<Query, string>
        {
            private readonly IRepository<Configuration> _repository;

            public GetConfigQueryHandler(IRepository<Configuration> repository)
            {
                _repository = repository;
            }

            public async Task<string> Handle(Query request, CancellationToken cancellationToken)
            {
                var config = await _repository.GetFirstWhereAsync(c => c.ConfigName == request.configName);
                return config.Value;
            }
        }
    }
}
