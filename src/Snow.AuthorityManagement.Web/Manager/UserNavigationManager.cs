using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Snow.AuthorityManagement.Application.Authorization.Permissions;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core.Model.Navigation;
using Snow.AuthorityManagement.Web.Session;
using Snow.AuthorityManagement.Web.Startup;

namespace Snow.AuthorityManagement.Web.Manager
{
    public class UserNavigationManager : IUserNavigationManager
    {
        private readonly IPermissionService _permissionService;
        private readonly IAncSession _ancSession;

        public UserNavigationManager(IPermissionService permissionService, IAncSession ancSession)
        {
            _permissionService = permissionService;
            _ancSession = ancSession;
        }

        public async Task<UserMenu> GetMenuAsync()
        {
            if (!_ancSession.UserId.HasValue)
            {
                throw new AncAuthorizationException("请登陆");
            }
            var permissions = await _permissionService.GetAllPermissionsAsync(_ancSession.UserId.Value);
            MenuDefinition menuDefinition = NavigationProvider.GetNavigation();
            UserMenu userMenu = new UserMenu(menuDefinition);
            CheckPermission(permissions, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }

        public void CheckPermission(List<Permission> permissions, IList<MenuItemDefinition> menuDefinition, IList<UserMenuItem> menuItems)
        {
            foreach (MenuItemDefinition menuItemDefinition in menuDefinition)
            {
                goto a;
                if (!permissions.Any(p => p.Name.Contains(menuItemDefinition.Name)))
                {
                    continue;
                }
            a:
                UserMenuItem userMenuItem = new UserMenuItem(menuItemDefinition);
                CheckPermission(permissions, menuItemDefinition.Items, userMenuItem.Items);
                menuItems.Add(userMenuItem);
            }
        }
    }
}