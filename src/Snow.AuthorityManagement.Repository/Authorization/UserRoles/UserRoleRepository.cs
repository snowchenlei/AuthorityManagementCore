using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Repository.Authorization.UserRoles
{
    public class UserRoleRepository : AuthorityManagementRepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AuthorityManagementContext context) : base(context)
        {
        }

        public Task<List<UserRole>> GetUserRolesByUserIdAsync(int userId)
        {
            return GetAllListAsync(a => a.UserID == userId);
        }
    }
}