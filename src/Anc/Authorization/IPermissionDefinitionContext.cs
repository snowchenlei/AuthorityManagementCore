using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Authorization
{
    /// <summary>
    /// This context is used on <see cref="AuthorizationProvider.SetPermissions"/> method.
    /// </summary>
    public interface IPermissionDefinitionContext
    {
        /// <summary>
        /// Creates a new permission under this group.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        /// <param name="displayName">Display name of the permission</param>
        /// <param name="description">A brief description for this permission</param>
        /// <returns>New created permission</returns>
        AncPermission CreatePermission(
            string name,
            string displayName = null,
            string description = null);

        /// <summary>
        /// Gets a permission with given name or null if can not find.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        /// <returns>Permission object or null</returns>
        AncPermission GetPermissionOrNull(string name);

        /// <summary>
        /// Remove permission with given name
        /// </summary>
        /// <param name="name"></param>
        void RemovePermission(string name);
    }
}