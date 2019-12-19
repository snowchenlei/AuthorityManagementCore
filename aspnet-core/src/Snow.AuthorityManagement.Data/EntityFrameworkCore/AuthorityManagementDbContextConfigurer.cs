using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snow.AuthorityManagement.EntityFrameworkCore
{
    public static class AuthorityManagementDbContextConfigurer
    {
        public static void Configure<T>(DbContextOptionsBuilder<T> dbContextOptions, string connectionString)
            where T : DbContext
        {
            dbContextOptions.UseMySql(connectionString);
        }

        public static void Configure<T>(DbContextOptionsBuilder<T> builder, DbConnection connection)
            where T : DbContext
        {
            builder.UseMySql(connection);
        }
    }
}