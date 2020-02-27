using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Domain.Entities;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionStore
    {
        Task<bool> IsGrantedAsync(
            string name,
            string providerName,
            string providerKey
        );

        Task<List<AncPermission>> GetPermissionsByRoleNamesAsync(params string[] roleNames);
    }
}