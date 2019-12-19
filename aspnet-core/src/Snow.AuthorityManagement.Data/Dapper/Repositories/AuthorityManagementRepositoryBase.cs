using Anc.Dapper;
using Anc.Dapper.Repositories;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Snow.AuthorityManagement.Data.Dapper.Repositories1
{
    public class AuthorityManagementRepositoryBase<TEntity, TPrimaryKey> : DapperRepositoryBase<TEntity, TPrimaryKey>
          where TEntity : class, IEntity<TPrimaryKey>
    {
        public AuthorityManagementRepositoryBase(DbContext context)
            : base(context)
        {
        }
    }

    /// <summary>
    /// Base class for custom repositories of the application. This is a shortcut of <see
    /// cref="AuthorityManagementRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class AuthorityManagementRepositoryBase<TEntity> : AuthorityManagementRepositoryBase<TEntity, int>, ISqlRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public AuthorityManagementRepositoryBase(DbContext context)
            : base(context)
        {
        }

        // Do not add any method here, add to the class above (since this inherits it)!!!
    }
}