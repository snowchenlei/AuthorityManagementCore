using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Anc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Snow.AuthorityManagement.Web.Startup.OnceTask
{
    public class InitalPermissionTask : IStartupTask
    {
        private readonly IServiceProvider _services;
        private readonly IPermissionDefinitionContext Context;

        public InitalPermissionTask(IServiceProvider services
            , IPermissionDefinitionContext context)
        {
            _services = services;
            Context = context;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Task task = Task.Run(() =>
            {
                _services
                    .GetServices<IAuthorizationProvider>()
                    .ToList()
                    .ForEach(p => p.SetPermissions(Context));
            }, cancellationToken);
            return task;
        }
    }
}