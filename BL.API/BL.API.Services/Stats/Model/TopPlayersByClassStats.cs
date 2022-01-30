using System.Collections.Generic;

namespace BL.API.Services.Stats.Model
{
    public class TopPlayersByClassStats
    {
        public IDictionary<string, double> Archer { get; set; }
        public IDictionary<string, double> Infantry { get; set; }
        public IDictionary<string, double> Cavalry { get; set; }
    }
}
