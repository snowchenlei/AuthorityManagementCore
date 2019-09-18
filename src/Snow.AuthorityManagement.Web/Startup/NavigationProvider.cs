using Anc.Application.Navigation;
using Snow.AuthorityManagement.Core;

namespace Snow.AuthorityManagement.Web.Startup
{
    public class NavigationProvider : INavigationProvider
    {
        MenuDefinition INavigationProvider.GetNavigation()
        {
            MenuDefinition menu = new MenuDefinition("MainMenu", "主菜单")
               .AddItem(new MenuItemDefinition(
                       PageNames.Users,
                       "系统管理",
                       "user"
                   ).AddItem(
                       new MenuItemDefinition(
                           PageNames.Users,
                           "用户管理",
                           "user",
                           "/User/Index",
                           target: "tab_1",
                           requiredPermissionName: PermissionNames.Pages_Users
                       )
                   ).AddItem(
                       new MenuItemDefinition(
                           PageNames.Roles,
                           "角色管理",
                           "user",
                           "/Role/Index",
                           target: "tab_2",
                           requiredPermissionName: PermissionNames.Pages_Roles)
                       ).AddItem(
                           new MenuItemDefinition(
                               PageNames.Menus,
                               "菜单管理",
                               "menu",
                               "/Menu/Index",
                               target: "tab_3",
                               requiredPermissionName: PermissionNames.Pages_Menus)
                       )
               );
            return menu;
        }
    }
}