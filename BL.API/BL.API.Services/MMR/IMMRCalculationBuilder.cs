using BL.API.Core.Domain.Match;

namespace BL.API.Services.MMR
{
    public interface IMMRCalculationBuilder
    {
        public ICalculateMMRStrategy BuildMMRStrategy(Season season, BasicMMRCalculationProperties props);
    }
}
