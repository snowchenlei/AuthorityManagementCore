using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Snow.AuthorityManagement.Common.Encryption;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Enum;

namespace Snow.AuthorityManagement.Data.Seed
{
    public class HostRoleAndUserCreator
    {
        private readonly AuthorityManagementContext _context;
        private readonly IConfiguration _configuration;

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
            var adminRoleForHost = await _context.Role.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (adminRoleForHost == null)
            {
                adminRoleForHost = (await _context.Role.AddAsync(new Role(){ Name = "系统管理员", Sort = 1})).Entity;
                await _context.SaveChangesAsync();
            }
            //TODO:权限获取
            var permissions = new List<Permission>();
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
