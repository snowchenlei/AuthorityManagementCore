using System.Threading.Tasks;
using Anc.Application.Navigation;
using Snow.AuthorityManagement.Core.Model.Navigation;

namespace Snow.AuthorityManagement.Web.Manager
{
    public interface IUserNavigationManager
    {
        Task<UserMenu> GetMenuAsync();
    }
}