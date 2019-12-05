using Anc.Authorization;
using Anc.Domain.Entities;
using Anc.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core.Authorization.Permissions.DomainService
{
    public interface IPermissionManager : IDomainService
    {
        Task<IEnumerable<AncPermission>> GetAllPermissionsByRoleIdAsync(string roleName);
    }
}