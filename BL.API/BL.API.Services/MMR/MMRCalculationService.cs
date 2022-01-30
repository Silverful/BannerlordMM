using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BL.API.Services.MMR
{
    public class MMRCalculationService : IMMRCalculationService
    {
        private readonly BasicMMRCalculationProperties _mmrProps;
        private readonly ISeasonResolverService _seasonResolverService;

        public MMRCalculationService(IOptions<BasicMMRCalculationProperties> settings, ISeasonResolverService seasonResolverService)
        {
            _mmrProps = settings.Value;
            _seasonResolverService = seasonResolverService;
        }
         
        public async Task<double> CalculateMMRChangeAsync(PlayerMatchRecord record)
        {
            var currentSeason = await _seasonResolverService.GetCurrentSeasonAsync();

            var strategy = MMRCalculationBuilder.BuildMMRStrategy(currentSeason, _mmrProps);

            var mmrChange = strategy.Execute(record);

            return mmrChange;
        }
    }
}
