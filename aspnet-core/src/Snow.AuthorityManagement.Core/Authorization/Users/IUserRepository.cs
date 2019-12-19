using Anc.Domain.Repositories;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core.Authorization.Users
{
    public interface IUserRepository : ILambdaRepository<User>
    {
        Task<bool> IsExistsByUserNameAsync(string userName);

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户</returns>
        Task<User> GetUserByUserNameAsync(string userName);
    }
}