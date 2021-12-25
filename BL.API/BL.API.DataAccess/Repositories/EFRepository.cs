using BL.API.Core.Abstractions.Model;
using BL.API.Core.Abstractions.Repositories;
using BL.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.API.DataAccess.Repositories
{
    public class EFRepository<T> : IRepository<T>
        where T : class, IBaseEntity
    {
        private EFContext _dbContext;

        public EFRepository(EFContext context) : base()
        {
            _dbContext = context;
        }

        #region Create
        public async Task<Guid> CreateAsync(T model)
        {
            await _dbContext.Set<T>().AddAsync(model);

            await _dbContext.SaveChangesAsync();

            return model.Id;
        }

        public async Task<IEnumerable<Guid>> CreateRangeAsync(IEnumerable<T> models)
        {

            await _dbContext.Set<T>().AddRangeAsync(models);

            await _dbContext.SaveChangesAsync();



            return models.Select(m => m.Id);
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(T model)
        {
            _dbContext.Set<T>().Remove(model);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            await DeleteAsync(entity);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> models)
        {
            _dbContext.Set<T>().RemoveRange(models);

            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var models = await GetRangeByIdsAsync(ids.ToList(), false);
            await DeleteRangeAsync(models);

            await _dbContext.SaveChangesAsync();

        }
        #endregion

        #region Get
        public async Task<IEnumerable<T>> GetAllAsync(bool isRead)
        {
            return isRead ?
                await _dbContext.Set<T>()
                .AsNoTracking()
                .ToListAsync()
                :
                await _dbContext.Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id, bool isRead)
        {
            return isRead ?
                await _dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
                :
                await _dbContext.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetFirstWhereAsync(Expression<Func<T, bool>> predicate, bool isRead)
        {
            return isRead ? 
                await _dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate)
                :
                await _dbContext.Set<T>()
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids, bool isRead)
        {
            return isRead ?
                await _dbContext.Set<T>()
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync()
                :
                await _dbContext.Set<T>()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, bool isRead)
        {
            return isRead ? 
                await _dbContext.Set<T>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync()
                : 
                await _dbContext.Set<T>()
                .Where(predicate)
                .ToListAsync();
        }
        #endregion

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T model)
        {
            _dbContext.Set<T>().Update(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> models)
        {
            _dbContext.Set<T>().UpdateRange(models);

            await _dbContext.SaveChangesAsync();
        }

        public void Detach(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public void DetachRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }
        }
    }
}
