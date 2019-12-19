using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Common
{
    public class PageHelper
    {
        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns>总页数</returns>
        public static int GetPageCount(int totalCount, int pageSize)
        {
            return (totalCount + pageSize - 1) / pageSize;
            //(int)Math.Ceiling((double)totalCount / pageSize);
        }

        public static void GetPageIndex(ref int pageIndex)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
        }

        public static void GetPageSize(ref int pageSize)
        {
            pageSize = pageSize < 1 ? 10 : pageSize;
        }
    }
}
