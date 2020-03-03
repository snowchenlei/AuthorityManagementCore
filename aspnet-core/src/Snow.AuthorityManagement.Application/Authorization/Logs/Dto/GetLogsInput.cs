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
        /// 日志等级
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// 创建时间段
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        public GetLogsInput()
        {
            Sorting = "ID";
        }
    }
}