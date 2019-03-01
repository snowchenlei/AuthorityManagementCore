using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Snow.AuthorityManagement.Data
{
    /// <summary>
    /// 设计时使用
    /// </summary>
    public class AuthorityDbContextFactory : IDesignTimeDbContextFactory<AuthorityManagementContext>
    {
        public AuthorityManagementContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AuthorityManagementContext>();

            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AuthCore1;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new AuthorityManagementContext(builder.Options);
        }
    }
}