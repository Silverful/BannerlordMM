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
        public decimal? KR { get; set; }
        public int? Assists { get; set; }
        public decimal? AR { get; set; }
        public decimal? KAR { get; set; }
        public int? TotalScore { get; set; }
        public decimal? SR { get; set; }
        public int? MVP { get; set; }
        public decimal? MVPR { get; set; }
    }
}
