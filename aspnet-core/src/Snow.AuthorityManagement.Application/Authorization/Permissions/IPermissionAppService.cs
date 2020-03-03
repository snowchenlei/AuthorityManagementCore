using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services;
using Anc.Domain.Entities;
using Snow.AuthorityManagement.Core.Authorization.Permissions;

namespace Snow.AuthorityManagement.Application.Authorization.Permissions
{
    /// <summary>
    /// 权限应用服务
    /// </summary>
    public interface IPermissionAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task<List<AncPermission>> GetUserPermissionsAsync(int userId);
    }
}