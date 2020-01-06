using Anc.Application.Navigation;
using Microsoft.Extensions.Caching.Distributed;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Menus;
using Snow.AuthorityManagement.Core.Authorization.Menus.DomainService;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Startup
{
    public class NavigationProvider : INavigationProvider
    {
        private readonly IMenuManager _menuManager;
        private readonly IDistributedCache _cache;

        public NavigationProvider(IMenuManager menuManager
            , IDistributedCache cache)
        {
            _menuManager = menuManager;
            this._cache = cache;
        }

        public async Task<MenuDefinition> GetNavigationAsync()
        {
            MenuDefinition menus = await _cache.GetOrCreateAsync(AuthorityManagementConsts.MenuTreeCache,
                async () => await _menuManager.CreateMenuDefinitionAsync());
            //await _menuManager.CreateMenuDefinitionAsync();
            //return menus;
            //MenuDefinition menu = new MenuDefinition("MainMenu", "主菜单")
            //   .AddItem(new MenuItemDefinition(
            //           PageNames.Users,
            //           "系统管理",
            //           "user"
            //       ).AddItem(
            //           new MenuItemDefinition(
            //               PageNames.Users,
            //               "用户管理",
            //               "user",
            //               "/User/Index",
            //               target: "tab_1",
            //               requiredPermissionName: PermissionNames.Pages_Administration_Users
            //           )
            //       ).AddItem(
            //           new MenuItemDefinition(
            //               PageNames.Roles,
            //               "角色管理",
            //               "user",
            //               "/Role/Index",
            //               target: "tab_2",
            //               requiredPermissionName: PermissionNames.Pages_Administration_Roles)
            //           ).AddItem(
            //               new MenuItemDefinition(
            //                   PageNames.Menus,
            //                   "菜单管理",
            //                   "menu",
            //                   "/Menu/Index",
            //                   target: "tab_3",
            //                   requiredPermissionName: PermissionNames.Pages_Administration_Menus)
            //           )
            //   );
            return menus;
        }
    }
}