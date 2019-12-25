using System;
using System.Collections.Generic;
using System.Text;
using Anc.Application.Services.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Logs.Dto
{
    /// <summary>
    /// 输出列表
    /// </summary>
    public class LogListDto : EntityDto<long>
    {
        public string Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}