using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Domain.Repositories
{
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {
    }
}