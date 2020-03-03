using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;

namespace Snow.AuthorityManagement.Application.Authorization.Permissions
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public class PermissionAppService : IPermissionAppService
    {
        
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public PermissionAppService(IPermissionRepository permissionRepository, IUserRoleRepository userRoleRepository)
        {
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<List<AncPermission>> GetUserPermissionsAsync(int userId)
        {
            List<string> roleNames = await GetRoleNamesByUserAsync(userId);
            List<AncPermission> permissions = new List<AncPermission>();
            foreach (string name in roleNames)
            {
                permissions.AddRange(await _permissionRepository.GetPermissionsByRoleNameAsync(name));
            }
            permissions.AddRange(await _permissionRepository.GetPermissionsByUserIdAsync(userId.ToString()));
            return permissions;
        }

        /// <summary>
        /// 获取用户角色名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<List<string>> GetRoleNamesByUserAsync(int userId)
        {
            List<string> userNames = await _userRoleRepository.GetRoleNamesByUserIdAsync(userId);
            return userNames;
        }
    }
}