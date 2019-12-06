using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anc.Authorization;
using Anc.Authorization.Permissions;
using Anc.Domain.Entities;
using Anc.Users;

namespace Anc.Application.Navigation
{
    public class UserNavigationManager : IUserNavigationManager
    {
        private readonly IPermissionStore _permissionStore;
        private readonly INavigationProvider _navigationProvider;

        private readonly ICurrentUser _currentUser;

        public UserNavigationManager(
            IPermissionStore permissionStore
            , ICurrentUser currentUser
            , INavigationProvider navigationProvider)
        {
            _permissionStore = permissionStore;
            _navigationProvider = navigationProvider;
            _currentUser = currentUser;
        }

        public async Task<UserMenu> GetMenuAsync()
        {
            // TODO:获取用户信息
            if (!_currentUser.Id.HasValue)
            {
                throw new AncAuthorizationException("请登陆");
            }
            var permissions = await _permissionStore.GetAllPermissionsByUserIdAsync("admin");
            MenuDefinition menuDefinition = await _navigationProvider.GetNavigationAsync();
            UserMenu userMenu = new UserMenu(menuDefinition);
            CheckPermission(permissions, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }

        public void CheckPermission(IEnumerable<AncPermission> permissions, IList<MenuItemDefinition> menuDefinition, IList<UserMenuItem> menuItems)
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