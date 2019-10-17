using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionDefinitionManager
    {
        [NotNull]
        PermissionDefinition Get([NotNull] string name);

        [CanBeNull]
        PermissionDefinition GetOrNull([NotNull] string name);

        IReadOnlyList<PermissionDefinition> GetPermissions();

        IReadOnlyList<PermissionGroupDefinition> GetGroups();
    }
}