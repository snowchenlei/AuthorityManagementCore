using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;

namespace Anc.Authorization.Permissions
{
    public interface IAncPermissionRepository : IRepository<AncPermission, Guid>
    {
        Task<AncPermission> FindAsync(string name, string providerName, string providerKey);

        Task<List<AncPermission>> GetListAsync(string providerName, string providerKey);
    }
}