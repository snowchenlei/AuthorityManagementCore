using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Web.Helpers;
using Snow.AuthorityManagement.Web.Manager;
using Snow.AuthorityManagement.Web.Startup;

namespace Snow.AuthorityManagement.Web.Views.Shared.Components.SideBarNav
{
    public class SideBarNavViewComponent : ViewComponent
    {
        private readonly IUserNavigationManager _userNavigationManager;

        public SideBarNavViewComponent(IUserNavigationManager userNavigationManager)
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