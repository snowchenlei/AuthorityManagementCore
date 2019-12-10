using System;
using System.Threading.Tasks;
using Anc.Authorization.Permissions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snow.AuthorityManagement.EntityFrameworkCore.Seed.Host;

namespace Snow.AuthorityManagement.EntityFrameworkCore.Seed
{
    public class SeedHelper
    {
        public static async Task SeedHostDbAsync(AuthorityManagementContext context, IServiceProvider serviceProvider)
        {
            await new HostRoleAndUserCreator(context, serviceProvider).CreateAsync();
            await new HostMenuCreator(context).CreateAsync();
        }
    }
}