using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Model.Navigation;

namespace Snow.AuthorityManagement.Web.Manager
{
    public interface IUserNavigationManager
    {
        Task<UserMenu> GetMenuAsync();
    }
}