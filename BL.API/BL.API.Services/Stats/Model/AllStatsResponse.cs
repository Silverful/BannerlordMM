using System.Collections.Generic;

namespace BL.API.Services.Stats.Model
{
    public class AllStatsResponse
    {
        public TopPlayersByClassStats TopPlayerByClassStats { get; set; }
        public IDictionary<string, decimal> IGLStats { get; set; }
        public IDictionary<string, decimal> DivisionStats { get; set; }
        public IDictionary<string, decimal> FactionStats { get; set; }
        public IEnumerable<PlayerByFactionWRItem> PlayersByFactionStats { get; set; }
        public IEnumerable<PlayerStatItemResponse> PlayerStats { get; set; }
    }
}
