using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionDefinitionManager
    {
        PermissionDefinition Get(string name);

        PermissionDefinition GetOrNull(string name);

        IReadOnlyList<PermissionDefinition> GetPermissions();
    }
}