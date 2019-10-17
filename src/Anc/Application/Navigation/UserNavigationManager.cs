using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anc.Authorization;
using Anc.Runtime.Session;
using Anc.Users;

namespace Anc.Application.Navigation
{
    public class UserNavigationManager : IUserNavigationManager
    {
        private readonly IPermissionManagerBase _permissionService;
        private readonly INavigationProvider _navigationProvider;
        private readonly ICurrentUser _currentUser;

        public UserNavigationManager(IPermissionManagerBase permissionService
            , ICurrentUser currentUser
            , INavigationProvider navigationProvider)
        {
            _permissionService = permissionService;
            _navigationProvider = navigationProvider;
            _currentUser = currentUser;
        }

        public async Task<UserMenu> GetMenuAsync()
        {
            if (!_currentUser.Id.HasValue)
            {
                throw new AncAuthorizationException("请登陆");
            }
            var permissions = await _permissionService.GetAllPermissionsByUserIdAsync(_currentUser.Id.Value);
            MenuDefinition menuDefinition = await _navigationProvider.GetNavigationAsync();
            UserMenu userMenu = new UserMenu(menuDefinition);
            CheckPermission(permissions, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }

        public void CheckPermission(IEnumerable<IPermission> permissions, IList<MenuItemDefinition> menuDefinition, IList<UserMenuItem> menuItems)
        {
            foreach (MenuItemDefinition menuItemDefinition in menuDefinition)
            {
                if (!String.IsNullOrEmpty(menuItemDefinition.RequiredPermissionName))
                {
                    if (!permissions.Any(p => p.Name == menuItemDefinition.RequiredPermissionName))
                    {
                        continue;
                    }
                }
                UserMenuItem userMenuItem = new UserMenuItem(menuItemDefinition);
                CheckPermission(permissions, menuItemDefinition.Items, userMenuItem.Items);
                menuItems.Add(userMenuItem);
            }
        }
    }
}