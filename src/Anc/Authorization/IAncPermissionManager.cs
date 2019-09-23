using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Authorization
{
    public interface IAncPermissionManager
    {
        /// <summary>
        /// Gets <see cref="AncPermission"/> object with given <paramref name="name"/> or throws
        /// exception if there is no permission with given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        AncPermission GetPermission(string name);

        /// <summary>
        /// Gets <see cref="AncPermission"/> object with given <paramref name="name"/> or returns
        /// null if there is no permission with given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        AncPermission GetPermissionOrNull(string name);

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <param name="tenancyFilter">Can be passed false to disable tenancy filter.</param>
        IReadOnlyList<AncPermission> GetAllPermissions();
    }
}