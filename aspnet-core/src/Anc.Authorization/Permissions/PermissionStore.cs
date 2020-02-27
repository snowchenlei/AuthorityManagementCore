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
        public PermissionStore(IAncPermissionRepository ancPermissionRepository)
        {
            _ancPermissionRepository = ancPermissionRepository;
        }

        // TODO:缓存
        private readonly IDistributedCache _cache;

        private readonly IAncPermissionRepository _ancPermissionRepository;

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await _ancPermissionRepository.FindAsync(name, providerName, providerKey)) != null;
        }

        public Task<List<AncPermission>> GetPermissionsByRoleNamesAsync(params string[] roleNames)
        {
            return _ancPermissionRepository.GetListAsync(AncConsts.PermissionRoleProviderName, roleNames);
        }
    }
}