using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Common.Extensions;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core.Model;

namespace Snow.AuthorityManagement.Web.Authorization
{
    public class PermissionDefinitionContextBase
    {
        public readonly PermissionDictionary Permissions;
        public static readonly PermissionDefinitionContextBase Context = new PermissionDefinitionContextBase();

        private PermissionDefinitionContextBase()
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
                throw new UserFriendlyException("There is already a permission with name: " + name);
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