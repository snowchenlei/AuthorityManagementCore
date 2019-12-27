using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Logs;
using Snow.AuthorityManagement.Application.Authorization.Logs.Dto;
using Snow.AuthorityManagement.Core;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    /// <summary>
    /// 日志
    /// </summary>
    [Route("api/logs")]
    public class LogController : PageController
    {
        private readonly ILogAppService _logService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        public LogController(IMapper mapper,
            ILogAppService logService) : base(mapper)
        {
            this._logService = logService;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <remarks>GET: api/logs</remarks>
        /// <param name="input">分页参数</param>
        /// <response code="200">获取成功</response>
        /// <returns></returns>
        [HttpGet(Name = "GetLogsPage")]
        [Authorize(PermissionNames.Pages_Administration_Menus_Query)]
        [ProducesResponseType(typeof(PagedResultDto<LogListDto>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery]GetLogsInput input)
        {
            var result = await _logService.GetLogPagedAsync(input);
            return Return<GetLogsInput, PagedResultDto<LogListDto>, LogListDto>(input, "GetLogsPage", result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <remarks>DELETE: api/logs/5</remarks>
        /// <param name="id">主键</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">没有找到</response>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            // 这个功能先不启用
            return NotFound();
            await _logService.DeleteLogAsync(id);
            return NoContent();
        }
    }
}