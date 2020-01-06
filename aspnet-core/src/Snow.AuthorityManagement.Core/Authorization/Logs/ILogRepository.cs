using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Domain.Repositories;

namespace Snow.AuthorityManagement.Core.Authorization.Logs
{
    public interface ILogRepository : ILambdaRepository<Log, long>
    {
        Task<Dictionary<string, int>> GetAllLevelAndCount();
    }
}