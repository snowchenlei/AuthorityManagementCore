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
        public PermissionDefinition Permission { get; }

        public ClaimsPrincipal Principal { get; }

        public PermissionValueCheckContext(
            PermissionDefinition permission,
            ClaimsPrincipal principal)
        {
            Check.NotNull(permission, nameof(permission));

            Permission = permission;
            Principal = principal;
        }
    }
}