using Anc.Domain.Entities;
using Anc.Domain.Repositories;
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

        public DapperRepositoryBase(IDbConnection connection)
        {
            Connection = connection;
        }

        public override void Delete(TPrimaryKey key)
        {
            throw new NotImplementedException();
        }

        public override void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(TPrimaryKey key)
        {
            throw new NotImplementedException();
        }

        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            throw new NotImplementedException();
        }

        public override Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            throw new NotImplementedException();
        }

        public override TEntity Get(TPrimaryKey id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TEntity> GetAllEnumerable()
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TEntity>> GetAllEnumerableAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<TEntity> GetAsync(TPrimaryKey id)
        {
            throw new NotImplementedException();
        }

        public override Task<Tuple<List<TEntity>, int>> GetPagedAsync(int pageIndex, int pageSize, string wheres, object[] parameters, string orders)
        {
            throw new NotImplementedException();
        }

        public override TEntity Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override TEntity Single(TPrimaryKey id)
        {
            throw new NotImplementedException();
        }

        public override Task<TEntity> SingleAsync(TPrimaryKey id)
        {
            throw new NotImplementedException();
        }

        public override TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}