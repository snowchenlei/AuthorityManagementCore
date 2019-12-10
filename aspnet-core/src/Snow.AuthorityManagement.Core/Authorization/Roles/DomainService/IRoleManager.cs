using Anc.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core.Authorization.Roles.DomainService
{
    public interface IRoleManager : IDomainService
    {
        int GetCount();

        Task<int> GetCountAsync();

        DateTime? GetLastModificationTime();

        Task<DateTime?> GetLastModificationTimeAsync();
    }
}