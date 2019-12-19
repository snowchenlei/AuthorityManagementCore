using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Menus;
using Snow.AuthorityManagement.EntityFrameworkCore;

namespace Snow.AuthorityManagement.EntityFrameworkCore.Seed.Host
{
    public class HostMenuCreator
    {
        private readonly AuthorityManagementContext _context;

        public HostMenuCreator(AuthorityManagementContext context)
        {
            _context = context;
        }

        public async Task CreateAsync()
        {
            await CreateMenuAsync();
        }

        private async Task CreateMenuAsync()
        {
            DateTime now = DateTime.Now;
            Menu systemMenu = await _context.Menu.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Name == "系统管理");
            if (systemMenu == null)
            {
                systemMenu = (await _context.Menu.AddAsync(new Menu()
                {
                    Name = "系统管理",
                    Parent = null,
                    Sort = 0,
                    CreationTime = now,
                    LastModificationTime = now,
                })).Entity;
                await _context.SaveChangesAsync();
            }
            // TODO:权限名称
            List<Menu> menus = await _context.Menu.IgnoreQueryFilters()
                .Where(a => new string[] { "用户管理", "角色管理", "菜单管理" }.Contains(a.Name))
                .ToListAsync();
            if (!menus.Any(a => a.Name == "用户管理"))
            {
                await _context.Menu.AddAsync(new Menu()
                {
                    Name = "用户管理",
                    PermissionName = PermissionNames.Pages_Administration_Users,
                    ParentID = systemMenu.Id,
                    Route = "/User/Index",
                    Sort = 2,
                    CreationTime = now,
                    LastModificationTime = now,
                });
            }
            if (!menus.Any(a => a.Name == "角色管理"))
            {
                await _context.Menu.AddAsync(new Menu()
                {
                    Name = "角色管理",
                    PermissionName = PermissionNames.Pages_Administration_Roles,
                    ParentID = systemMenu.Id,
                    Route = "/Role/Index",
                    Sort = 5,
                    CreationTime = now,
                    LastModificationTime = now,
                });
            }
            if (!menus.Any(a => a.Name == "菜单管理"))
            {
                await _context.Menu.AddAsync(new Menu()
                {
                    Name = "菜单管理",
                    PermissionName = PermissionNames.Pages_Administration_Menus,
                    ParentID = systemMenu.Id,
                    Route = "/Menu/Index",
                    Sort = 7,
                    CreationTime = now,
                    LastModificationTime = now,
                });
            }
            await _context.SaveChangesAsync();
        }
    }
}