using Anc.Domain.Repositories;
using Snow.AuthorityManagement.Core.Authorization.Users.DomainService;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Repository.Authorization.Users.DomainService
{
    public class UserManager : AuthorityManagementDomainServiceBase, IUserManager
    {
        private readonly ILambdaRepository<User> _userRepository;

        public UserManager(ILambdaRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public int GetCount()
        {
            return _userRepository.Count(a => true);
        }

        public Task<int> GetCountAsync()
        {
            return _userRepository.CountAsync(a => true);
        }
    }
}