using Snow.AuthorityManagement.Core.Model.Navigation;
using Snow.AuthorityManagement.Web.Authorization;

namespace Snow.AuthorityManagement.Web.Startup
{
    public class NavigationProvider
    {
        public static MenuDefinition GetNavigation()
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
                            requiredPermissionName: PermissionNames.Pages_Users
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.Roles,
                            "角色管理",
                            "user",
                            "/Role/Index",
                            requiredPermissionName: PermissionNames.Pages_Roles))
                );
            return menu;
        }
    }
}