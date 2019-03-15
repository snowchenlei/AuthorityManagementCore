using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Snow.AuthorityManagement.Common;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Core.Dto.User;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core.Model;
using Snow.AuthorityManagement.IService.Authorization;
using Snow.AuthorityManagement.Web.Session;

namespace Snow.AuthorityManagement.Web.Authorization
{
    public class AuthorizationHelper : IAuthorizationHelper
    {
        private readonly IPermissionService _permissionService;
        private readonly IAncSession _ancSession;

        public AuthorizationHelper(IPermissionService permissionService, IAncSession ancSession)
        {
            _permissionService = permissionService;
            _ancSession = ancSession;
        }

        public async Task CheckPermissionsAsync(MethodInfo methodInfo, Type type)
        {
            if (AllowAnonymous(methodInfo, type))
            {
                return;
            }

            AncAuthorizeAttribute[] authorizeAttributes =
                ReflectionHelper
                    .GetAttributesOfMemberAndType(methodInfo, type)
                    .OfType<AncAuthorizeAttribute>()
                    .ToArray();
            if (!authorizeAttributes.Any())
            {
                return;
            }

            await AuthorizeAsync(authorizeAttributes);
        }

        public virtual async Task AuthorizeAsync(IEnumerable<AncAuthorizeAttribute> authorizeAttributes)
        {
            if (!_ancSession.UserId.HasValue)
            {
                throw new AncAuthorizationException("请登陆");
            }
            List<Permission> permissions = await _permissionService.GetAllPermissionsAsync(_ancSession.UserId.Value);

            foreach (var authorizeAttribute in authorizeAttributes)
            {
                IsGranted(authorizeAttribute.RequireAllPermissions, permissions, authorizeAttribute.Permissions);
            }
        }

        public void IsGranted(bool requireAll, List<Permission> permissions, params string[] permissionNames)
        {
            if (!permissionNames.Any())
            {
                return;
            }
            if (requireAll)
            {
                if (!permissions.All(p => permissionNames.Contains(p.Name)))
                {
                    throw new AncAuthorizationException("用户必须具有" + string.Join(", ", permissionNames) + "所有权限");
                }
            }
            else
            {
                if (!permissions.Any(p => permissionNames.Contains(p.Name)))
                {
                    throw new AncAuthorizationException("用户必须具有" + string.Join(", ", permissionNames) + "中任意一个");
                }
            }
        }

        /// <summary>
        /// 判断是否被AllowAnonymousAttribute修饰
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool AllowAnonymous(MemberInfo memberInfo, Type type)
        {
            return ReflectionHelper
                .GetAttributesOfMemberAndType(memberInfo, type)
                .OfType<AllowAnonymousAttribute>()
                .Any();
        }
    }
}