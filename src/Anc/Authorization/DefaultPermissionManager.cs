using Anc.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Authorization
{
    public class DefaultPermissionManager : IPermissionManagerBase, ISingletonDependency
    {
        public Task<IEnumerable<IPermission>> GetAllPermissionsAsync(int userId)
        {
            IEnumerable<IPermission> permissions = new List<IPermission>();
            return Task.FromResult(permissions);
        }
    }
}