using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Anc.Authorization
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/aspnet/core/security/authorization/policies?view=aspnetcore-3.0#authorization-handlers
    /// </summary>
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionChecker _permissionChecker;

        public PermissionRequirementHandler(
            IPermissionChecker permissionChecker
            )
        {
            _permissionChecker = permissionChecker;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // 进行权限检查
            if (await _permissionChecker.IsGrantedAsync(context.User, requirement.PermissionName))
            {
                context.Succeed(requirement);
            }
        }
    }
}