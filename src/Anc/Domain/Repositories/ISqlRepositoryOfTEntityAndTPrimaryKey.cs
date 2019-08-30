using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Domain.Repositories
{
    public interface ISqlRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
    }
}