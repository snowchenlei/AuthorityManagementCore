using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anc;
using Anc.Authorization;
using Anc.Authorization.Permissions;
using Anc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Snow.AuthorityManagement.Common.Encryption;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Snow.AuthorityManagement.Enum;

namespace Snow.AuthorityManagement.EntityFrameworkCore.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly AuthorityManagementContext _context;
        private readonly IServiceProvider _serviceProvider;

        public HostRoleAndUserCreator(AuthorityManagementContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task CreateAsync()
        {
            await CreateHostRoleAndUsersAsync();
        }

        private async Task CreateHostRoleAndUsersAsync()
        {
            //TODO: 初始化数据
            // Admin role for host
            var adminRoleForHost = await _context.Role.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (adminRoleForHost == null)
            {
                adminRoleForHost = (await _context.Role.AddAsync(new Role()
                {
                    Name = StaticRoleNames.Host.Name,
                    DisplayName = StaticRoleNames.Host.DisplayName,
                    CreationTime = DateTime.Now
                })).Entity;
                await _context.SaveChangesAsync();
            }
            // Grant all permissions to admin role for host
            var grantedPermissions = _context.Permission.IgnoreQueryFilters()
               .Where(p => p.ProviderName == AncConsts.PermissionRoleProviderName && p.ProviderKey == adminRoleForHost.Name)
               .Select(p => p.Name)
               .ToList();
            using (var scope = _serviceProvider.CreateScope())
            {
                IPermissionDefinitionManager permissionDefinitionManager = scope.ServiceProvider.GetService<IPermissionDefinitionManager>();
                var permissions = permissionDefinitionManager
                    .GetPermissions()
                    .Where(a=> !grantedPermissions.Contains(a.Name))
                    .ToList();

                if (permissions.Any())
                {
                    await _context.Permission.AddRangeAsync(
                        permissions.Select(permission => new AncPermission()
                        {
                            Name = permission.Name,
                            ProviderName = AncConsts.PermissionRoleProviderName,
                            ProviderKey = adminRoleForHost.Name
                        })
                    );
                    await _context.SaveChangesAsync();
                }

                var adminUserForHost = await _context.User.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.UserName == "admin");
                if (adminUserForHost == null)
                {
                    var user = new User
                    {
                        UserName = "admin",
                        Name = "系统管理员",
                        CanUse = true,
                        CreationTime = DateTime.Now
                    };
                    string password = scope.ServiceProvider.GetService<IConfiguration>()["AppSetting:DefaultPassword"];
                    user.Password = Md5Encryption.Encrypt(password);

                    adminUserForHost = (await _context.User.AddAsync(user)).Entity;
                    await _context.SaveChangesAsync();

                    await _context.Set<UserRole>().AddAsync(new UserRole()
                    {
                        UserID = adminUserForHost.Id,
                        RoleID = adminRoleForHost.Id
                    });
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}