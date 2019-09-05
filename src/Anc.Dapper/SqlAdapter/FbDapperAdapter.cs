using System;

namespace Anc.Dapper.SqlAdapter
{
    /// <summary>
    /// Fb适配器
    /// </summary>
    public partial class FbBaseAdapter : FbAdapter, IBaseSqlAdapter
    {
        public string GetPagedSql(string tableName, string orderBy, string selectColumns, string whereClause, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}