﻿using Snow.AuthorityManagement.Core.Authorization.Users;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Snow.AuthorityManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Repository.Authorization.Users
{
    public class UserRepository : AuthorityManagementRepositoryBase<User>, IUserRepository
    {
        public UserRepository(AuthorityManagementContext context) : base(context)
        {
        }

        public Task<User> GetUserByUserNameAsync(string userName)
        {
            return FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public Task<bool> IsExistsByUserNameAsync(string userName)
        {
            return ExistsAsync(a => a.UserName == userName);
        }
    }
}