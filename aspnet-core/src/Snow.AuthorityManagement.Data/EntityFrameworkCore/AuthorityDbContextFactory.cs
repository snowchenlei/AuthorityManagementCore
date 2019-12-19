using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Configuration;
using Snow.AuthorityManagement.Core.Web;

namespace Snow.AuthorityManagement.EntityFrameworkCore
{
    /// <summary>
    /// 设计时使用
    /// </summary>
    public class AuthorityDbContextFactory : IDesignTimeDbContextFactory<AuthorityManagementContext>
    {
        public AuthorityManagementContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AuthorityManagementContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), "Development");
            AuthorityManagementDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AuthorityManagementConsts.ConnectionStringName));
            return new AuthorityManagementContext(builder.Options);
        }
    }
}