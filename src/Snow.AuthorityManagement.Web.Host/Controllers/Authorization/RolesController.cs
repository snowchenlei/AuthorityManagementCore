using Anc.Application.Services.Dto;
using Anc.AspNetCore.Web.Mvc.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Roles;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    [ApiController]
    [Route("api/role")]
    [AncAuthorize(PermissionNames.Pages_Roles)]
    public class RolesController : PageController
    {
        private readonly IRoleService _roleService;

        public RolesController(IMapper mapper
            , IRoleService roleService) : base(mapper)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <remarks>GET: api/role</remarks>
        /// <param name="input">分页参数</param>
        /// <response code="200">获取成功</response>
        /// <returns></returns>
        [HttpGet(Name = "GetRolesPage")]
        [AncAuthorize(PermissionNames.Pages_Roles_Query)]
        [ProducesResponseType(typeof(PagedResultDto<RoleListDto>), 200)]
        public async Task<IActionResult> GetPaged(GetRoleInput input)
        {
            var result = await _roleService.GetPagedAsync(input);
            return Return<GetRoleInput, PagedResultDto<RoleListDto>, RoleListDto>(input, "GetUsersPage", result);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <remarks>GET: api/role/{id}</remarks>
        /// <param name="id">主键</param>
        /// <response code="200">获取成功</response>
        /// <response code="304">没有修改</response>
        /// <response code="404">没有找到</response>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetRole")]
        [ProducesResponseType(typeof(GetRoleForEditOutput), 200)]
        [ProducesResponseType(304)]
        [ProducesResponseType(404)]
        [AncAuthorize(PermissionNames.Pages_Roles_Query)]
        public async Task<IActionResult> Get(int id)
        {
            GetRoleForEditOutput result = await _roleService.GetForEditAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <remarks>POST: api/role</remarks>
        /// <param name="input">创建参数</param>
        /// <response code="201">创建成功</response>
        /// <response code="400">请求参数有误</response>
        /// <returns></returns>
        [HttpPost(Name = "PostRole")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AncAuthorize(PermissionNames.Pages_Roles_Create)]
        public async Task<IActionResult> Post([FromBody]CreateOrUpdateRole input)
        {
            var result = await _roleService.CreateAsync(input.Role, input.Permission);
            return CreatedAtRoute("GetRole", new { id = result.ID }, result);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <remarks>PUT: api/role/5</remarks>
        /// <param name="id">主键</param>
        /// <param name="input">修改参数</param>
        /// <response code="204">修改成功</response>
        /// <response code="400">请求参数有误</response>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [AncAuthorize(PermissionNames.Pages_Roles_Edit)]
        public async Task<IActionResult> Put(int id, [FromBody]CreateOrUpdateRole input)
        {
            input.Role.ID = id;
            await _roleService.EditAsync(input.Role, input.Permission);
            return NoContent();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <remarks>DELETE: api/role/5</remarks>
        /// <param name="id">主键</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">没有找到</response>
        [HttpDelete("{id}", Name = "DeleteRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [AncAuthorize(PermissionNames.Pages_Roles_Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleService.DeleteAsync(id);
            return NoContent();
        }
    }
}