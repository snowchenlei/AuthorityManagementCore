namespace Anc.Dapper.SqlAdapter
{
    /// <summary>
    /// SqlCeServer适配器
    /// </summary>
    public partial class SqlCeServerBaseAdapter : SqlCeServerAdapter, IBaseSqlAdapter
    {
        public string GetPagedSql(string tableName, string orderBy, string selectColumns, string whereClause, int pageIndex, int pageSize)
        {
            throw new System.NotImplementedException();
        }
    }
}