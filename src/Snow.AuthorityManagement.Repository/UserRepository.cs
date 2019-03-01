using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Entities.Authorization.User;

namespace Snow.AuthorityManagement.Repository
{
    public partial class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AuthorityManagementContext context) : base(context)
        { }
    }
}