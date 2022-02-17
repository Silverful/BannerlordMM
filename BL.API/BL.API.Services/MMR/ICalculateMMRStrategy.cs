using BL.API.Core.Domain.Match;
using System.Threading.Tasks;

namespace BL.API.Services.MMR
{
    public interface ICalculateMMRStrategy
    {
        public Task<double> ExecuteAsync(PlayerMatchRecord record);
    }
}
