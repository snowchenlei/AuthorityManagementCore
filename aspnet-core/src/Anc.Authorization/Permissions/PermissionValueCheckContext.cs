using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using JetBrains.Annotations;

namespace Anc.Authorization.Permissions
{
    /// <summary>
    /// 封装,也可以不需要
    /// </summary>
    public class PermissionValueCheckContext
    {
        [NotNull]
        public PermissionDefinition Permission { get; }

        [CanBeNull]
        public ClaimsPrincipal Principal { get; }

        public PermissionValueCheckContext(
            [NotNull] PermissionDefinition permission,
            [CanBeNull] ClaimsPrincipal principal)
        {
            Check.NotNull(permission, nameof(permission));

            Permission = permission;
            Principal = principal;
        }
    }
}