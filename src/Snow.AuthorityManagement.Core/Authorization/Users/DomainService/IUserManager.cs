using Anc.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core.Authorization.Users.DomainService
{
    public interface IUserManager : IDomainService
    {
        int GetCount();

        Task<int> GetCountAsync();
    }
}