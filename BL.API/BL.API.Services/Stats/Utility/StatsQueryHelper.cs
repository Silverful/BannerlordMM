using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Stats.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.API.Services.Stats.Utility
{
    public static class StatsQueryHelper
    {
        public static IEnumerable<PlayerStatItemResponse> GetPlayersStats(IEnumerable<Player> players, IEnumerable<PlayerMatchRecord> matchRecords)
        {
            var groupedMatchRecords = from record in matchRecords
                                      where record.PlayerId.HasValue
                                      group record by record.PlayerId.Value into g
                                      join p in players on g.Key equals p.Id
                                      select PlayerStatItemResponse.FromMatchRecordGrouping(p, g);

            return groupedMatchRecords;
        }

        public static PlayerStatItemResponse GetPlayerStats(Player player, IEnumerable<PlayerMatchRecord> matchRecords)
        {
            var records =
                from record in matchRecords
                group record by record.PlayerId.Value into g
                select g;

            var stats = PlayerStatItemResponse.FromMatchRecordGrouping(player, records.FirstOrDefault());
            return stats;
        }
    }
}
