using System.Collections.Generic;
using System.Threading.Tasks;
using Anc;
using Anc.DependencyInjection;
using Anc.Domain.Entities;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Snow.AuthorityManagement.Repositories.PermissionRequirement;

namespace Snow.AuthorityManagement.Repository.Authorization.Permissions
{
    public class PermissionRepository : AncPermissionRepository, IPermissionRepository, ITransientDependency
    {
        public PermissionRepository(AuthorityManagementContext context) : base(context)
        {
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="permissions">权限</param>
        /// <returns></returns>
        public async Task<bool> SetPermissionsByRoleNameAsync(string roleName, IEnumerable<AncPermission> permissions)
        {
            List<AncPermission> oldPermissions = await GetListAsync(AncConsts.PermissionRoleProviderName, roleName);
            await DeleteAsync(oldPermissions);
            await InsertAsync(permissions);
            return true;
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public Task<List<AncPermission>> GetPermissionsByRoleNameAsync(string roleName)
        {
            return GetListAsync(AncConsts.PermissionRoleProviderName, roleName);
        }
    }
}