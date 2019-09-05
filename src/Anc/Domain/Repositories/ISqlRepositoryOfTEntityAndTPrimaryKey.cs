using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Domain.Repositories
{
    public interface ISqlRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        bool Exists(object condition);

        Task<bool> ExistsAsync(object condition);

        int Count(object condition);

        Task<int> CountAsync(object condition);

        long Int64Count(object condition);

        Task<long> Int64CountAsync(object condition);

        TEntity Single(object condition);

        Task<TEntity> SingleAsync(object condition);

        TEntity FirstOrDefault(object condition);

        Task<TEntity> FirstOrDefaultAsync(object condition);

        Task<Tuple<IEnumerable<TEntity>, int>> GetPagedImplAsync(int pageIndex, int pageSize,
            string orderBy, string selectColumns = "*", string whereClause = "", Dictionary<string, object> paras = null);

        Task<List<TEntity>> GetAllListAsync(object condition);
    }
}