namespace BL.API.Services.MMR
{
    public class BasicMMRCalculationProperties
    {
        public int DefaultChange { get; set; }
        public int AdditionalBank { get; set; }

        public double AvgCavScore { get; set; }
        public double CavPositiveExp { get; set; }
        public double CavNegativExp { get; set; }
        public double AvgInfScore { get; set; }
        public double InfPositiveExp { get; set; }
        public double InfNegativeExp { get; set; }
        public double AvgArcherScore { get; set; }
        public double ArchPositiveExp { get; set; }
        public double ArcherNegativeExp { get; set; }
        public double Factor { get; set; }
        public double CalibrationIndexFactor { get; set; }
    }
}
