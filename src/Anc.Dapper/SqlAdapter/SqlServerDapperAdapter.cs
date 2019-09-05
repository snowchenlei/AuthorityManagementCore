using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Dapper.SqlAdapter
{
    public class SqlServerBaseAdapter : SqlServerAdapter, IBaseSqlAdapter
    {
        public string GetPagedSql(string tableName, string orderBy, string selectColumns, string whereClause, int pageIndex, int pageSize)
        {
            return $@"SELECT * FROM (
                        SELECT ROW_NUMBER() OVER(ORDER BY {orderBy}) AS PagedNumber, {selectColumns} FROM {tableName}
                        {whereClause}) AS u
                    WHERE PagedNumber BETWEEN (({pageIndex}) * {pageSize} + 1) AND ({pageIndex} + 1 * {pageSize})";
        }
    }
}