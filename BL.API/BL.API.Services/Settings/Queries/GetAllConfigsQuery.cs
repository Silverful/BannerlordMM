using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.Settings;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL.API.Services.Settings
{
    public static class GetAllConfigsQuery
    {
        public record Query(string RegionShortName) : IRequest<IEnumerable<ConfigurationResponse>>;

        public class GetAllConfigsQueryHandler : IRequestHandler<Query, IEnumerable<ConfigurationResponse>>
        {
            private readonly IRepository<Configuration> _repository;

            public GetAllConfigsQueryHandler(IRepository<Configuration> repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<ConfigurationResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                return (await _repository.GetWhereAsync(s => s.Region.ShortName == request.RegionShortName))
                    .Select(c => new ConfigurationResponse { ConfigName = c.ConfigName, Value = c.Value })
                    .ToList();
            }
        }

        public class ConfigurationResponse
        {
            public string ConfigName { get; set; }
            public string Value { get; set; }
        }
    }
}
