using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Authorization.Permissions;

namespace Snow.AuthorityManagement.Application.Authorization.Permissions
{
    public interface IPermissionService
    {
        Task<bool> IsGrantedAsync(string permissionName, int userId);

        Task<List<Permission>> GetAllPermissionsAsync(int userId);

        Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId);
    }
}