using System.Threading.Tasks;

namespace Anc.Application.Navigation
{
    public interface IUserNavigationManager
    {
        Task<UserMenu> GetMenuAsync();
    }
}