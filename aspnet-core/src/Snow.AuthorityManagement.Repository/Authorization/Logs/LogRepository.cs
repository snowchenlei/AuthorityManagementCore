using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.Logs;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Snow.AuthorityManagement.Repositories;

namespace Snow.AuthorityManagement.Repository.Authorization.Logs
{
    public class LogRepository : AuthorityManagementRepositoryBase<Log, long>, ILogRepository
    {
        public LogRepository(AuthorityManagementContext context) : base(context)
        {
        }

        public async Task<Dictionary<string, int>> GetAllLevelAndCount()
        {
            return await GetAll()
                .GroupBy(l => l.Level)
                .Select(a => new
                {
                    Key = a.Key,
                    Count = a.Count()
                }).ToDictionaryAsync(key => key.Key, value => value.Count);
        }
    }
}