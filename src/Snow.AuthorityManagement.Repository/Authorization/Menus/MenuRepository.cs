using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.Menus;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Repository.Authorization.Menus
{
    public class MenuRepository : AuthorityManagementRepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(AuthorityManagementContext context) : base(context)
        {
        }

        public Task<bool> IsExistsByNameAsync(string name)
        {
            return ExistsAsync(a => a.Name == name);
        }
    }
}