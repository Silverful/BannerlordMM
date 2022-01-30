using System.Collections.Generic;

namespace BL.API.Services.Stats.Utility
{
    public static class StatsQueryHelper
    {
        private static Dictionary<string, double> _rankMultipliers = new Dictionary<string, double>()
        {
            ["Beast"] = 0.97,
            ["Diamond"] = 0.75,
            ["Platinum"] = 0.63,
            ["Gold"] = 0.51,
            ["Silver"] = 0.4,
            ["Bronze"] = 0.28,
            ["Iron"] = 0.14,
            ["Classic"] = 0,
            ["Wood"] = -1
        };

        public static Dictionary<string, double> RankMultipliers { get => _rankMultipliers; }
    }
}
