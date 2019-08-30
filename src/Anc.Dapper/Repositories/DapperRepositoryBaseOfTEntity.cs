using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Anc.Dapper.Repositories
{
    public class DapperRepositoryBase<TEntity> : DapperRepositoryBase<TEntity, int>, IDapperRepository<TEntity>
       where TEntity : class, IEntity<int>
    {
        public DapperRepositoryBase(IDbConnection connection) : base(connection)
        {
        }
    }
}