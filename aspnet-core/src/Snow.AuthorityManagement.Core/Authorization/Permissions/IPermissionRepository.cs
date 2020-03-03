using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Authorization.Permissions;
using Anc.Domain.Entities;
using Snow.AuthorityManagement.Core.Authorization.Roles;

namespace Snow.AuthorityManagement.Core.Authorization.Permissions
{
    public interface IPermissionRepository : IAncPermissionRepository
    {
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        Task<List<AncPermission>> GetPermissionsByRoleNameAsync(string roleName);


        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task<List<AncPermission>> GetPermissionsByUserIdAsync(string userId);

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="permissions">权限</param>
        /// <returns></returns>
        Task<bool> SetPermissionsByRoleNameAsync(string roleName, IEnumerable<AncPermission> permissions);


    }
}