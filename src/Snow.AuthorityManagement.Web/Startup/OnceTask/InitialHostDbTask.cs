using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.Data.Seed;

namespace Snow.AuthorityManagement.Web.Startup.OnceTask
{
    public class InitialHostDbTask : IStartupTask
    {
        private readonly AuthorityManagementContext _context;

        public InitialHostDbTask(AuthorityManagementContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await SeedHelper.SeedHostDbAsync(_context);
        }
    }
}
