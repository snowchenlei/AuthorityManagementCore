using Anc.Authorization;
using Anc.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Permissions.DomainService;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Permission>> GetAllPermissionsByRoleIdAsync(int roleId)
        {
            return await _permissionRepository
                .GetAll()
                .Where(p => p.Role.ID == roleId)
                .ToListAsync();
        }

        public async Task<IEnumerable<IPermission>> GetAllPermissionsByUserIdAsync(int userId)
        {
            var roleIds = await _userRoleRepository
                .GetAll()
                .Where(r => r.UserID == userId)
                .Select(ur => ur.RoleID)
                .ToListAsync();
            return await _permissionRepository
                .GetAll()
                .Where(p => p.User.ID == userId
                                    || roleIds.Contains(p.Role.ID))
                .ToListAsync();
        }
    }
}