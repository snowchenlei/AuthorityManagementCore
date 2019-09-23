using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Snow.AuthorityManagement.Common.Encryption;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Enum;

namespace Snow.AuthorityManagement.Data.Seed
{
    public class HostRoleAndUserCreator
    {
        private readonly IConfiguration _configuration;
        private readonly AuthorityManagementContext _context;

        public HostRoleAndUserCreator(AuthorityManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task CreateAsync()
        {
            await CreateHostRoleAndUsersAsync();
        }

        private async Task CreateHostRoleAndUsersAsync()
        {
            // Admin role for host
            var adminRoleForHost = await _context.Role.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (adminRoleForHost == null)
            {
                adminRoleForHost = (await _context.Role.AddAsync(new Role() { Name = StaticRoleNames.Host.Name, DisplayName = StaticRoleNames.Host.DisplayName })).Entity;
                await _context.SaveChangesAsync();
            }
            // Grant all permissions to admin role for host
            var grantedPermissions = _context.Permission.IgnoreQueryFilters()
               .Where(p => p.RoleID == adminRoleForHost.ID)
               .Select(p => p.Name)
               .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new AuthorityManagementAuthorizationProvider())
                .Where(p => !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                await _context.Permission.AddRangeAsync(
                    permissions.Select(permission => new Permission()
                    {
                        Name = permission.Name,
                        IsGranted = true,
                        Role = adminRoleForHost
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
                };

                user.Password = Md5Encryption.Encrypt(_configuration["AppSetting:DefaultPassword"]);

                adminUserForHost = (await _context.User.AddAsync(user)).Entity;
                await _context.SaveChangesAsync();

                await _context.Set<UserRole>().AddAsync(new UserRole()
                {
                    UserID = adminUserForHost.ID,
                    RoleID = adminRoleForHost.ID
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}