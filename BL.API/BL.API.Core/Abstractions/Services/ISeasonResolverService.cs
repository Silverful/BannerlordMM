using BL.API.Core.Domain.Match;
using System;
using System.Threading.Tasks;

namespace BL.API.Core.Abstractions.Services
{
    public interface ISeasonResolverService
    {
        Task<Season> GetCurrentSeasonAsync();
        Task<Season> GetSeasonOnDateAsync(DateTime date);
    }
}
