using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Snow.AuthorityManagement.EntityFrameworkCore.Seed;

namespace Snow.AuthorityManagement.Web.Startup.OnceTask
{
    public class InitialHostDbTask : IStartupTask
    {
        private readonly AuthorityManagementContext _context;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public InitialHostDbTask(AuthorityManagementContext context, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _context = context;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await SeedHelper.SeedHostDbAsync(_context, _serviceProvider);
        }
    }
}