using BL.API.Core.Domain.Match;
using System.Threading.Tasks;

namespace BL.API.Core.Abstractions.Services
{
    public interface IMMRCalculationService
    {
        public Task<double> CalculateMMRChangeAsync(PlayerMatchRecord record);
    }
}
