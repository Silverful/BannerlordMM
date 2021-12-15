using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using Microsoft.Extensions.Options;
using System.Linq;

namespace BL.API.Services.MMR
{
    public class MMRCalculationService : IMMRCalculationService
    {
        private readonly BasicMMRCalculationProperties _mmrProps;

        public MMRCalculationService(IOptions<BasicMMRCalculationProperties> settings)
        {
            _mmrProps = settings.Value;
        }

        public int CalculateMMRChange(PlayerMatchRecord record)
        {
            var isWon = record.TeamIndex == record.Match.TeamWon ? 1 : 0;

            if (isWon == 0 && record.CalibrationIndex > 0)
            {
                return 0; //MMR do decrease on calibration
            }

            var calibrationIndexAdjust = record.CalibrationIndex + 1 == 1 ? 1 : (isWon == 0 ? 0 : 4);
            var totalTeamScore = record.Match.PlayerRecords.Where(pr => pr.TeamIndex == record.TeamIndex).Sum(r => r.Score) ?? 0;

            var mmrChange =
                isWon * -1 + 1 //loss mmr constant increase
                + (isWon - 1) * _mmrProps.AdditionalBank * 2 / 6 //loss punishment
                + 2 * _mmrProps.DefaultChange * isWon - _mmrProps.DefaultChange //regular mmr change
                + _mmrProps.AdditionalBank * record.Score / totalTeamScore; //% from additional bank

            if (!mmrChange.HasValue || mmrChange == 0)
            {
                mmrChange = _mmrProps.DefaultChange * isWon == 1 ? 1 : -1;
            }

            mmrChange *= calibrationIndexAdjust;

            return mmrChange.Value;
        }
    }
}
