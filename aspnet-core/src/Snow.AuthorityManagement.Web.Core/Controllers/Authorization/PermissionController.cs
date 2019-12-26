using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services.Dto;
using Anc.Domain.Entities;
using Anc.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Permissions;
using Snow.AuthorityManagement.Application.Authorization.Permissions.Dto;
using Snow.AuthorityManagement.Core;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    /// <summary>
    /// 权限
    /// </summary>
    [Route("api/permissions")]
    [Authorize(PermissionNames.Pages_Administration_Logs)]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionAppService _permissionService;
        private readonly ICurrentUser _user;

        public PermissionController(IPermissionAppService permissionService,
            ICurrentUser user)
        {
            this._permissionService = permissionService;
            this._user = user;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <remarks>GET: api/user</remarks>
        /// <param name="input">分页参数</param>
        /// <response code="200">获取成功</response>
        /// <returns></returns>
        [HttpGet(Name = "GetAllPermissions")]
        [Authorize(PermissionNames.Pages_Administration_Users_Query)]
        [ProducesResponseType(typeof(List<IEnumerable<string>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<string> permissions = await _permissionService.GetUserPermissionsAsync(_user.Roles);
            return Ok(new Result<IEnumerable<string>>
            {
                Status = Status.Success,
                Data = permissions
            });
        }
    }
}