using BL.API.Core.Domain.Match;

namespace BL.API.Core.Abstractions.Services
{
    public interface IMMRCalculationService
    {
        public int CalculateMMRChange(PlayerMatchRecord record);
    }
}
