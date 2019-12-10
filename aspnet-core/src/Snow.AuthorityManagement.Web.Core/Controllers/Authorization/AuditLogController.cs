using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    /// <summary>
    /// 日志
    /// </summary>
    [Route("api/auditlogs")]
    public class AuditLogController : PageController
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        public AuditLogController(IMapper mapper) : base(mapper)
        {
        }
    }
}