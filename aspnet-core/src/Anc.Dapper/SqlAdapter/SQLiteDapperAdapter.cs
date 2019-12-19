using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Dapper.SqlAdapter
{
    /// <summary>
    /// SQLite适配器
    /// </summary>
    public partial class SQLiteBaseAdapter : SQLiteAdapter, IBaseSqlAdapter
    {
        public string GetPagedSql(string tableName, string orderBy, string selectColumns, string whereClause, int pageIndex,
            int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}