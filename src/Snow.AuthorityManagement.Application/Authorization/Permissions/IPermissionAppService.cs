using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services;
using Anc.Domain.Entities;
using Snow.AuthorityManagement.Core.Authorization.Permissions;

namespace Snow.AuthorityManagement.Application.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        Task<bool> IsGrantedAsync(string permissionName, int userId);

        Task<List<AncPermission>> GetAllPermissionsAsync(int userId);

        Task<List<AncPermission>> GetPermissionsByRoleIdAsync(int roleId);
    }
}