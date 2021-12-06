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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            return await _dbContext.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
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
    }
}
