using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Anc.EntityFrameworkCore;
using Anc.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Repository
{
    public class AuthorityManagementRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<TEntity, TPrimaryKey>
         where TEntity : class, IEntity<TPrimaryKey>
    {
        protected AuthorityManagementRepositoryBase(DbContext context)
            : base(context)
        {
        }

        // Add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application. This is a shortcut of <see
    /// cref="AuthorityManagementRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class AuthorityManagementRepositoryBase<TEntity> : AuthorityManagementRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        protected AuthorityManagementRepositoryBase(DbContext context)
            : base(context)
        {
        }

        // Do not add any method here, add to the class above (since this inherits it)!!!
    }
}