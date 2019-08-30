using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Domain.Repositories;
using Snow.AuthorityManagement.Core.Authorization.Roles;

namespace Snow.AuthorityManagement.Core.Authorization.Permissions
{
    public interface IPermissionRepository : ILambdaRepository<Permission>
    {
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="newPermissions">新权限</param>
        /// <param name="lostPermissions">旧权限</param>
        /// <returns></returns>
        Task<bool> SetPermissionsByRoleId(Role role, List<Permission> newPermissions
            , List<Permission> lostPermissions);

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId);
    }
}