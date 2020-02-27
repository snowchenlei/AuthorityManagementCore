using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Anc.Authorization
{
    public interface IAncAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        Task<List<string>> GetPoliciesNamesAsync();
    }
}