using BL.API.Core.Domain.Match;
using System;
using System.Threading.Tasks;

namespace BL.API.Core.Abstractions.Services
{
    public interface ISeasonResolverService
    {
        Task<Season> GetCurrentSeasonAsync(Guid regionId);
        Task<Season> GetSeasonOnDateAsync(DateTime date, Guid regionId);
    }
}
