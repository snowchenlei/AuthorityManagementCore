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

        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        private ICurrentPrincipalAccessor PrincipalAccessor { get; }

        public PermissionChecker(ICurrentPrincipalAccessor principalAccessor
            , IPermissionDefinitionManager permissionDefinitionManager
            , IServiceProvider serviceProvider
            )
        {
            PrincipalAccessor = principalAccessor;
            PermissionDefinitionManager = permissionDefinitionManager;
            _serviceProvider = serviceProvider;
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

            using (var scope = _serviceProvider.CreateScope())
            {
                foreach (var provider in scope.ServiceProvider.GetServices<IPermissionValueProvider>())
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