using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anc.Authorization
{
    public interface IPermissionManagerBase
    {
        Task<IEnumerable<IPermission>> GetAllPermissionsByUserIdAsync(int userId);
    }
}