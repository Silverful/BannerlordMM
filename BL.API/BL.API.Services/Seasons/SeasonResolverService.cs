using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.Core.Domain.Match;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BL.API.Services.Seasons
{


    public class SeasonResolverService : ISeasonResolverService
    {
        private IEnumerable<Season> _seasons;
        private IRepository<Season> _seasonRep;
        private Timer _updateTimer;
        private ILogger<SeasonResolverService> _logger;

        public SeasonResolverService(IRepository<Season> seasonRep, ILogger<SeasonResolverService> logger)
        {
            _seasonRep = seasonRep;
            _logger = logger;
            _updateTimer = new Timer(1000 * 60);
            _updateTimer.Elapsed += Timer_Elapsed;
            _updateTimer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _seasons = _seasonRep.GetAllAsync().GetAwaiter().GetResult();
            }
            catch (ObjectDisposedException)
            {
                _updateTimer?.Stop();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Season cant be updated");
            }
        }

        public async Task<Season> GetCurrentSeasonAsync()
        {
            if (_seasons == null)
            {
                _seasons = await _seasonRep.GetAllAsync();
            };

            return _seasons.Where(s => s.OnGoing == true).FirstOrDefault();
        }

        public async Task<Season> GetSeasonOnDateAsync(DateTime date)
        {
            if (_seasons == null)
            {
                _seasons = await _seasonRep.GetAllAsync();
            };

            return _seasons.Where(s => s.Started <= date && s.Finished >= date).FirstOrDefault() ?? _seasons.Where(s => s.OnGoing == true).FirstOrDefault();
        }
    }
}
