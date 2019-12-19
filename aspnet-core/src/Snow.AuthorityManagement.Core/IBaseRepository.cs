using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core
{
    public interface IBaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLamada">过滤条件</param>
        /// <returns></returns>
        bool IsExists(Expression<Func<T, bool>> whereLamada);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLamada">过滤条件</param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(Expression<Func<T, bool>> whereLamada);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>添加的实体</returns>
        T Add(T entity);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>添加的实体</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities">要添加的实体</param>
        /// <returns>添加的实体</returns>
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns></returns>
        bool Delete(T entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        bool DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns></returns>
        bool Edit(T entity);

        /// <summary>
        /// 获取满足条件的首行指定列数据
        /// </summary>
        /// <typeparam name="S">数据的类型</typeparam>
        /// <param name="whereLamada">过滤条件</param>
        /// <param name="selector">查询条件</param>
        /// <returns></returns>
        S LoadScalar<S>(Expression<Func<T, bool>> whereLamada, Expression<Func<T, int, S>> selectLamada);

        /// <summary>
        /// 获取满足条件的首行指定列数据
        /// </summary>
        /// <typeparam name="S">数据的类型</typeparam>
        /// <param name="whereLamada">过滤条件</param>
        /// <param name="selector">查询条件</param>
        /// <returns></returns>
        Task<S> LoadScalarAsync<S>(Expression<Func<T, bool>> whereLamada, Expression<Func<T, int, S>> selectLamada);

        /// <summary>
        /// 首条数据
        /// </summary>
        /// <param name="whereLamada">过滤表达式</param>
        /// <returns>满足条件的第一条数据</returns>
        T FirstOrDefault(Expression<Func<T, bool>> whereLamada, bool isAsNoTracking = false);

        /// <summary>
        /// 首条数据
        /// </summary>
        /// <param name="whereLamada">过滤表达式</param>
        /// <returns>满足条件的第一条数据</returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereLamada, bool isAsNoTracking = false);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLamada">过滤条件</param>
        /// <param name="isAsNoTracking">是否追踪查询结果(如果有select则忽略)</param>
        /// <returns></returns>
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLamada, bool isAsNoTracking = false);

        Task<List<T>> LoadListAsync(Expression<Func<T, bool>> whereLamada, bool isAsNoTracking = false);

        Task<Tuple<List<T>, int>> GetPagedAsync(int pageIndex, int pageSize
            , string wheres, object[] parameters, string orders);

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="whereLamada">过滤条件</param>
        /// <param name="orderbyLamada">排序条件</param>
        /// <param name="isASC">排序方式</param>
        /// <param name="isAsNoTracking">是否跟踪</param>
        /// <returns></returns>
        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLamada, Expression<Func<T, s>> orderbyLamada, bool isASC = true, bool isAsNoTracking = false);

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="orderbyLamada">排序条件</param>
        /// <param name="isASC">排序方式</param>
        /// <returns></returns>
        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, IQueryable<T> entities, Expression<Func<T, s>> orderbyLamada, bool isASC = true);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="entities">待分页数据</param>
        /// <returns></returns>
        IQueryable<T> LoadPageEntities(int pageIndex, int pageSize, IQueryable<T> entities);
    }
}