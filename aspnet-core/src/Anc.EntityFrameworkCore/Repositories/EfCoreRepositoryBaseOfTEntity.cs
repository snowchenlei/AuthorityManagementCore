using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Anc.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryBase<TEntity> : EfCoreRepositoryBase<TEntity, int>, ILambdaRepository<TEntity>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public EfCoreRepositoryBase(DbContext context)
            : base(context)
        {
        }
    }
}