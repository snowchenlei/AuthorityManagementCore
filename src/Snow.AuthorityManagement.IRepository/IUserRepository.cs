using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Entities.Authorization.User;

namespace Snow.AuthorityManagement.IRepository
{
    public partial interface IUserRepository : IBaseRepository<User>
    {
    }
}