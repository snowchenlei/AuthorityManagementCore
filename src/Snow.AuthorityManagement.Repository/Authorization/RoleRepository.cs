using System;
using System.Collections.Generic;
using System.Text;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.IRepository.Authorization;

namespace Snow.AuthorityManagement.Repository.Authorization
{
    public partial class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AuthorityManagementContext context) : base(context)
        { }
    }
}