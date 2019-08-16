using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Data;

namespace Snow.AuthorityManagement.Repository.Authorization
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AuthorityManagementContext context) : base(context)
        {
        }

        public async Task<bool> SetPermissionsByRoleId(Role role, List<Permission> newPermissions
            , List<Permission> lostPermissions)
        {
            foreach (var entity in lostPermissions)
            {
                Delete(entity);
            }

            foreach (var newPermission in newPermissions)
            {
                newPermission.Role = role;
                await AddAsync(newPermission);
            }

            return true;
        }
    }
}