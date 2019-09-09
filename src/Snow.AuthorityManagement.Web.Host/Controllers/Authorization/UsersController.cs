using Anc.Application.Services.Dto;
using Anc.AspNetCore.Web.Mvc.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Users;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    [ApiController]
    [Route("api/user")]
    public class UsersController : PageController
    {
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IUserService userService) : base(mapper)
        {
            _userService = userService;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <remarks>GET: api/user</remarks>
        /// <param name="input">分页参数</param>
        /// <response code="200">获取成功</response>
        /// <returns></returns>
        [HttpGet(Name = "GetUsersPage")]
        [AncAuthorize(PermissionNames.Pages_Users_Query)]
        [ProducesResponseType(typeof(PagedResultDto<UserListDto>), 200)]
        public async Task<IActionResult> GetPaged(GetUserInput input)
        {
            var result = await _userService.GetPagedAsync(input);
            return Return<GetUserInput, PagedResultDto<UserListDto>, UserListDto>(input, "GetUsersPage", result);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <remarks>GET: api/user/{id}</remarks>
        /// <param name="id">主键</param>
        /// <response code="200">获取成功</response>
        /// <response code="304">没有修改</response>
        /// <response code="404">没有找到</response>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(typeof(GetUserForEditOutput), 200)]
        [ProducesResponseType(304)]
        [ProducesResponseType(404)]
        [AncAuthorize(PermissionNames.Pages_Users_Query)]
        public async Task<IActionResult> Get(int id)
        {
            GetUserForEditOutput result = await _userService.GetForEditAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <remarks>POST: api/user</remarks>
        /// <param name="input">创建参数</param>
        /// <response code="201">创建成功</response>
        /// <response code="400">请求参数有误</response>
        /// <returns></returns>
        [HttpPost(Name = "PostUser")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AncAuthorize(PermissionNames.Pages_Users_Create)]
        public async Task<IActionResult> Post([FromBody]CreateOrUpdateUser input)
        {
            var result = await _userService.CreateAsync(input.User, input.RoleIds);
            return CreatedAtRoute("GetUser", new { id = result.ID }, result);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <remarks>PUT: api/user/5</remarks>
        /// <param name="id">主键</param>
        /// <param name="input">修改参数</param>
        /// <response code="204">修改成功</response>
        /// <response code="400">请求参数有误</response>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [AncAuthorize(PermissionNames.Pages_Users_Edit)]
        public async Task<IActionResult> Put(int id, [FromBody]CreateOrUpdateUser input)
        {
            input.User.ID = id;
            await _userService.EditAsync(input.User, input.RoleIds);
            return NoContent();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <remarks>DELETE: api/user/5</remarks>
        /// <param name="id">主键</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">没有找到</response>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [AncAuthorize(PermissionNames.Pages_Users_Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}