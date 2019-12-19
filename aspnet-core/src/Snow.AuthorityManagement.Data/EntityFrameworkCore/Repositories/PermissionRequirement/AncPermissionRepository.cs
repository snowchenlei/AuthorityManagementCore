using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Anc.Authorization.Permissions;
using Anc.DependencyInjection;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Anc.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.EntityFrameworkCore;

namespace Snow.AuthorityManagement.Repositories.PermissionRequirement
{
    public class AncPermissionRepository : EfCoreRepositoryBase<AncPermission, Guid>, IAncPermissionRepository, ITransientDependency
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

        public async Task<List<AncPermission>> GetListAsync(string providerName, params string[] providerKeys)
        {
            Expression<Func<AncPermission, bool>> whereLambda;
            if (!providerKeys.Any())
            {
                throw new ArgumentException($"{nameof(providerKeys)} is Empty");
            }
            if (providerKeys.Length == 1)
            {
                whereLambda = s =>
                  s.ProviderName == providerName &&
                   s.ProviderKey == providerKeys[0];
            }
            else
            {
                whereLambda = s =>
                 providerKeys.Contains(s.ProviderKey) &&
                  s.ProviderName == providerName;
            }
            return await Table
               .Where(whereLambda).ToListAsync();
        }
    }
}