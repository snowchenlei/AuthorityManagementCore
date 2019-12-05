using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Domain.Repositories
{
    public interface ILambdaRepository<TEntity> : ILambdaRepository<TEntity, int>, IRepository<TEntity> where TEntity : class, IEntity<int>
    {
    }
}