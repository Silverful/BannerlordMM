using BL.API.Core.Domain.Match;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BL.API.Services.MMR
{
    public class BetaSeasonStrategy : ICalculateMMRStrategy
    {
        private readonly BasicMMRCalculationProperties _mmrProps;

        public BetaSeasonStrategy(BasicMMRCalculationProperties settings)
        {
            _mmrProps = settings;
        }

        private int DefaultChange { get => _mmrProps.DefaultChange; }
        private int AdditionalBank { get => _mmrProps.AdditionalBank; }
        private double CalibrationIndexFactor { get => _mmrProps.CalibrationIndexFactor; }

        public double Execute(PlayerMatchRecord record)
        {
            var isWon = record.TeamIndex == record.Match.TeamWon ? 1 : 0;

            if (isWon == 0 && record.CalibrationIndex > 0)
            {
                return 0; //MMR does not decrease on calibration
            }

            var calibrationIndexAdjust = record.CalibrationIndex + 1 == 1 ? 1 : (isWon == 0 ? 0 : CalibrationIndexFactor);

            if (AdditionalBank == 0)
            {
                return (isWon == 1 ? DefaultChange : -1 * DefaultChange) * calibrationIndexAdjust;
            }

            var totalTeamScore = record.Match.PlayerRecords.Where(pr => pr.TeamIndex == record.TeamIndex).Sum(r => r.Score);

            double? mmrChange = null;

            try
            {
                mmrChange =
                isWon * -1 + 1 //loss mmr constant increase
                + (isWon - 1) * AdditionalBank * 2 / 6 //loss punishment
                + 2 * DefaultChange * isWon - DefaultChange //regular mmr change
                + AdditionalBank * record.Score / totalTeamScore; //% from additional bank
            }
            catch (Exception)
            {
                mmrChange = CalculateWithDefaultFormula(isWon);
            }

            mmrChange *= calibrationIndexAdjust;

            return mmrChange.Value;
        }

        public Task<double> ExecuteAsync(PlayerMatchRecord record)
        {
            var result = Execute(record);

            return Task.FromResult(result);
        }

        private double CalculateWithDefaultFormula(int isWon)
        {
            return DefaultChange * (isWon == 1 ? 1 : -1);
        }
    }
}
