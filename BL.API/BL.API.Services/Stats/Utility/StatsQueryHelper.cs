using System.Collections.Generic;

namespace BL.API.Services.Stats.Utility
{
    public static class StatsQueryHelper
    {
        private static Dictionary<string, decimal> _rankMultipliers = new Dictionary<string, decimal>()
        {
            ["Beast"] = 0.97M,
            ["Diamond"] = 0.75M,
            ["Platinum"] = 0.63M,
            ["Gold"] = 0.51M,
            ["Silver"] = 0.4M,
            ["Bronze"] = 0.28M,
            ["Iron"] = 0.14M,
            ["Classic"] = 0M,
            ["Wood"] = -1M
        };

        public static Dictionary<string, decimal> RankMultipliers { get => _rankMultipliers; }
    }
}
