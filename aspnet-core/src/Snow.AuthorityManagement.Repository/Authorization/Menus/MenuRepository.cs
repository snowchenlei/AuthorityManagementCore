using Snow.AuthorityManagement.Core.Authorization.Menus;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Snow.AuthorityManagement.Repositories;
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