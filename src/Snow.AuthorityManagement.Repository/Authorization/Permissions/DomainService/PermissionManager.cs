using Anc.Authorization;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Permissions.DomainService;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Repository.Authorization.Permissions.DomainService
{
    public class PermissionManager : AuthorityManagementDomainServiceBase, IPermissionManager
    {
        private readonly ILambdaRepository<UserRole> _userRoleRepository;
        private readonly ILambdaRepository<AncPermission, Guid> _permissionRepository;

        public PermissionManager(ILambdaRepository<UserRole> userRoleRepository
            , ILambdaRepository<AncPermission, Guid> permissionRepository)
        {
            _userRoleRepository = userRoleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<AncPermission>> GetAllPermissionsByRoleIdAsync(string roleName)
        {
            return await _permissionRepository
                .GetAll()
                .Where(p => p.ProviderName == "Role" && p.ProviderKey == roleName)
                .ToListAsync();
        }
    }
}