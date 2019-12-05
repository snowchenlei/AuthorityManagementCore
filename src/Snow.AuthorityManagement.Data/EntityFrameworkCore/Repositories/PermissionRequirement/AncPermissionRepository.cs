using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anc.Authorization.Permissions;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Anc.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Data;

namespace Snow.AuthorityManagement.Repositories.PermissionRequirement
{
    public class AncPermissionRepository : EfCoreRepositoryBase<AncPermission, Guid>, IAncPermissionRepository
    {
        /// <summary>
        /// 可以放底层，尚未解决Context注入问题
        /// </summary>
        /// <param name="context"></param>
        public AncPermissionRepository(AuthorityManagementContext context)
           : base(context)
        {
        }

        public async Task<AncPermission> FindAsync(string name, string providerName, string providerKey)
        {
            return await Table
               .FirstOrDefaultAsync(s =>
                   s.Name == name &&
                   s.ProviderName == providerName &&
                   s.ProviderKey == providerKey
               );
        }

        public async Task<List<AncPermission>> GetListAsync(string providerName, string providerKey)
        {
            return await Table
               .Where(s =>
                   s.ProviderName == providerName &&
                   s.ProviderKey == providerKey
               ).ToListAsync();
        }
    }
}