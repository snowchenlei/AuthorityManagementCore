using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Session
{
    public interface IAncSession
    {
        int? UserId { get; }
    }
}