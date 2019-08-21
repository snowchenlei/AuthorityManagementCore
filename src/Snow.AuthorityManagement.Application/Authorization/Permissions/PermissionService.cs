using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Anc.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Core.Exception;

namespace Snow.AuthorityManagement.Application.Authorization.Permissions
{
    public class PermissionService : IPermissionService
    {
        private readonly ILambdaRepository<Permission> _permissionRepository;
        private readonly ILambdaRepository<UserRole> _userRoleRepository;

        public PermissionService(ILambdaRepository<Permission> permissionRepository, ILambdaRepository<UserRole> userRoleRepository)
        {
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<bool> IsGrantedAsync(string permissionName, int userId)
        {
            var userRoles = await _userRoleRepository.GetAllListAsync(r => r.UserID == userId);
            var roleIds = userRoles.Select(ur => ur.RoleID);

            return await _permissionRepository
                .IsExistsAsync(p => (p.User.ID == userId
                                    || roleIds.Contains(p.Role.ID)) && p.Name == permissionName);
        }

        public async Task<List<Permission>> GetAllPermissionsAsync(int userId)
        {
            var userRoles = await _userRoleRepository.GetAllListAsync(r => r.UserID == userId);
            var roleIds = userRoles.Select(ur => ur.RoleID);

            return await _permissionRepository
                .GetAllListAsync(p => p.User.ID == userId
                                    || roleIds.Contains(p.Role.ID));
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _permissionRepository.GetAllListAsync(p => p.Role.ID == roleId);
        }
    }
}