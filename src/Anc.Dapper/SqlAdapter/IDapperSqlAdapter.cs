using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Dapper.SqlAdapter
{
    public interface IBaseSqlAdapter : ISqlAdapter
    {
        /// <summary>
        /// 获取分页Sql
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">排序</param>
        /// <param name="selectColumns">查询列</param>
        /// <param name="whereClause">过滤条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>分页Sql</returns>
        string GetPagedSql(string tableName, string orderBy, string selectColumns, string whereClause, int pageIndex, int pageSize);
    }
}