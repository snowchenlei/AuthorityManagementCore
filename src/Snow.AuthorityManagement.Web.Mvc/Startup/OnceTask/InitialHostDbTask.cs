using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.Data.Seed;

namespace Snow.AuthorityManagement.Web.Startup.OnceTask
{
    public class InitialHostDbTask : IStartupTask
    {
        private readonly AuthorityManagementContext _context;
        private readonly IConfiguration _configuration;

        public InitialHostDbTask(AuthorityManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await SeedHelper.SeedHostDbAsync(_context, _configuration);
        }
    }
}