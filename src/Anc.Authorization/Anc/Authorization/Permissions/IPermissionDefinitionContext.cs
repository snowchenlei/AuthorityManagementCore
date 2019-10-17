using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionDefinitionContext
    {
        //TODO: Add Get methods to find and modify a permission or group.
        PermissionGroupDefinition GetGroupOrNull(string name);

        PermissionGroupDefinition AddGroup(
            [NotNull] string name,
            string displayName = null);

        void RemoveGroup(string name);
    }
}