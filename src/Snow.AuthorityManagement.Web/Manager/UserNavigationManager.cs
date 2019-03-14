using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Core.Dto.User;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core.Model.Navigation;
using Snow.AuthorityManagement.IService.Authorization;
using Snow.AuthorityManagement.Web.Startup;

namespace Snow.AuthorityManagement.Web.Manager
{
    public class UserNavigationManager : IUserNavigationManager
    {
        private readonly IPermissionService _permissionService;
        private readonly ISession _session;

        public UserNavigationManager(IHttpContextAccessor httpContextAccessor, IPermissionService permissionService)
        {
            _permissionService = permissionService;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public async Task<UserMenu> GetMenuAsync()
        {
            string session = _session.GetString("LoginUser");
            if (String.IsNullOrEmpty(session))
            {
                throw new AncAuthorizationException("请登陆");
            }
            UserLoginOutput user = Serialization.DeserializeObject<UserLoginOutput>(session);
            MenuDefinition menuDefinition = NavigationProvider.GetNavigation();

            var permissions = await _permissionService.GetAllPermissionsAsync(user.ID);
            UserMenu userMenu = new UserMenu(menuDefinition);
            CheckPermission(permissions, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }

        public void CheckPermission(List<Permission> permissions, IList<MenuItemDefinition> menuDefinition, IList<UserMenuItem> menuItems)
        {
            foreach (MenuItemDefinition menuItemDefinition in menuDefinition)
            {
                if (!permissions.Any(p => p.Name.Contains(menuItemDefinition.Name)))
                {
                    continue;
                }
                UserMenuItem userMenuItem = new UserMenuItem(menuItemDefinition);
                CheckPermission(permissions, menuItemDefinition.Items, userMenuItem.Items);
                menuItems.Add(userMenuItem);
            }
        }
    }
}