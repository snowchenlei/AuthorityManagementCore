using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Anc.DependencyInjection;
using Anc.Security.Claims;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Anc.Authorization.Permissions
{
    public class PermissionChecker : IPermissionChecker, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IPermissionValueProvider> _permissionValueProviders;

        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected IPermissionValueProviderManager PermissionValueProviderManager { get; }
        private ICurrentPrincipalAccessor PrincipalAccessor { get; }

        public PermissionChecker(ICurrentPrincipalAccessor principalAccessor
            , IPermissionDefinitionManager permissionDefinitionManager
            , IPermissionValueProviderManager permissionValueProviderManager
            , IServiceProvider serviceProvider
            , IEnumerable<IPermissionValueProvider> permissionValueProviders
            )
        {
            PrincipalAccessor = principalAccessor;
            PermissionDefinitionManager = permissionDefinitionManager;
            PermissionValueProviderManager = permissionValueProviderManager;
            this._serviceProvider = serviceProvider;
            _permissionValueProviders = permissionValueProviders;
        }

        public Task<bool> IsGrantedAsync([NotNull] string name)
        {
            return IsGrantedAsync(PrincipalAccessor.Principal, name);
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="claimsPrincipal">Claims</param>
        /// <param name="name">权限名称</param>
        /// <returns>是否有权</returns>
        public virtual async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            Check.NotNull(name, nameof(name));

            PermissionDefinition permission = PermissionDefinitionManager.Get(name);

            var isGranted = false;
            var context = new PermissionValueCheckContext(permission, claimsPrincipal);

            // TODO:单例类解析服务会导致EfCore线程问题。https://docs.microsoft.com/zh-cn/ef/core/miscellaneous/configuring-dbcontext#avoiding-dbcontext-threading-issues
            // 为什么使用直接注入的_permissionValueProviders也是不行的 foreach (var provider in PermissionValueProviderManager.ValueProviders)
            using (var scope = _serviceProvider.CreateScope())
            {
                foreach (var provider in scope.ServiceProvider.GetServices<IPermissionValueProvider>())
                //foreach (var provider in _permissionValueProviders)
                {
                    if (context.Permission.Providers.Any() &&
                        !context.Permission.Providers.Contains(provider.Name))
                    {
                        continue;
                    }
                    var result = await provider.CheckAsync(context);

                    if (result == PermissionGrantResult.Granted)
                    {
                        isGranted = true;
                    }
                    else if (result == PermissionGrantResult.Prohibited)
                    {
                        return false;
                    }
                }
            }

            return isGranted;
        }
    }
}