using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Domain.Repositories
{
    public interface ILambdaRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLamada">过滤条件</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TEntity, bool>> whereLamada);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLamada">过滤条件</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> whereLamada);

        int Count(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        long Int64Count(Expression<Func<TEntity, bool>> predicate);

        Task<long> Int64CountAsync(Expression<Func<TEntity, bool>> predicate);

        TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector);

        Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector);

        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        Tuple<List<TEntity>, int> GetPaged(int pageIndex, int pageSize, string wheres, object[] parameters, string orders);

        Task<Tuple<List<TEntity>, int>> GetPagedAsync(int pageIndex, int pageSize, string wheres, object[] parameters, string orders);
    }
}