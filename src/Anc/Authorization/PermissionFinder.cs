using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Anc.Authorization
{
    /// <summary>
    /// This class is used to get permissions out of the system. Normally, you should inject and use
    /// <see cref="IPermissionManagerBase"/> and use it. This class can be used in database
    /// migrations or in unit tests where Abp is not initialized.
    /// </summary>
    public static class PermissionFinder
    {
        /// <summary>
        /// Collects and gets all permissions in given providers. This method can be used to get
        /// permissions in database migrations or in unit tests where Abp is not initialized.
        /// Otherwise, use <see cref="IPermissionManagerBase.GetAllPermissions(bool)"/> method.
        /// </summary>
        /// <param name="authorizationProviders">Authorization providers</param>
        /// <returns>List of permissions</returns>
        /// <remarks>
        /// This method creates instances of <see cref="authorizationProviders"/> by order and calls
        /// <see cref="AuthorizationProvider.SetPermissions"/> to build permission list. So,
        /// providers should not use dependency injection.
        /// </remarks>
        public static IReadOnlyList<AncPermission> GetAllPermissions(params IAuthorizationProvider[] authorizationProviders)
        {
            return new InternalPermissionFinder(authorizationProviders).GetAllPermissions();
        }

        internal class InternalPermissionFinder : PermissionDefinitionContextBase
        {
            public InternalPermissionFinder(params IAuthorizationProvider[] authorizationProviders)
            {
                foreach (var provider in authorizationProviders)
                {
                    provider.SetPermissions(this);
                }
                Permissions.AddAllPermissions();
            }

            public IReadOnlyList<AncPermission> GetAllPermissions()
            {
                return Permissions.Values.ToImmutableList();
            }
        }
    }
}