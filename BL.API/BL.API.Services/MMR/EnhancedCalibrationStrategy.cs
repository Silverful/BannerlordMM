using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BL.API.Services.MMR
{
    public class EnhancedCalibrationStrategy : ICalculateMMRStrategy
    {
        private readonly BasicMMRCalculationProperties _mmrProps;
        private readonly Func<Guid, Task<double>> _avrScoreResolver;

        public EnhancedCalibrationStrategy(BasicMMRCalculationProperties settings, Func<Guid, Task<double>> avrScoreResolver = null)
        {
            _mmrProps = settings;
            _avrScoreResolver = avrScoreResolver;
        }

        private int DefaultChange { get => _mmrProps.DefaultChange; }
        private double CalibrationIndexFactor { get => _mmrProps.CalibrationIndexFactor; }

        public async Task<double> ExecuteAsync(PlayerMatchRecord record)
        {
            var isWon = record.TeamIndex == record.Match.TeamWon ? 1 : 0;

            var team1Score = record.RoundsWon;
            var team2Score = record.Match.PlayerRecords.Where(pr => pr.TeamIndex != record.TeamIndex).First().RoundsWon;

            var defaultChange = DefaultChange * Math.Abs(team1Score - team2Score);
            var calibrationIndexAdjust = record.CalibrationIndex == 0 || record.CalibrationIndex == null ? 1 : (isWon == 0 ? 0 : CalibrationIndexFactor);
            double bonusMMR = 0;

            if (record.CalibrationIndex > 0 && _avrScoreResolver != null && record.PlayerId.HasValue)
            {
                double exp = 0;
                double avgClassScore = 0;
                double avgPlayerScore = await _avrScoreResolver(record.PlayerId.Value);    

                switch (record.Player.MainClass)
                {
                    case PlayerClass.Archer:
                        avgClassScore = _mmrProps.AvgArcherScore;
                        exp = avgPlayerScore > avgClassScore ? _mmrProps.ArchPositiveExp : _mmrProps.ArcherNegativeExp;
                        break;
                    case PlayerClass.Infantry:
                        avgClassScore = _mmrProps.AvgInfScore;
                        exp = avgPlayerScore > avgClassScore ? _mmrProps.InfPositiveExp : _mmrProps.InfNegativeExp;
                        break;
                    case PlayerClass.Cavalry:
                        avgClassScore = _mmrProps.AvgCavScore;
                        exp = avgPlayerScore > avgClassScore ? _mmrProps.CavPositiveExp : _mmrProps.CavNegativExp;
                        break;
                }

                bonusMMR = Math.Pow(Math.Abs(avgClassScore - avgPlayerScore), exp) * _mmrProps.Factor; 
            } 

            return defaultChange * calibrationIndexAdjust + bonusMMR;
        }
    }
}
