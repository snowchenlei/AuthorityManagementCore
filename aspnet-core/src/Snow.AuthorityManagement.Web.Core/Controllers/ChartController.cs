using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Logs;

namespace Snow.AuthorityManagement.Web.Core.Controllers
{
    /// <summary>
    /// 图表控制器
    /// </summary>
    [Route("api/charts")]
    public class ChartController : ApiController
    {
        private readonly ILogAppService _logAppService;

        public ChartController(ILogAppService logAppService)
        {
            this._logAppService = logAppService;
        }

        /// <summary>
        /// 日志统计饼状图
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLogPie")]
        public async Task<object> GetLogInfoAsync()
        {
            Dictionary<string, int> result = await _logAppService.GetCountPieAsync();
            return new
            {
                labels = result.Keys,
                dates = result.Values
            };
        }
    }
}