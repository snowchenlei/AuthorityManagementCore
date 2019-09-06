using Anc.Authorization;
using Anc.Dependency;
using Anc.Domain.Repositories;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Permissions.DomainService;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Repository.Authorization.Permissions.DomainService
{
    public class PermissionManager : AuthorityManagementDomainServiceBase, IPermissionManager
    {
        private readonly ILambdaRepository<UserRole> _userRoleRepository;
        private readonly ILambdaRepository<Permission> _permissionRepository;

        public PermissionManager(ILambdaRepository<UserRole> userRoleRepository
            , ILambdaRepository<Permission> permissionRepository)
        {
            _userRoleRepository = userRoleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<IPermission>> GetAllPermissionsAsync(int userId)
        {
            var userRoles = await _userRoleRepository.GetAllListAsync(r => r.UserID == userId);
            var roleIds = userRoles.Select(ur => ur.RoleID);
            return await _permissionRepository
                .GetAllListAsync(p => p.User.ID == userId
                                    || roleIds.Contains(p.Role.ID));
        }
    }
}