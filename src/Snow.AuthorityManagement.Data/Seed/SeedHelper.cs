using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Snow.AuthorityManagement.Data.Seed
{
    public class SeedHelper
    {
        public static async Task SeedHostDbAsync(AuthorityManagementContext context)
        {
            await new HostRoleAndUserCreator(context).CreateAsync();
        }
    }
}
