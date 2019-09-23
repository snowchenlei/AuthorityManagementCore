using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Model;

namespace Snow.AuthorityManagement.Web.Authorization
{
    public interface IAuthorizationHelper
    {
        Task CheckPermissionsAsync(MethodInfo methodInfo, Type type);
    }
}