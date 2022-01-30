using BL.API.Core.Domain.Match;

namespace BL.API.Services.MMR
{
    public interface ICalculateMMRStrategy
    {
        public double Execute(PlayerMatchRecord record);
    }
}
