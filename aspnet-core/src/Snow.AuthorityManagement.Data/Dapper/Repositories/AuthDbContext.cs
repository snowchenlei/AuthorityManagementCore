using Anc.Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Snow.AuthorityManagement.Data.Dapper.Repositories
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(IConfiguration configuration)
        {
            DbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}