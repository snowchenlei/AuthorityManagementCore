using Anc.DependencyInjection;
using Anc.Reflection;
using Anc.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Anc.Authorization
{
    public class AuthorizationHelper : IAuthorizationHelper, ITransientDependency
    {
        private readonly IPermissionManagerBase _permissionManagerBase;
        private readonly IAncSession _ancSession;

        public AuthorizationHelper(IPermissionManagerBase permissionManagerBase, IAncSession ancSession)
        {
            _permissionManagerBase = permissionManagerBase;
            _ancSession = ancSession;
        }

        public async Task CheckPermissionsAsync(MethodInfo methodInfo, Type type)
        {
            if (AllowAnonymous(methodInfo, type))
            {
                return;
            }

            IAbpAuthorizeAttribute[] authorizeAttributes =
                ReflectionHelper
                    .GetAttributesOfMemberAndType(methodInfo, type)
                    .OfType<IAbpAuthorizeAttribute>()
                    .ToArray();
            if (!authorizeAttributes.Any())
            {
                return;
            }

            await AuthorizeAsync(authorizeAttributes);
        }

        public virtual async Task AuthorizeAsync(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes)
        {
            if (!_ancSession.UserId.HasValue)
            {
                throw new AncAuthorizationException("请登陆");
            }
            IEnumerable<IPermission> permissions = await _permissionManagerBase.GetAllPermissionsByUserIdAsync(_ancSession.UserId.Value);

            foreach (var authorizeAttribute in authorizeAttributes)
            {
                IsGranted(authorizeAttribute.RequireAllPermissions, permissions, authorizeAttribute.Permissions);
            }
        }

        public void IsGranted(bool requireAll, IEnumerable<IPermission> permissions, params string[] permissionNames)
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
                .OfType<IAncAllowAnonymousAttribute>()
                .Any();
        }
    }
}