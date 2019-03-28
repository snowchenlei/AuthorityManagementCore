using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Web.Authorization;

namespace Snow.AuthorityManagement.Web.Startup.OnceTask
{
    public class InitalPermissionTask:IStartupTask
    {
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Task task = Task.Run(() => { AuthorizationProvider.SetPermissions(); }, cancellationToken);
            return task;
        }
    }
}
