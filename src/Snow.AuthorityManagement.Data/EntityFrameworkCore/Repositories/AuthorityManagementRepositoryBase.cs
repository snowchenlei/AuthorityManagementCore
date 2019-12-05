using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Anc.EntityFrameworkCore;
using Anc.EntityFrameworkCore.Repositories;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Snow.AuthorityManagement.Repositories
{
    public class AuthorityManagementRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<TEntity, TPrimaryKey>
         where TEntity : class, IEntity<TPrimaryKey>
    {
        public AuthorityManagementRepositoryBase(AuthorityManagementContext context)
            : base(context)
        {
        }

        // Add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application. This is a shortcut of <see
    /// cref="AuthorityManagementRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/>
    /// primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class AuthorityManagementRepositoryBase<TEntity> : AuthorityManagementRepositoryBase<TEntity, int>, ILambdaRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public AuthorityManagementRepositoryBase(AuthorityManagementContext context)
            : base(context)
        {
        }

        // Do not add any method here, add to the class above (since this inherits it)!!!
    }
}