using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.IRepository;
using Snow.AuthorityManagement.IService.Authorization;

namespace Snow.AuthorityManagement.Service.Authorization
{
    public class PermissionService : IPermissionService
    {
        private readonly IBaseRepository<Permission> _permissionRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;

        public PermissionService(IBaseRepository<Permission> permissionRepository, IBaseRepository<UserRole> userRoleRepository)
        {
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync(int userId)
        {
            var userRoles = await _userRoleRepository.LoadListAsync(r => r.UserID == userId);
            var roleIds = userRoles.Select(ur => ur.RoleID);

            return await _permissionRepository
                .LoadListAsync(p => p.User.ID == userId
                                    || roleIds.Contains(p.Role.ID));
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _permissionRepository.LoadListAsync(p => p.Role.ID == roleId);
        }
    }
}