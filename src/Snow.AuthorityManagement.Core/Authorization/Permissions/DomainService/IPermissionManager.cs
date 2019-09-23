using Anc.Authorization;
using Anc.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core.Authorization.Permissions.DomainService
{
    public interface IPermissionManager : IDomainService, IPermissionManagerBase
    {
        Task<IEnumerable<Permission>> GetAllPermissionsByRoleIdAsync(int roleId);
    }
}