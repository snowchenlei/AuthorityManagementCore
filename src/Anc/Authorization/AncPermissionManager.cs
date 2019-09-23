using Anc.Collections.Extensions;
using Anc.Dependency;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Authorization
{
    /// <summary>
    /// Permission manager.
    /// </summary>
    public class AncPermissionManager : PermissionDefinitionContextBase, IAncPermissionManager, ISingletonDependency
    {
        public AncPermission GetPermission(string name)
        {
            var permission = Permissions.GetOrDefault(name);
            if (permission == null)
            {
                throw new AncException("There is no permission with name: " + name);
            }

            return permission;
        }

        public IReadOnlyList<AncPermission> GetAllPermissions()
        {
            return Permissions.Values.ToImmutableList();
        }
    }
}