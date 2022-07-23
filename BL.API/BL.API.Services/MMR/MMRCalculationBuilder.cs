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
            var algo = season.MMRAlgorithm ?? MMRAlgorithm.Enhanced;

            ICalculateMMRStrategy strategy = algo switch
            {
                MMRAlgorithm.Beta => new BetaSeasonStrategy(props),
                MMRAlgorithm.Basic => new BasicStrategy(props),
                MMRAlgorithm.Enhanced => new EnhancedCalibrationStrategy(props, _mediator),
                _ => new BasicStrategy(props)
            };

            return strategy;
        }
    }
}
