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
        /// 设置角色权限
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="permissions">权限</param>
        /// <returns></returns>
        Task<bool> SetPermissionsByRoleNameAsync(string roleName, IEnumerable<AncPermission> permissions);

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        Task<List<AncPermission>> GetPermissionsByRoleNameAsync(string roleName);
    }
}