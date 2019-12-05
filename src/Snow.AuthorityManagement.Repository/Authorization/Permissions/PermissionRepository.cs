using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.Repositories;
using Snow.AuthorityManagement.Repositories.PermissionRequirement;

namespace Snow.AuthorityManagement.Repository.Authorization.Permissions
{
    public class PermissionRepository : AncPermissionRepository, IPermissionRepository
    {
        public PermissionRepository(AuthorityManagementContext context) : base(context)
        {
        }

        public async Task<bool> SetPermissionsByRoleId(Role role, List<AncPermission> newPermissions
            , List<AncPermission> lostPermissions)
        {
            foreach (var entity in lostPermissions)
            {
                Delete(entity);
            }

            foreach (var newPermission in newPermissions)
            {
                await InsertAsync(newPermission);
            }

            return true;
        }

        public Task<List<AncPermission>> GetPermissionsByRoleIdAsync(string roleName)
        {
            return GetAll().Where(a => a.ProviderName == "Role" && a.ProviderKey == roleName).ToListAsync();
        }
    }
}