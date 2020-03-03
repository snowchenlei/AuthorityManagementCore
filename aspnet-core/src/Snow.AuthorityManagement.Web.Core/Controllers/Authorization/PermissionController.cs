using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services.Dto;
using Anc.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Application.Authorization.Permissions;
using Snow.AuthorityManagement.Core;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    /// <summary>
    /// 权限
    /// </summary>
    [Route("api/permissions")]
    public class PermissionController : PageController
    {
        private readonly ICurrentUser _user;
        private readonly IPermissionAppService _permissionService;

        public PermissionController(IMapper mapper
            , IPermissionAppService permissionService
            , ICurrentUser user) : base(mapper)
        {
            _permissionService = permissionService;
            _user = user;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <remarks>GET: api/menus</remarks>
        /// <response code="200">获取成功</response>
        /// <returns></returns>
        [HttpGet(Name = "GetPermissions")]
        [Authorize(PermissionNames.Pages_Administration_Menus_Query)]
        [ProducesResponseType(typeof(PagedResultDto<MenuListDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _permissionService.GetUserPermissionsAsync(_user.GetId());
            string[] permissions = result.Select(p => p.Name).ToArray();
            return Ok(permissions);
        }
    }
}
