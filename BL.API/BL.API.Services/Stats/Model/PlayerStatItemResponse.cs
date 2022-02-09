using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.API.Services.Stats.Model
{
    public class PlayerStatItemResponse
    {
        public string PlayerId { get; set; }
        public int Position { get; set; }
        public string Nickname { get; set; }
        public string Country { get; set; }
        public string IGL { get; set; }
        public string Clan { get; set; }
        public string MainClass { get; set; }
        public string SecondaryClass { get; set; }
        public string Rank { get; set; }
        public int? MMR { get; set; }
        public int? Played { get; set; }
        public int? Wins { get; set; }
        public double? WR { get; set; }
        public int? Rounds { get; set; }
        public int? Kills { get; set; }
        public double? KR { get; set; }
        public int? Assists { get; set; }
        public double? AR { get; set; }
        public double? KAR { get; set; }
        public int? Deaths { get; set; }
        public double? KD { get; set; }
        public double? KDA { get; set; }
        public int? Score { get; set; }
        public double? SR { get; set; }
        public int? MVP { get; set; }
        public double? MVPR { get; set; }
        public long? DiscordId { get; set; }

        public static PlayerStatItemResponse FromMatchRecordGrouping(Player player, IGrouping<Guid, PlayerMatchRecord> record, IDictionary<string, double> RankTable)
        {
            return new PlayerStatItemResponse
            {
                PlayerId = player.Id.ToString(),
                Nickname = player.Nickname,
                Country = player.Country,
                IGL = player.IsIGL ? "IGL" : "",
                Clan = player.Clan,
                MainClass = player.MainClass.ToString(),
                SecondaryClass = player.SecondaryClass.ToString(),
                DiscordId = player.DiscordId,
                Rank = RankTable.Where(x => x.Value < player.PlayerMMR.MMR).First().Key ?? "Classic",
                MMR = (int)player.PlayerMMR.MMR,
                Played = record?.Count() ?? 0,
                Wins = record?.Where(x => x.TeamIndex == x.Match.TeamWon).Count() ?? 0,
                WR = record == null ? 0 : record?.Where(x => x.TeamIndex == x.Match.TeamWon).Count() == 0 ? 0 : (double)record?.Where(x => x.TeamIndex == x.Match.TeamWon).Count() / record?.Count(),
                Rounds = record?.Sum(x => x.Match.RoundsPlayed) ?? 0,
                Kills = record?.Sum(x => x.Kills) ?? 0,
                KR = record == null ? 0 : (double)record?.Sum(x => x.Kills) / record?.Sum(x => x.Match.RoundsPlayed),
                Assists = record?.Sum(x => x.Assists) ?? 0,
                AR = record == null ? 0 : (double)record?.Sum(x => x.Assists) / record?.Sum(x => x.Match.RoundsPlayed) ?? 0,
                KAR = record == null ? 0 : (double)(record?.Sum(x => x.Kills) + record?.Sum(x => x.Assists)) / record?.Sum(x => x.Match.RoundsPlayed),
                Deaths = record?.Sum(x => x.Deaths) ?? 0,
                KD = record == null ? 0 : (double)record?.Sum(x => x.Kills) / record?.Sum(x => x.Deaths),
                KDA = record == null ? 0 : ((double)record?.Sum(x => x.Kills) + (double)record?.Sum(x => x.Assists)) / record?.Sum(x => x.Deaths),
                Score = record?.Sum(x => x.Score) ?? 0,
                SR = record == null ? 0 : (double)record?.Sum(x => x.Score) / record?.Sum(x => x.Match.RoundsPlayed),
                MVP = record?.Sum(x => x.MVPs) ?? 0,
                MVPR = record == null? 0 : (double)record?.Sum(x => x.MVPs) / record?.Sum(x => x.Match.RoundsPlayed)
            };
        }
    }
}
