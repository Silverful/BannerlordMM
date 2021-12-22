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

        public async Task<Guid> CreateAsync(T model)
        {
            await _dbContext.Set<T>().AddAsync(model);

            await _dbContext.SaveChangesAsync();

            return model.Id;
        }

        public async Task<IEnumerable<Guid>> CreateRangeAsync(IEnumerable<T> models)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Set<T>().AddRangeAsync(models);

                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch(Exception)
                {
                    await transaction.RollbackAsync();
                    throw;  
                }
            }

            return models.Select(m => m.Id).ToList();
        }

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
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Set<T>().RemoveRange(models);

                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var models = await GetRangeByIdsAsync(ids.ToList());
                    await DeleteRangeAsync(models);

                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>()
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                .Where(predicate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            return await _dbContext.Set<T>()
                .Where(x => ids.Contains(x.Id))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

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
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Set<T>().UpdateRange(models);

                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
