using BL.API.Core.Domain.Match;

namespace BL.API.Services.MMR
{
    public class BasicStrategy : ICalculateMMRStrategy
    {
        private readonly BasicMMRCalculationProperties _mmrProps;

        public BasicStrategy(BasicMMRCalculationProperties settings)
        {
            _mmrProps = settings;
        }
        private int DefaultChange { get => _mmrProps.DefaultChange; }
        private double CalibrationIndexFactor { get => _mmrProps.CalibrationIndexFactor; }

        public double Execute(PlayerMatchRecord record)
        {
            var isWon = record.TeamIndex == record.Match.TeamWon ? 1 : 0;

            if (isWon == 0 && record.CalibrationIndex > 0)
            {
                return 0; //MMR does not decrease on calibration
            }

            var calibrationIndexAdjust = record.CalibrationIndex == 0 || record.CalibrationIndex == null ? 1 : (isWon == 0 ? 0 : CalibrationIndexFactor);

            return (isWon == 1 ? DefaultChange : -1 * DefaultChange) * calibrationIndexAdjust;
        }
    }
}
