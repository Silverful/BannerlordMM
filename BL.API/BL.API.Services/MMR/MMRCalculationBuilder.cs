using BL.API.Core.Domain.Match;
using BL.API.Services.Players.Queries;
using MediatR;
using System.Threading.Tasks;

namespace BL.API.Services.MMR
{
    public class MMRCalculationBuilder : IMMRCalculationBuilder
    {
        private readonly IMediator _mediator;

        public MMRCalculationBuilder(IMediator mediator)
        {
            _mediator = mediator;
        }


        public ICalculateMMRStrategy BuildMMRStrategy(Season season, BasicMMRCalculationProperties props)
        {
            ICalculateMMRStrategy strategy = season.Title switch
            {
                "Beta" => new BetaSeasonStrategy(props),
                "Test" => new EnhancedCalibrationStrategy(props, async id => await _mediator.Send(new GetPlayersAvgCalibrationScoreQuery.Query(id, null))),
                _ => new BasicStrategy(props),
            };

            return strategy;
        }
    }
}
