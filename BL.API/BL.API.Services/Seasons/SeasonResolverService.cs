using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace BL.API.Services.Seasons
{


    public class SeasonResolverService : ISeasonResolverService
    {
        private Season _currentSeason;
        private IRepository<Season> _seasonRep;
        private Timer _updateTimer;
        private ILogger<SeasonResolverService> _logger;

        public SeasonResolverService(IRepository<Season> seasonRep, ILogger<SeasonResolverService> logger)
        {
            _seasonRep = seasonRep;
            _logger = logger;
            var timer = new Timer(1000 * 60 * 60);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _currentSeason = _seasonRep.GetFirstWhereAsync(x => x.OnGoing).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Season cant be updated");
            }
        }

        public async Task<Season> GetCurrentSeasonAsync()
        {
            if (_currentSeason == null)
            {
                _currentSeason = await _seasonRep.GetFirstWhereAsync(x => x.OnGoing);
            };

            return _currentSeason;
        }
    }
}
