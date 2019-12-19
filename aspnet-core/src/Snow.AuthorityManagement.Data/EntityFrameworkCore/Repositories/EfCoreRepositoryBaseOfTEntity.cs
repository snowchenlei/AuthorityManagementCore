using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Snow.AuthorityManagement.Data.EntityFrameworkCore.Repositories.EfCoreRepositoryBaseOfTEntityAndTPrimaryKey;

namespace Snow.AuthorityManagement.Data.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryBase<TEntity> : EfCoreRepositoryBase<TEntity, int>, ILambdaRepository<TEntity>, IRepository<TEntity>
       where TEntity : class, IEntity<int>
    {
        public EfCoreRepositoryBase(AuthorityManagementContext context)
            : base(context)
        {
        }
    }
}