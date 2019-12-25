using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services;
using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Authorization.Logs.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Logs
{
    /// <summary>
    /// 审计日志
    /// </summary>
    public interface ILogAppService : IApplicationService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        Task<PagedResultDto<LogListDto>> GetLogPagedAsync(GetLogsInput input);

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        Task<bool> DeleteLogAsync(long id);
    }
}