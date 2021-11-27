using BL.API.Core.Abstractions.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.API.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T : IBaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids);

        Task<Guid> CreateAsync(T model);

        Task DeleteAsync(T model);

        Task UpdateAsync(T model);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetFirstWhereAsync(Expression<Func<T, bool>> predicate);

        Task SaveAsync();
    }
}
