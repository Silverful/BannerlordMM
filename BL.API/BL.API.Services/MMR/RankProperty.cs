namespace BL.API.Services.MMR
{
    public class RankProperty
    {
        public string Title { get; set; }
        public double? Percentile { get; set; }
        public int Position { get; set; }
        public double? Threshold { get; set; }
        public bool IsBottomRank { get; set; }
    }
}
