using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionDefinitionContext
    {
        PermissionDefinition GetPermissionOrNull(string name);

        /// <summary>
        /// Creates a new permission under this group.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        /// <param name="displayName">Display name of the permission</param>
        /// <returns>New created permission</returns>
        PermissionDefinition CreatePermission(
            string name,
            string displayName = null);

        void RemovePermission(string name);
    }
}