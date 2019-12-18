using System;
using System.Collections.Generic;
using System.Text;
using Anc.Authorization.Permissions;

namespace Anc.Authorization.Anc.Authorization.Permissions
{
    public class PermissionDefinitionContext : IPermissionDefinitionContext
    {
        internal Dictionary<string, PermissionDefinition> PermissionDefinitions { get; }

        internal PermissionDefinitionContext()
        {
            PermissionDefinitions = new Dictionary<string, PermissionDefinition>();
        }

        public PermissionDefinition GetPermissionOrNull(string name)
        {
            return PermissionDefinitions.GetOrDefault(name);
        }

        public PermissionDefinition CreatePermission(string name, string displayName = null)
        {
            if (PermissionDefinitions.ContainsKey(name))
            {
                throw new AncException("There is already a permission with name: " + name);
            }

            var permission = new PermissionDefinition(name, displayName);
            PermissionDefinitions[permission.Name] = permission;
            return permission;
        }

        public virtual void RemovePermission(string name)
        {
            PermissionDefinitions.Remove(name);
        }
    }
}