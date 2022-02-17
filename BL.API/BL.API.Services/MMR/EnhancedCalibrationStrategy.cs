using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Services.Players.Queries;
using MediatR;
using System;
using System.Threading.Tasks;

namespace BL.API.Services.MMR
{
    public class EnhancedCalibrationStrategy : ICalculateMMRStrategy
    {
        private readonly BasicMMRCalculationProperties _mmrProps;
        private readonly IMediator _mediator;

        public EnhancedCalibrationStrategy(BasicMMRCalculationProperties settings, IMediator mediator)
        {
            _mmrProps = settings;
            _mediator = mediator;
        }

        private int DefaultChange { get => _mmrProps.DefaultChange; }
        private double CalibrationIndexFactor { get => _mmrProps.CalibrationIndexFactor; }

        public async Task<double> ExecuteAsync(PlayerMatchRecord record)
        {
            var isWon = record.TeamIndex == record.Match.TeamWon ? 1 : 0;
            var isOnCalibration = record.CalibrationIndex.HasValue && record.CalibrationIndex > 0;

            var team1Score = record.RoundsWon;
            var team2Score = record.Match.RoundsPlayed - team1Score;

            var defaultChange = DefaultChange * Math.Abs(team1Score - team2Score);
            var calibrationIndexAdjust = CalculateCalibrationAdjustment(record.CalibrationIndex);
            var isWonAdjust = isWon == 1 ?
                isOnCalibration ? CalibrationIndexFactor : 1 //x4 for win on calibration
                :
                isOnCalibration ? 0 : -1; //-1 for loss and 0 for calibration loss
            double bonusMMR = 0;

            if (isOnCalibration && _mediator != null && record.PlayerId.HasValue)
            {
                double exp = 0;
                double avgClassScore = 0;
                double avgPlayerScore = await _mediator.Send(new GetPlayersAvgCalibrationScoreQuery.Query(record.PlayerId.Value, null));
                var player = record.Player ?? await _mediator.Send(new GetPlayerByIdQuery.Query(record.PlayerId.Value.ToString()));

                switch (player.MainClass)
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

                if (avgPlayerScore > 1000)
                {
                    avgPlayerScore = 1000;
                }

                var factor = (avgPlayerScore > avgClassScore ? 1 : -1) * _mmrProps.Factor;

                bonusMMR = Math.Pow(Math.Abs(avgClassScore - avgPlayerScore), exp) * factor;
            }

            return defaultChange * isWonAdjust + bonusMMR * calibrationIndexAdjust;
        }

        private double CalculateCalibrationAdjustment(int? calibrationIndex)
        {
            switch (10 - calibrationIndex + 1)
            {
                case 1:
                    return 5;
                case >= 2 and <= 6:
                    return 1;
                default:
                    return 0;
            }
        } 
    }
}
