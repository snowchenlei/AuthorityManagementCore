using Anc.Collections.Extensions;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Anc.Reflection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Anc.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryBase<TEntity, TPrimaryKey> :
        AncRepositoryBase<TEntity, TPrimaryKey>, ILambdaRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        /// <summary>
        /// Gets DbQuery for given entity.
        /// </summary>
        public virtual DbQuery<TEntity> DbQueryTable => Context.Query<TEntity>();

        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public readonly DbContext Context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public EfCoreRepositoryBase(DbContext context)
        {
            Context = context;
        }

        private static readonly ConcurrentDictionary<Type, bool> EntityIsDbQuery =
            new ConcurrentDictionary<Type, bool>();

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Any(predicate);
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().AnyAsync(predicate);
        }

        public override TEntity Single(TPrimaryKey id)
        {
            return GetAll().Single(CreateEqualityExpressionForId(id));
        }

        public override Task<TEntity> SingleAsync(TPrimaryKey id)
        {
            return GetAll().SingleAsync(CreateEqualityExpressionForId(id));
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().SingleAsync(predicate);
        }

        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public override Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefaultAsync(predicate);
        }

        public TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return Table.Max(selector);
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return Table.MaxAsync(selector);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Count(predicate);
        }

        public long Int64Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().CountAsync(predicate);
        }

        public Task<long> Int64CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCountAsync(predicate);
        }

        public override TEntity Get(TPrimaryKey id)
        {
            TEntity entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public override async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            TEntity entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual Tuple<List<TEntity>, int> GetPaged(int pageIndex, int pageSize, string wheres, object[] parameters, string orders)
        {
            IQueryable<TEntity> temp;
            if (string.IsNullOrEmpty(wheres))
            {
                temp = GetAll().AsNoTracking();
            }
            else
            {
                temp = GetAll().AsNoTracking().Where(wheres, parameters);
            }
            int totalCount = temp.Count();
            temp = temp.OrderBy(orders);
            var result = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new Tuple<List<TEntity>, int>(result, totalCount);
        }

        public virtual async Task<Tuple<List<TEntity>, int>> GetPagedAsync(int pageIndex, int pageSize, string wheres, object[] parameters, string orders)
        {
            IQueryable<TEntity> temp;
            if (string.IsNullOrEmpty(wheres))
            {
                temp = GetAll().AsNoTracking();
            }
            else
            {
                temp = GetAll().AsNoTracking().Where(wheres, parameters);
            }
            int totalCount = await temp.CountAsync();
            temp = temp.OrderBy(orders);
            var result = await temp.Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();
            return new Tuple<List<TEntity>, int>(result, totalCount);
        }

        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return GetAll().ToListAsync();
        }

        public override IEnumerable<TEntity> GetAllEnumerable()
        {
            return GetAll().ToList();
        }

        public override async Task<IEnumerable<TEntity>> GetAllEnumerableAsync()
        {
            return await GetAll().ToListAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public override void Delete(TPrimaryKey key)
        {
            var entity = GetFromChangeTrackerOrNull(key);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(key);
            if (entity != null)
            {
                Delete(entity);
                return;
            }
        }

        public override async Task DeleteAsync(TPrimaryKey key)
        {
            var entity = GetFromChangeTrackerOrNull(key);
            if (entity != null)
            {
                await DeleteAsync(entity);
                return;
            }

            entity = FirstOrDefault(key);
            if (entity != null)
            {
                await DeleteAsync(entity);
                return;
            }
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override void Delete(IEnumerable<TEntity> entities)
        {
            AttachIfNot(entities);
            Table.RemoveRange(entities);
        }

        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            return (await Table.AddAsync(entity)).Entity;
        }

        public override IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities)
        {
            Table.AddRange(entities);
            return entities;
        }

        public override async Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities)
        {
            await Table.AddRangeAsync(entities);
            return entities;
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                Context.SaveChanges();
            }

            return entity.ID;
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (MayHaveTemporaryKey(entity) || entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.ID;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        protected virtual void AttachIfNot(IEnumerable<TEntity> entities)
        {
            var entry = Context.ChangeTracker.Entries().Where(ent => entities.Contains(ent.Entity));
            if (entry != null)
            {
                return;
            }

            Table.AttachRange(entities);
        }

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).ID)
                );

            return entry?.Entity as TEntity;
        }

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            if (EntityIsDbQuery.GetOrAdd(typeof(TEntity), key => Context.GetType().GetProperties().Any(property =>
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbQuery<>)) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                        typeof(IEntity<>)) &&
                    property.PropertyType.GetGenericArguments().Any(x => x == typeof(TEntity)))))
            {
                return DbQueryTable.AsQueryable();
            }

            return Table.AsQueryable();
        }

        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            //var lambdaParam = Expression.Parameter(typeof(TEntity));

            //var leftExpression = Expression.PropertyOrField(lambdaParam, "ID");

            //Expression<Func<object>> closure = () => id;
            //var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            //var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            //return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);

            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var leftExpression = Expression.PropertyOrField(lambdaParam, "ID");

            var idValue = Convert.ChangeType(id, typeof(TPrimaryKey));

            Expression<Func<object>> closure = () => idValue;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        private static bool MayHaveTemporaryKey(TEntity entity)
        {
            if (typeof(TPrimaryKey) == typeof(byte))
            {
                return true;
            }

            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(entity.ID) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(entity.ID) <= 0;
            }

            return false;
        }
    }
}