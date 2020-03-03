using Anc.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core.Authorization.UserRoles
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<List<UserRole>> GetUserRolesByUserIdAsync(int userId);

        Task<List<string>> GetRoleNamesByUserIdAsync(int userId);
    }
}