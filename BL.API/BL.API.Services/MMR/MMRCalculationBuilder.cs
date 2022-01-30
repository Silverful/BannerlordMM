using BL.API.Core.Domain.Match;

namespace BL.API.Services.MMR
{
    public class MMRCalculationBuilder
    {
        public static ICalculateMMRStrategy BuildMMRStrategy(Season season, BasicMMRCalculationProperties props)
        {
            ICalculateMMRStrategy strategy = season.Title switch
            {
                "Beta" => new BetaSeasonStrategy(props),
                _ => new BasicStrategy(props),
            };

            return strategy;
        }
    }
}
