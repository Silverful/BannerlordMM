using BL.API.Core.Domain.Match;
using System;
using System.Linq;

namespace BL.API.Services.Players.Queries
{
    public class PlayerStatItemResponse
    {
        public string PlayerId { get; set; }
        public string Nickname { get; set; }
        public string Country { get; set; }
        public string Clan { get; set; }
        public string MainClass { get; set; }
        public string SecondaryClass { get; set; }
        public long? DiscordId { get; set; }
        public int? MMR { get; set; }
        public int? MatchesPlayed { get; set; }
        public int? MatchesWon { get; set; }
        public decimal? WR { get; set; }
        public int? RoundsPlayed { get; set; }
        public int? Kills { get; set; }
        public decimal? KR { get; set; }
        public int? Assists { get; set; }
        public decimal? AR { get; set; }
        public decimal? KAR { get; set; }
        public int? TotalScore { get; set; }
        public decimal? SR { get; set; }
        public int? MVP { get; set; }
        public decimal? MVPR { get; set; }

        public static PlayerStatItemResponse FromMatchRecordGrouping(IGrouping<Guid, PlayerMatchRecord> record)
        {
            return new PlayerStatItemResponse 
            {
                PlayerId = record.First().Player?.Id.ToString(),
                Nickname = record.First().Player?.Nickname,
                Country = record.First().Player?.Country,
                Clan = record.First().Player?.Clan,
                MainClass = record.First().Player.MainClass.ToString(),
                SecondaryClass = record.First().Player?.SecondaryClass.ToString(),
                DiscordId = record.First().Player?.DiscordId,
                MMR = record.First().Player?.PlayerMMR,
                MatchesPlayed = record.Count(),
                MatchesWon = record.Where(x => x.TeamIndex == x.Match.TeamWon).Count(),
                WR = record.Where(x => x.TeamIndex == x.Match.TeamWon).Count() == 0 ? 0 : (decimal)record.Where(x => x.TeamIndex == x.Match.TeamWon).Count() / record.Count(), //TODO make default view with premade params
                RoundsPlayed = record.Sum(x => x.Match.RoundsPlayed),
                Kills = record.Sum(x => x.Kills),
                KR = (decimal)record.Sum(x => x.Kills) / record.Sum(x => x.Match.RoundsPlayed),
                Assists = record.Sum(x => x.Assists),
                AR = (decimal)record.Sum(x => x.Assists) / record.Sum(x => x.Match.RoundsPlayed),
                KAR = (decimal)(record.Sum(x => x.Kills) + record.Sum(x => x.Assists)) / record.Sum(x => x.Match.RoundsPlayed),
                TotalScore = record.Sum(x => x.Score),
                SR = (decimal)record.Sum(x => x.Score) / record.Sum(x => x.Match.RoundsPlayed),
                MVP = record.Sum(x => x.MVPs),
                MVPR = (decimal)record.Sum(x => x.MVPs) / record.Sum(x => x.Match.RoundsPlayed)
            };

        }
    }
}
