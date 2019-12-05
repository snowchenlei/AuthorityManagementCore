using Anc.Application.Navigation;
using Snow.AuthorityManagement.Core.Authorization.Menus;
using Snow.AuthorityManagement.Core.Authorization.Menus.DomainService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Repository.Authorization.Menus.DomainService
{
    public class MenuManager : AuthorityManagementDomainServiceBase, IMenuManager
    {
        private readonly IMenuRepository _menuRepository;

        public MenuManager(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<MenuDefinition> CreateMenuDefinitionAsync()
        {
            MenuDefinition menuDefinition = new MenuDefinition("MainMenu", "主菜单");
            List<Menu> menus = await _menuRepository.GetAllListAsync();
            MenuItemDefinition menuItemDefinition;
            foreach (Menu menu in menus.Where(m => m.ParentID == null).OrderBy(m => m.Sort))
            {
                menuItemDefinition = new MenuItemDefinition(
                    menu.Name,
                    menu.Name,
                    menu.Icon,
                    menu.Route,
                    order: menu.Sort,
                    requiredPermissionName: menu.PermissionName
                );
                CreateChild(menuItemDefinition, menus.Where(m => m.Parent?.Id == menu.Id).OrderBy(m => m.Sort));
                menuDefinition.AddItem(menuItemDefinition);
            }
            return menuDefinition;
        }

        private void CreateChild(MenuItemDefinition menuItemDefinition, IEnumerable<Menu> menus)
        {
            MenuItemDefinition newMenuItemDefinition;
            foreach (Menu menu in menus)
            {
                newMenuItemDefinition = new MenuItemDefinition(
                    menu.Name,
                    menu.Name,
                    menu.Icon,
                    menu.Route,
                    target: "tab_" + menu.Id,
                    requiredPermissionName: menu.PermissionName,
                    order: menu.Sort
                );
                CreateChild(newMenuItemDefinition, menus.Where(m => m.Parent?.Id == menu.Id).OrderBy(m => m.Sort));
                menuItemDefinition.AddItem(newMenuItemDefinition);
            }
        }
    }
}