using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Authorization.Permissions;
using Anc.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;

namespace Anc.Authorization.Permissions
{
    public class PermissionStore : IPermissionStore, ITransientDependency
    {
        protected IAncPermissionRepository PermissionRepository { get; }

        private readonly IDistributedCache _cache;

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await PermissionRepository.FindAsync(name, providerName, providerKey)) != null;
        }
    }
}