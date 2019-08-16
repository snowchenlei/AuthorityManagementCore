using Snow.AuthorityManagement.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Snow.AuthorityManagement.Core;

namespace Snow.AuthorityManagement.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        protected readonly AuthorityManagementContext CurrentContext = null;

        public BaseRepository(AuthorityManagementContext context)
        {
            CurrentContext = context;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLamada">过滤条件</param>
        /// <returns></returns>
        public bool IsExists(Expression<Func<T, bool>> whereLamada)
        {
            return CurrentContext.Set<T>().Any(whereLamada);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLamada">过滤条件</param>
        /// <returns></returns>
        public Task<bool> IsExistsAsync(Expression<Func<T, bool>> whereLamada)
        {
            return CurrentContext.Set<T>().AnyAsync(whereLamada);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>添加的实体</returns>
        public T Add(T entity)
        {
            CurrentContext.Set<T>().Add(entity);
            return entity;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>添加的实体</returns>
        public async Task<T> AddAsync(T entity)
        {
            await CurrentContext.Set<T>().AddAsync(entity);

            return entity;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>添加的实体</returns>
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await CurrentContext.Set<T>().AddRangeAsync(entities);

            return entities;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            CurrentContext.Set<T>().Remove(entity);
            return true;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns></returns>
        public bool DeleteRange(IEnumerable<T> entities)
        {
            CurrentContext.Set<T>().RemoveRange(entities);
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">要修改的实体</param>
        /// <returns></returns>
        public bool Edit(T entity)
        {
            CurrentContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        /// <summary>
        /// 获取满足条件的首行指定列数据
        /// </summary>
        /// <typeparam name="S">数据的类型</typeparam>
        /// <param name="whereLamada">过滤条件</param>
        /// <param name="selector">查询条件</param>
        /// <returns></returns>
        public S LoadScalar<S>(Expression<Func<T, bool>> whereLamada, Expression<Func<T, int, S>> selectLamada)
        {
            return CurrentContext.Set<T>().Where(whereLamada).Select(selectLamada).FirstOrDefault();
        }

        /// <summary>
        /// 获取满足条件的首行指定列数据
        /// </summary>
        /// <typeparam name="S">数据的类型</typeparam>
        /// <param name="whereLamada">过滤条件</param>
        /// <param name="selector">查询条件</param>
        /// <returns></returns>
        public Task<S> LoadScalarAsync<S>(Expression<Func<T, bool>> whereLamada, Expression<Func<T, int, S>> selectLamada)
        {
            return CurrentContext.Set<T>().Where(whereLamada).Select(selectLamada).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 首条数据
        /// </summary>
        /// <param name="whereLamada">过滤表达式</param>
        /// <returns>满足条件的第一条数据</returns>
        public T FirstOrDefault(Expression<Func<T, bool>> whereLamada, bool isAsNoTracking = false)
        {
            if (isAsNoTracking)
            {
                return CurrentContext.Set<T>().AsNoTracking().FirstOrDefault(whereLamada);
            }
            else
            {
                return CurrentContext.Set<T>().FirstOrDefault(whereLamada);
            }
        }

        /// <summary>
        /// 首条数据
        /// </summary>
        /// <param name="whereLamada">过滤表达式</param>
        /// <returns>满足条件的第一条数据</returns>
        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereLamada, bool isAsNoTracking = false)
        {
            if (isAsNoTracking)
            {
                return CurrentContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(whereLamada);
            }
            else
            {
                return CurrentContext.Set<T>().FirstOrDefaultAsync(whereLamada);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLamada">过滤条件</param>
        /// <param name="isAsNoTracking">是否追踪查询结果(如果有select则忽略)</param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLamada, bool isAsNoTracking = false)
        {
            if (isAsNoTracking)
            {
                return CurrentContext.Set<T>().AsNoTracking().Where(whereLamada);
            }
            else
            {
                return CurrentContext.Set<T>().Where(whereLamada);
            }
        }

        public Task<List<T>> LoadListAsync(Expression<Func<T, bool>> whereLamada, bool isAsNoTracking = false)
        {
            if (isAsNoTracking)
            {
                return CurrentContext.Set<T>().AsNoTracking().Where(whereLamada).ToListAsync();
            }
            else
            {
                return CurrentContext.Set<T>().Where(whereLamada).ToListAsync();
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="wheres">过滤条件</param>
        /// <param name="parameters">参数</param>
        /// <param name="orders">排序方式</param>
        /// <returns></returns>
        public async Task<Tuple<List<T>, int>> GetPagedAsync(int pageIndex, int pageSize
            , string wheres, object[] parameters, string orders)
        {
            IQueryable<T> temp;
            if (String.IsNullOrEmpty(wheres))
            {
                temp = CurrentContext.Set<T>().AsNoTracking();
            }
            else
            {
                temp = CurrentContext.Set<T>().AsNoTracking().Where(wheres, parameters);
            }
            int totalCount = await temp.CountAsync();
            temp = temp.OrderBy(orders);
            var result = await temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Tuple<List<T>, int>(result, totalCount);
        }

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
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLamada, Expression<Func<T, s>> orderbyLamada, bool isASC = true, bool isAsNoTracking = false)
        {
            IQueryable<T> temp = null;
            if (isAsNoTracking)
            {
                temp = CurrentContext.Set<T>().AsNoTracking().Where(whereLamada);
            }
            else
            {
                temp = CurrentContext.Set<T>().Where(whereLamada);
            }
            totalCount = temp.Count();
            return LoadPageEntities(pageIndex, pageSize, temp, orderbyLamada, isASC);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="orderbyLamada">排序条件</param>
        /// <param name="isASC">排序方式</param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, IQueryable<T> entities, Expression<Func<T, s>> orderbyLamada, bool isASC = true)
        {
            if (isASC)
            {
                entities = entities.OrderBy(orderbyLamada);
            }
            else
            {
                entities = entities.OrderByDescending(orderbyLamada);
            }
            return LoadPageEntities(pageIndex, pageSize, entities);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="entities">待分页数据</param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities(int pageIndex, int pageSize, IQueryable<T> entities)
        {
            return entities.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}