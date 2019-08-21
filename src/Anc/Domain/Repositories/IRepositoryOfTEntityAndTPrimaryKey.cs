using Anc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Domain.Repositories
{
    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : class, IEntity<TPrimaryKey>
    {
        #region 通用

        TEntity Single(TPrimaryKey id);

        Task<TEntity> SingleAsync(TPrimaryKey id);

        TEntity Get(TPrimaryKey id);

        IEnumerable<TEntity> GetAllEnumerable();

        Task<IEnumerable<TEntity>> GetAllEnumerableAsync();

        Task<TEntity> GetAsync(TPrimaryKey id);

        Task<Tuple<List<TEntity>, int>> GetPagedAsync(int pageIndex, int pageSize
            , string wheres, object[] parameters, string orders);

        TEntity FirstOrDefault(TPrimaryKey id);

        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        TEntity Insert(TEntity entity);

        IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities);

        Task<TEntity> InsertAsync(TEntity entity);

        Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities);

        TPrimaryKey InsertAndGetId(TEntity entity);

        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        void Delete(TPrimaryKey key);

        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        Task DeleteAsync(TPrimaryKey key);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(IEnumerable<TEntity> entities);

        #endregion 通用

        ///// <summary>
        ///// 是否存在
        ///// </summary>
        ///// <param name="whereLamada">过滤条件</param>
        ///// <returns></returns>
        //bool IsExists(Expression<Func<TEntity, bool>> whereLamada);

        ///// <summary>
        ///// 是否存在
        ///// </summary>
        ///// <param name="whereLamada">过滤条件</param>
        ///// <returns></returns>
        //Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> whereLamada);

        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="entity">要添加的实体</param>
        ///// <returns>添加的实体</returns>
        //TEntity Add(TEntity entity);

        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="entity">要添加的实体</param>
        ///// <returns>添加的实体</returns>
        //Task<TEntity> AddAsync(TEntity entity);

        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="entities">要添加的实体</param>
        ///// <returns>添加的实体</returns>
        //Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="entity">要删除的实体</param>
        ///// <returns></returns>
        //bool Delete(TEntity entity);

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="entities">实体集合</param>
        ///// <returns></returns>
        //bool DeleteRange(IEnumerable<TEntity> entities);

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="entity">要修改的实体</param>
        ///// <returns></returns>
        //bool Edit(TEntity entity);

        ///// <summary>
        ///// 获取满足条件的首行指定列数据
        ///// </summary>
        ///// <typeparam name="S">数据的类型</typeparam>
        ///// <param name="whereLamada">过滤条件</param>
        ///// <param name="selector">查询条件</param>
        ///// <returns></returns>
        //S LoadScalar<S>(Expression<Func<TEntity, bool>> whereLamada, Expression<Func<TEntity, int, S>> selectLamada);

        ///// <summary>
        ///// 获取满足条件的首行指定列数据
        ///// </summary>
        ///// <typeparam name="S">数据的类型</typeparam>
        ///// <param name="whereLamada">过滤条件</param>
        ///// <param name="selector">查询条件</param>
        ///// <returns></returns>
        //Task<S> LoadScalarAsync<S>(Expression<Func<TEntity, bool>> whereLamada, Expression<Func<TEntity, int, S>> selectLamada);

        ///// <summary>
        ///// 首条数据
        ///// </summary>
        ///// <param name="whereLamada">过滤表达式</param>
        ///// <returns>满足条件的第一条数据</returns>
        //TEntity FirstOrDefault(Expression<Func<TEntity, bool>> whereLamada, bool isAsNoTracking = false);

        ///// <summary>
        ///// 首条数据
        ///// </summary>
        ///// <param name="whereLamada">过滤表达式</param>
        ///// <returns>满足条件的第一条数据</returns>
        //Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereLamada, bool isAsNoTracking = false);

        ///// <summary>
        ///// 查询
        ///// </summary>
        ///// <param name="whereLamada">过滤条件</param>
        ///// <param name="isAsNoTracking">是否追踪查询结果(如果有select则忽略)</param>
        ///// <returns></returns>
        //IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLamada, bool isAsNoTracking = false);

        //Task<List<TEntity>> LoadListAsync(Expression<Func<TEntity, bool>> whereLamada, bool isAsNoTracking = false);

        //Task<Tuple<List<TEntity>, int>> GetPagedAsync(int pageIndex, int pageSize
        //    , string wheres, object[] parameters, string orders);

        ///// <summary>
        ///// 分页
        ///// </summary>
        ///// <typeparam name="s"></typeparam>
        ///// <param name="pageIndex">当前页码</param>
        ///// <param name="pageSize">每页记录数</param>
        ///// <param name="totalCount">总记录数</param>
        ///// <param name="whereLamada">过滤条件</param>
        ///// <param name="orderbyLamada">排序条件</param>
        ///// <param name="isASC">排序方式</param>
        ///// <param name="isAsNoTracking">是否跟踪</param>
        ///// <returns></returns>
        //IQueryable<TEntity> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<TEntity, bool>> whereLamada, Expression<Func<TEntity, s>> orderbyLamada, bool isASC = true, bool isAsNoTracking = false);

        ///// <summary>
        ///// 分页
        ///// </summary>
        ///// <typeparam name="s"></typeparam>
        ///// <param name="pageIndex">当前页码</param>
        ///// <param name="pageSize">每页记录数</param>
        ///// <param name="orderbyLamada">排序条件</param>
        ///// <param name="isASC">排序方式</param>
        ///// <returns></returns>
        //IQueryable<TEntity> LoadPageEntities<s>(int pageIndex, int pageSize, IQueryable<TEntity> entities, Expression<Func<TEntity, s>> orderbyLamada, bool isASC = true);

        ///// <summary>
        ///// 分页
        ///// </summary>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">每页记录数</param>
        ///// <param name="entities">待分页数据</param>
        ///// <returns></returns>
        //IQueryable<TEntity> LoadPageEntities(int pageIndex, int pageSize, IQueryable<TEntity> entities);
    }
}