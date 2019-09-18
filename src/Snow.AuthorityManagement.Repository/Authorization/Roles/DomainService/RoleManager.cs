using Anc.Domain.Repositories;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Authorization.Roles.DomainService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Repository.Authorization.Roles.DomainService
{
    public class RoleManager : AuthorityManagementDomainServiceBase, IRoleManager
    {
        private readonly ILambdaRepository<Role> _roleRepository;

        public RoleManager(ILambdaRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public int GetCount()
        {
            return _roleRepository.Count(a => true);
        }

        public Task<int> GetCountAsync()
        {
            return _roleRepository.CountAsync(a => true);
        }

        public DateTime? GetLastModificationTime()
        {
            return _roleRepository.Max(a => a.LastModificationTime);
        }

        public Task<DateTime?> GetLastModificationTimeAsync()
        {
            return _roleRepository.MaxAsync(a => a.LastModificationTime);
        }
    }
}