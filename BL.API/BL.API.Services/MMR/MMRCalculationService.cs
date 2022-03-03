using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BL.API.Services.MMR
{
    public class MMRCalculationService : IMMRCalculationService
    {
        private readonly BasicMMRCalculationProperties _mmrProps;
        private readonly ISeasonResolverService _seasonResolverService;
        private readonly IMMRCalculationBuilder _mmrBuilder;

        public MMRCalculationService(IOptions<BasicMMRCalculationProperties> settings, 
            ISeasonResolverService seasonResolverService, 
            IMMRCalculationBuilder mmrBuilder)
        {
            _mmrProps = settings.Value;
            _seasonResolverService = seasonResolverService;
            _mmrBuilder = mmrBuilder;
        }
         
        public async Task<double> CalculateMMRChangeAsync(PlayerMatchRecord record)
        {
            var currentSeason = await _seasonResolverService.GetCurrentSeasonAsync(record.Match.RegionId.Value);

            var strategy = _mmrBuilder.BuildMMRStrategy(currentSeason, _mmrProps);

            var mmrChange = await strategy.ExecuteAsync(record);

            return mmrChange;
        }
    }
}
