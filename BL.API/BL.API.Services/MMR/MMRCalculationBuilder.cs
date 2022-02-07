using BL.API.Core.Domain.Match;
using MediatR;

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
                "Test" => new EnhancedCalibrationStrategy(props, _mediator),
                _ => new BasicStrategy(props),
            };

            return strategy;
        }
    }
}
