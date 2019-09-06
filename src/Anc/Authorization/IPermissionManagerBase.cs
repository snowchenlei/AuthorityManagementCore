using Anc.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Authorization
{
    public interface IPermissionManagerBase
    {
        Task<IEnumerable<IPermission>> GetAllPermissionsAsync(int userId);
    }
}