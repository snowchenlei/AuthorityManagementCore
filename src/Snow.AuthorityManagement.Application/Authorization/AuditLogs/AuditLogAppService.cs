using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services.Dto;
using Anc.Auditing;
using Anc.Domain.Repositories;
using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.AuditLogs.Dto;
using Snow.AuthorityManagement.Core.Authorization.AuditLogs;

namespace Snow.AuthorityManagement.Application.Authorization.AuditLogs
{
    /// <summary>
    /// 审计日志
    /// </summary>
    public class AuditLogAppService : IAuditLogAppService
    {
        private readonly IMapper _mapper;
        private readonly ILambdaRepository<AuditLog, long> _auditLogRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="auditLogRepository"></param>
        public AuditLogAppService(IMapper mapper
            , ILambdaRepository<AuditLog, long> auditLogRepository)
        {
            _mapper = mapper;
            _auditLogRepository = auditLogRepository;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogPagedAsync(GetAuditLogsInput input)
        {
            List<string> wheres = new List<string>();
            List<object> parameters = new List<object>();
            var result = await _auditLogRepository
                .GetPagedAsync(input.PageIndex, input.PageSize,
                    string.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
            return new PagedResultDto<AuditLogListDto>()
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                Items = _mapper.Map<List<AuditLogListDto>>(result.Item1),
                TotalCount = result.Item2
            };
        }
    }
}