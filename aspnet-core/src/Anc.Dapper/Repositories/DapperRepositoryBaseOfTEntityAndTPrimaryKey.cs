using Anc.Dapper.SqlAdapter;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Dapper.Repositories
{
    public class DapperRepositoryBase<TEntity, TPrimaryKey> : AncRepositoryBase<TEntity, TPrimaryKey>, ISqlRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly IDbConnection Connection;
        protected IDbTransaction Transaction;
        private static readonly IBaseSqlAdapter DefaultDapperAdapter = new SqlServerBaseAdapter();

        private static readonly Dictionary<string, IBaseSqlAdapter> DapperAdapterDictionary
            = new Dictionary<string, IBaseSqlAdapter>
            {
                ["sqlconnection"] = new SqlServerBaseAdapter(),
                ["sqlceconnection"] = new SqlCeServerBaseAdapter(),
                ["npgsqlconnection"] = new PostgresBaseAdapter(),
                ["sqliteconnection"] = new SQLiteBaseAdapter(),
                ["mysqlconnection"] = new MySqlBaseAdapter(),
                ["fbconnection"] = new FbBaseAdapter()
            };

        public DapperRepositoryBase(DbContext context)
        {
            Connection = context.DbConnection;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public virtual bool Exists(object condition)
        {
            string sql = GetExistsSql(condition);
            return Connection.ExecuteScalar<bool>(sql, condition);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public virtual async Task<bool> ExistsAsync(object condition)
        {
            string sql = GetExistsSql(condition);
            return (await Connection.ExecuteScalarAsync<int>(sql, condition)) > 0;
        }

        private string GetExistsSql(object condition)
        {
            return $"SELECT ISNULL(({BuildQuerySql(condition, selectPart: " TOP(1) 'true ' ")}), 'false')";
        }

        public virtual int Count(object condition)
        {
            string tableName = GetTableName(typeof(TEntity));
            string sql = BuildQuerySql(condition, tableName, "COUNT(1)");
            return Connection.ExecuteScalar<int>(sql, condition);
        }

        public virtual Task<int> CountAsync(object condition)
        {
            string tableName = GetTableName(typeof(TEntity));
            string sql = BuildQuerySql(condition, tableName, "COUNT(1)");
            return Connection.ExecuteScalarAsync<int>(sql, condition);
        }

        public virtual long Int64Count(object condition)
        {
            string tableName = GetTableName(typeof(TEntity));
            string sql = BuildQuerySql(condition, tableName, "COUNT(1)");
            return Connection.ExecuteScalar<long>(sql, condition);
        }

        public virtual Task<long> Int64CountAsync(object condition)
        {
            string tableName = GetTableName(typeof(TEntity));
            string sql = BuildQuerySql(condition, tableName, "COUNT(1)");
            return Connection.ExecuteScalarAsync<long>(sql, condition);
        }

        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            object condition = CreateEqualityExpressionForId(id);
            string sql = BuildQuerySql(condition);
            return Connection.QueryFirst<TEntity>(sql, condition);
        }

        public override Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            object condition = CreateEqualityExpressionForId(id);
            string sql = BuildQuerySql(condition);
            return Connection.QueryFirstAsync<TEntity>(sql, condition);
        }

        public TEntity FirstOrDefault(object condition)
        {
            string sql = BuildQuerySql(condition);
            return Connection.QueryFirstOrDefault<TEntity>(sql, condition);
        }

        public Task<TEntity> FirstOrDefaultAsync(object condition)
        {
            string sql = BuildQuerySql(condition);
            return Connection.QueryFirstOrDefaultAsync<TEntity>(sql, condition);
        }

        public override TEntity Single(TPrimaryKey id)
        {
            object condition = CreateEqualityExpressionForId(id);
            string sql = BuildQuerySql(condition);
            return Connection.QuerySingle<TEntity>(sql, condition);
        }

        public override Task<TEntity> SingleAsync(TPrimaryKey id)
        {
            object condition = CreateEqualityExpressionForId(id);
            string sql = BuildQuerySql(condition);
            return Connection.QuerySingleAsync<TEntity>(sql, condition);
        }

        public TEntity Single(object condition)
        {
            string sql = BuildQuerySql(condition);
            return Connection.QuerySingle<TEntity>(sql, condition);
        }

        public Task<TEntity> SingleAsync(object condition)
        {
            string sql = BuildQuerySql(condition);
            return Connection.QuerySingleAsync<TEntity>(sql, condition);
        }

        public override TEntity Get(TPrimaryKey id)
        {
            TEntity entity = Connection.Get<TEntity>(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public override async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            TEntity entity = await Connection.GetAsync<TEntity>(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public override IEnumerable<TEntity> GetAllEnumerable()
        {
            return Connection.GetAll<TEntity>();
        }

        public override Task<IEnumerable<TEntity>> GetAllEnumerableAsync()
        {
            return Connection.GetAllAsync<TEntity>();
        }

        public Task<List<TEntity>> GetAllListAsync(object condition)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 单表分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="orderBy">排序</param>
        /// <param name="selectColumns">查询列</param>
        /// <param name="whereClause">过滤条件</param>
        /// <param name="paras">参数</param>
        /// <returns>分页数据</returns>
        public virtual async Task<Tuple<IEnumerable<TEntity>, int>> GetPagedImplAsync(int pageIndex, int pageSize,
            string orderBy, string selectColumns = "*", string whereClause = "", Dictionary<string, object> paras = null)
        {
            IBaseSqlAdapter sqlAdapter = GetFormatter(Connection);
            string tableName = GetTableName(typeof(TEntity));
            var count = await CountAsync(null);
            string sql = sqlAdapter.GetPagedSql(tableName, orderBy, selectColumns, whereClause, pageIndex, pageSize);
            var entities = await Connection.QueryAsync<TEntity>(sql);
            return new Tuple<IEnumerable<TEntity>, int>(entities, count);
        }

        public override void Delete(TPrimaryKey key)
        {
            TEntity entity = Get(key);
            Delete(entity);
        }

        public override async Task DeleteAsync(TPrimaryKey key)
        {
            TEntity entity = await GetAsync(key);
            await DeleteAsync(entity);
        }

        public override void Delete(TEntity entity)
        {
            Connection.Delete(entity);
        }

        public override Task DeleteAsync(TEntity entity)
        {
            return Connection.DeleteAsync(entity);
        }

        public override void Delete(IEnumerable<TEntity> entities)
        {
            Connection.Delete(entities);
        }

        public override Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            return Connection.DeleteAsync(entities);
        }

        public override TEntity Insert(TEntity entity)
        {
            Connection.Insert(entity);
            return entity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Connection.InsertAsync(entity);
            return entity;
        }

        public override IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities)
        {
            long num = Connection.Insert(entities);
            return entities;
        }

        public override async Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities)
        {
            await Connection.InsertAsync(entities);
            return entities;
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            Connection.Insert(entity);
            return default(TPrimaryKey);
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            await Connection.InsertAsync(entity);
            return default(TPrimaryKey);
        }

        public override TEntity Update(TEntity entity)
        {
            Connection.Update(entity);
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Connection.UpdateAsync(entity);
            return entity;
        }

        #region 辅助方法

        protected virtual object CreateEqualityExpressionForId(TPrimaryKey id)
        {
            return new
            {
                ID = id
            };
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected IBaseSqlAdapter GetFormatter(IDbConnection connection)
        {
            string name = connection.GetType().Name.ToLower();

            return !DapperAdapterDictionary.ContainsKey(name)
                ? DefaultDapperAdapter
                : DapperAdapterDictionary[name];
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetTableName(Type type)
        {
            string name;
            var info = type;
            var tableAttrName =
                info.GetCustomAttribute<TableAttribute>(false)?.Name
                ?? (info.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic)?.Name;
            if (tableAttrName != null)
            {
                name = tableAttrName;
            }
            else
            {
                name = type.Name + "s";
                if (type.GetTypeInfo().IsInterface && name.StartsWith("I"))
                    name = name.Substring(1);
            }

            return name;
        }

        /// <summary>
        /// 构造查询Sql
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="selectPart">查询列</param>
        /// <param name="isOr">Or?</param>
        /// <returns>查询语句</returns>
        private string BuildQuerySql(object condition, string selectPart = "*", bool isOr = false)
        {
            string tableName = GetTableName(typeof(TEntity));
            return BuildQuerySql(condition, tableName, selectPart, isOr);
        }

        /// <summary>
        /// 构造查询Sql
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="table">表名</param>
        /// <param name="selectPart">查询列</param>
        /// <param name="isOr">Or?</param>
        /// <returns>查询语句</returns>
        protected string BuildQuerySql(object condition, string table, string selectPart = "*", bool isOr = false)
        {
            // var conditionObj = condition as object;
            var properties = GetProperties(condition);
            if (properties.Count == 0)
            {
                return $"SELECT {selectPart} FROM {table}";
            }

            var separator = isOr ? " OR " : " AND ";
            var wherePart = string.Join(separator, properties.Select(p => p + " = @" + p));

            return $"SELECT {selectPart} FROM {table} WHERE {wherePart}";
        }

        private List<string> GetProperties(object obj)
        {
            if (obj == null)
            {
                return new List<string>();
            }
            if (obj is DynamicParameters)
            {
                return (obj as DynamicParameters).ParameterNames.ToList();
            }
            return GetPropertyInfos(obj).Select(x => x.Name).ToList();
        }

        private static List<PropertyInfo> GetPropertyInfos(object obj)
        {
            if (obj == null)
            {
                return new List<PropertyInfo>();
            }

            var properties = obj.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public).ToList();
            return properties;
        }

        #endregion 辅助方法
    }
}