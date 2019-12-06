using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Authorization.Permissions;
using Anc.DependencyInjection;
using Anc.Domain.Entities;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;

namespace Anc.Authorization.Permissions
{
    public class PermissionStore : IPermissionStore, ITransientDependency
    {
        protected IAncPermissionRepository PermissionRepository { get; }

        // TODO:缓存
        private readonly IDistributedCache _cache;

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await PermissionRepository.FindAsync(name, providerName, providerKey)) != null;
        }

        public Task<List<AncPermission>> GetAllPermissionsByUserIdAsync(string userName)
        {
            return PermissionRepository.GetListAsync(AncContext.PermissionUserProviderName, userName);
        }
    }
}