using System.Collections.Generic;

namespace BL.API.Services.Clans.Queries
{
    public class ClanStatsItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string AvatarURL { get; set; }
        public int? GamesPlayed { get; set; }
        public double? AvgMMR { get; set; }
        public double? AvgSR { get; set; }
        public double? AvgWR { get; set; }
        public string LeaderNickname { get; set; }
        public IEnumerable<ClanMemberStatsItem> Roster { get; set; }
    }
}
