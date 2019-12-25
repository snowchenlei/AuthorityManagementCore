using System;
using System.Collections.Generic;
using System.Text;
using Anc.Application.Services.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Logs.Dto
{
    /// <summary>
    /// 获取列表参数
    /// </summary>
    public class GetLogsInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public GetLogsInput()
        {
            Sorting = "ID";
        }
    }
}