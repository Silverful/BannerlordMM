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
        Task<IEnumerable<T>> GetAllAsync(bool isRead = true, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(Guid id, bool isRead = true, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids, bool isRead = true, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, bool isRead = true, params Expression<Func<T, object>>[] includes);
        Task<T> GetFirstWhereAsync(Expression<Func<T, bool>> predicate, bool isRead = true, params Expression<Func<T, object>>[] includes);

        Task<Guid> CreateAsync(T model);
        Task<IEnumerable<Guid>> CreateRangeAsync(IEnumerable<T> models);

        Task DeleteAsync(T model);
        Task DeleteRangeAsync(IEnumerable<T> models);

        Task UpdateAsync(T model);
        Task UpdateRangeAsync(IEnumerable<T> models);

        Task DeleteAsync(Guid id);
        Task DeleteRangeAsync(IEnumerable<Guid> ids);

        Task SaveAsync();
        void Detach(T entity);
        void DetachRange(IEnumerable<T> entities);
    }
}
