using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Snow.AuthorityManagement.Common;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core.Model;
using Snow.AuthorityManagement.IService.Authorization;

namespace Snow.AuthorityManagement.Web.Authorization
{
    public class AuthorizationHelper
    {
        private readonly IPermissionService _permissionService;

        public AuthorizationHelper(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task CheckPermissionsAsync(MethodInfo methodInfo, Type type)
        {
            if (AllowAnonymous(methodInfo, type))
            {
                return;
            }
            AuthorizationFactory auth = new AuthorizationFactory();
            LoginUserInfo user = auth.GetLoginUserInfo();
            if (user == null)
            {
                throw new AncAuthorizationException("请登陆");
            }
            RBACAuthorizeAttribute[] authorizeAttributes =
                ReflectionHelper
                    .GetAttributesOfMemberAndType(methodInfo, type)
                    .OfType<RBACAuthorizeAttribute>()
                    .ToArray();
            if (!authorizeAttributes.Any())
            {
                return;
            }

            await AuthorizeAsync(authorizeAttributes, user);
        }

        public virtual async Task AuthorizeAsync(IEnumerable<RBACAuthorizeAttribute> authorizeAttributes, LoginUserInfo user)
        {
            if ("admin".Equals(user.UserName, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            List<Permission> permissions = await _permissionService.GetAllPermissionsAsync(user.UserId);

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
                    throw new AncAuthenticationException("用户必须具有" + string.Join(", ", permissionNames) + "所有权限");
                }
            }
            else
            {
                if (!permissions.Any(p => permissionNames.Contains(p.Name)))
                {
                    throw new AncAuthenticationException("用户必须具有" + string.Join(", ", permissionNames) + "中任意一个");
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