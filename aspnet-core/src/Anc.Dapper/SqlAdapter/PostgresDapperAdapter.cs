using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Dapper.SqlAdapter
{
    /// <summary>
    /// Postgres适配器
    /// </summary>
    public partial class PostgresBaseAdapter : PostgresAdapter, IBaseSqlAdapter
    {
        public string GetPagedSql(string tableName, string orderBy, string selectColumns, string whereClause, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}