using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Anc.Dapper
{
    public abstract class DbContext
    {
        public IDbConnection DbConnection { get; protected set; }
    }
}