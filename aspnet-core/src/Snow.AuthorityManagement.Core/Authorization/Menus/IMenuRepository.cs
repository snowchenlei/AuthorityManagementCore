using Anc.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core.Authorization.Menus
{
    public interface IMenuRepository : ILambdaRepository<Menu>
    {
        Task<bool> IsExistsByNameAsync(string name);
    }
}