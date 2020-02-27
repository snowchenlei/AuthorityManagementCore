using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Anc.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement([NotNull]string permissionName)
        {
            Check.NotNull(permissionName, nameof(permissionName));

            PermissionName = permissionName;
        }
    }
}