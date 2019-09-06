using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Anc.Authorization
{
    public interface IAuthorizationHelper
    {
        Task CheckPermissionsAsync(MethodInfo methodInfo, Type type);
    }
}