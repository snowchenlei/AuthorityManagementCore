using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc;
using Anc.Application.Services.Dto;
using Anc.Domain.Repositories;
using Anc.Domain.Uow;
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
            int index = 0;
            if (!String.IsNullOrEmpty(input.LogLevel))
            {
                wheres.Add($"Level=@{index++}");
                parameters.Add(input.LogLevel);
            }

            if (!string.IsNullOrEmpty(input.Date))
            {
                DateTime[] date = Array.ConvertAll(input.Date
                    .Split(new[] { '~' }, StringSplitOptions.RemoveEmptyEntries)
                    , DateTime.Parse);
                wheres.Add($"CreationTime > (@{index++}) AND CreationTime < (@{index++})");
                parameters.Add(date[0]);
                parameters.Add(date[1]);
            }
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                input.Sorting = input.Sorting + (input.Order == OrderType.ASC ? " ASC" : " DESC");
            }
            var result = await _logRepository
                .GetPagedAsync(input.PageIndex, input.PageSize, string.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
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
        [UnitOfWork]
        public async Task<bool> DeleteLogAsync(long id)
        {
            await _logRepository.DeleteAsync(id);
            return true;
        }
    }
}