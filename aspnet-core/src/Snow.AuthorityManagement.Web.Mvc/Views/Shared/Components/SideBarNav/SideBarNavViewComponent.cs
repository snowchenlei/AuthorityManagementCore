using System.Threading.Tasks;
using Anc.Application.Navigation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Snow.AuthorityManagement.Web.Views.Shared.Components.SideBarNav
{
    public class SideBarNavViewComponent : ViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;

        public SideBarNavViewComponent(
             IUserNavigationManager userNavigationManager)
        {
            _userNavigationManager = userNavigationManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string activeMenu = "")
        {
            var model = new SideBarNavViewModel
            {
                MainMenu = await _userNavigationManager.GetMenuAsync(),
                ActiveMenuItemName = activeMenu
            };

            return View(model);
        }
    }
}