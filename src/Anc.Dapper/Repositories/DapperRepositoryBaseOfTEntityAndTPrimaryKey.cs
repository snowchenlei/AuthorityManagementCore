using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Dapper.Repositories
{
    public class DapperRepositoryBase<TEntity, TPrimaryKey> : AncRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly IDbConnection Connection;
        protected IDbTransaction Transaction;

        public DapperRepositoryBase(DbContext context)
        {
            Connection = context.DbConnection;
        }

        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            return Get(id);
        }

        public override Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return GetAsync(id);
        }

        public override TEntity Single(TPrimaryKey id)
        {
            return Get(id);
        }

        public override Task<TEntity> SingleAsync(TPrimaryKey id)
        {
            return GetAsync(id);
        }

        public override TEntity Get(TPrimaryKey id)
        {
            return Connection.Get<TEntity>(id);
        }

        public override Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return Connection.GetAsync<TEntity>(id);
        }

        public override IEnumerable<TEntity> GetAllEnumerable()
        {
            return Connection.GetAll<TEntity>();
        }

        public override Task<IEnumerable<TEntity>> GetAllEnumerableAsync()
        {
            return Connection.GetAllAsync<TEntity>();
        }

        public override Task<Tuple<List<TEntity>, int>> GetPagedAsync(int pageIndex, int pageSize, string wheres, object[] parameters, string orders)
        {
            throw new NotImplementedException();
        }

        public override void Delete(TPrimaryKey key)
        {
            TEntity entity = Get(key);
            Delete(entity);
        }

        public override async Task DeleteAsync(TPrimaryKey key)
        {
            TEntity entity = await GetAsync(key);
            await DeleteAsync(entity);
        }

        public override void Delete(TEntity entity)
        {
            Connection.Delete(entity);
        }

        public override Task DeleteAsync(TEntity entity)
        {
            return Connection.DeleteAsync(entity);
        }

        public override void Delete(IEnumerable<TEntity> entities)
        {
            Connection.Delete(entities);
        }

        public override Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            return Connection.DeleteAsync(entities);
        }

        public override TEntity Insert(TEntity entity)
        {
            Connection.Insert(entity);
            return entity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Connection.InsertAsync(entity);
            return entity;
        }

        public override IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities)
        {
            long num = Connection.Insert(entities);
            return entities;
        }

        public override async Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities)
        {
            await Connection.InsertAsync(entities);
            return entities;
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            Connection.Insert(entity);
            return default(TPrimaryKey);
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            await Connection.InsertAsync(entity);
            return default(TPrimaryKey);
        }

        public override TEntity Update(TEntity entity)
        {
            Connection.Update(entity);
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Connection.UpdateAsync(entity);
            return entity;
        }
    }
}