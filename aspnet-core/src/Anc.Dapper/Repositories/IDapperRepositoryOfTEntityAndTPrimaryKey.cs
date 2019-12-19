using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Dapper.Repositories
{
    public interface IDapperRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
    }
}