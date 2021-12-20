using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.API.Services.Stats.Model
{
    public class AllStatsResponse
    {
        public TopPlayersByClassStats TopPlayerByClassStats { get; set; }
        public IDictionary<string, decimal> IGLStats { get; set; }
        public IDictionary<string, decimal> DivisionStats { get; set; }
        public IDictionary<string, decimal> FactionStats { get; set; }
        public IEnumerable<PlayerStatItemResponse> PlayerStats { get; set; }
    }
}
