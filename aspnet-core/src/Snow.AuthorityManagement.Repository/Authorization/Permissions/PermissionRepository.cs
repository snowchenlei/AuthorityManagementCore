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

        public Task<List<AncPermission>> GetPermissionsByRoleNameAsync(string roleName)
        {
            return GetListAsync(AncConsts.PermissionRoleProviderName, roleName);
        }
    }
}