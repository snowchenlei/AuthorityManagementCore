using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Anc.Runtime.Session
{
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}