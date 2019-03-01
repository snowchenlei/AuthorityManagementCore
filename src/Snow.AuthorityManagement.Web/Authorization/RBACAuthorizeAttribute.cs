using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Snow.AuthorityManagement.Web.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RBACAuthorizeAttribute : AuthorizeAttribute
    {
        public string[] Permissions { get; set; }

        public bool RequireAllPermissions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="RBACAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public RBACAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }
    }
}