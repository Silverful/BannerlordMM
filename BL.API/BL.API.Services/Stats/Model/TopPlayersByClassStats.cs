using System.Collections.Generic;

namespace BL.API.Services.Stats.Model
{
    public class TopPlayersByClassStats
    {
        public IDictionary<string, decimal> Archer { get; set; }
        public IDictionary<string, decimal> Infantry { get; set; }
        public IDictionary<string, decimal> Cavalry { get; set; }
    }
}
