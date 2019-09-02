using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Domain.Repositories
{
    public abstract class AncRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public abstract TEntity FirstOrDefault(TPrimaryKey id);

        public abstract Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        public abstract TEntity Single(TPrimaryKey id);

        public abstract Task<TEntity> SingleAsync(TPrimaryKey id);

        public abstract TEntity Get(TPrimaryKey id);

        public abstract Task<TEntity> GetAsync(TPrimaryKey id);

        public abstract IEnumerable<TEntity> GetAllEnumerable();

        public abstract Task<IEnumerable<TEntity>> GetAllEnumerableAsync();

        public abstract Task<Tuple<List<TEntity>, int>> GetPagedAsync(int pageIndex, int pageSize, string wheres, object[] parameters, string orders);

        public abstract TEntity Insert(TEntity entity);

        public abstract IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities);

        public abstract TPrimaryKey InsertAndGetId(TEntity entity);

        public abstract Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        public abstract Task<TEntity> InsertAsync(TEntity entity);

        public abstract Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities);

        public abstract void Delete(TPrimaryKey key);

        public abstract void Delete(TEntity entity);

        public abstract void Delete(IEnumerable<TEntity> entities);

        public abstract Task DeleteAsync(TPrimaryKey key);

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            Delete(entities);
            return Task.CompletedTask;
        }

        public abstract TEntity Update(TEntity entity);

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }
    }
}