using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc;
using Anc.Application.Services.Dto;
using Anc.Domain.Repositories;
using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Logs.Dto;
using Snow.AuthorityManagement.Core.Authorization.AuditLogs;
using Snow.AuthorityManagement.Core.Authorization.Logs;

namespace Snow.AuthorityManagement.Application.Authorization.Logs
{
    /// <summary>
    /// 审计日志
    /// </summary>
    public class LogAppService : ILogAppService
    {
        private readonly IMapper _mapper;
        private readonly ILambdaRepository<Log, long> _logRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="logRepository"></param>
        public LogAppService(IMapper mapper
            , ILambdaRepository<Log, long> logRepository)
        {
            _mapper = mapper;
            _logRepository = logRepository;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        public async Task<PagedResultDto<LogListDto>> GetLogPagedAsync(GetLogsInput input)
        {
            List<string> wheres = new List<string>();
            List<object> parameters = new List<object>();
            var result = await _logRepository
                .GetPagedAsync(input.PageIndex, input.PageSize, "", parameters.ToArray(), input.Sorting);
            return new PagedResultDto<LogListDto>()
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                Items = _mapper.Map<List<LogListDto>>(result.Item1),
                TotalCount = result.Item2
            };
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public async Task<bool> DeleteLogAsync(long id)
        {
            // TODO:删除
            await _logRepository.DeleteAsync(id);
            return true;
        }
    }
}