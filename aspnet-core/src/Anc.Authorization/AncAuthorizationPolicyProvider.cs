using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anc.Authorization;
using Anc.Authorization.Permissions;
using Anc.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Anc.Authorization
{
    /// <summary>
    /// 授权Policy
    /// </summary>
    public class AncAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider, IAncAuthorizationPolicyProvider, ITransientDependency
    {
        private readonly AuthorizationOptions _options;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        public AncAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            IPermissionDefinitionManager permissionDefinitionManager)
            : base(options)
        {
            _permissionDefinitionManager = permissionDefinitionManager;
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
            {
                return policy;
            }

            var permission = _permissionDefinitionManager.GetOrNull(policyName);
            if (permission != null)
            {
                //TODO: Optimize & Cache!
                var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
                policyBuilder.Requirements.Add(new PermissionRequirement(policyName));
                return policyBuilder.Build();
            }

            return null;
        }

        public Task<List<string>> GetPoliciesNamesAsync()
        {
            return Task.FromResult(
                _options.GetPoliciesNames()
                    .Union(
                        _permissionDefinitionManager
                            .GetPermissions()
                            .Select(p => p.Name)
                    )
                    .ToList()
            );
        }
    }
}