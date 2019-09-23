using Anc.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anc.Authorization
{
    public class PermissionDefinitionContextBase : IPermissionDefinitionContext
    {
        protected readonly PermissionDictionary Permissions;

        protected PermissionDefinitionContextBase()
        {
            Permissions = new PermissionDictionary();
        }

        public AncPermission CreatePermission(
            string name,
            string displayName = null,
            string description = null)
        {
            if (Permissions.ContainsKey(name))
            {
                throw new AncException("There is already a permission with name: " + name);
            }

            var permission = new AncPermission(name, displayName, description);
            Permissions[permission.Name] = permission;
            return permission;
        }

        public AncPermission GetPermissionOrNull(string name)
        {
            return Permissions.GetOrDefault(name);
        }

        public void RemovePermission(string name)
        {
            Permissions.Remove(name);
        }
    }
}