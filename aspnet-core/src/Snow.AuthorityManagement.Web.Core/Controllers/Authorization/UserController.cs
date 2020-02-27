using Anc.Application.Services.Dto;
using AutoMapper;
using CacheCow.Server.Core.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Snow.AuthorityManagement.Application.Authorization.Users;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core;
using System;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/users")]
    [Authorize(PermissionNames.Pages_Administration_Users)]
    public class UserController : PageController
    {
        // TODO:Api缓存隔离
        private readonly IUserAppService _userService;

        private readonly IDistributedCache _cache;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userService"></param>
        /// <param name="cache"></param>
        public UserController(IMapper mapper
            , IUserAppService userService
            , IDistributedCache cache) : base(mapper)
        {
            _userService = userService;
            _cache = cache;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <remarks>GET: api/user</remarks>
        /// <param name="input">分页参数</param>
        /// <response code="200">获取成功</response>
        /// <returns></returns>
        [HttpGet(Name = "GetUsersPage")]
        [HttpCacheFactory(0, ViewModelType = typeof(PagedResultDto<UserListDto>))]
        [Authorize(PermissionNames.Pages_Administration_Users_Query)]
        [ProducesResponseType(typeof(PagedResultDto<UserListDto>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery]GetUsersInput input)
        {
            var result = await _userService.GetUserPagedAsync(input);
            if (!await _cache.ExistsStringAsync(AuthorityManagementConsts.UserLastResponseCache))
            {
                DateTime? lastModificationTime = await _userService.GetLastModificationTimeAsync();
                await _cache.SetStringAsync(AuthorityManagementConsts.UserLastResponseCache, (await _userService.GetLastModificationTimeAsync()).ToString());
            }
            return Return<GetUsersInput, PagedResultDto<UserListDto>, UserListDto>(input, "GetUsersPage", result);
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
        [HttpCacheFactory(0, ViewModelType = typeof(GetUserForEditOutput))]
        [ProducesResponseType(typeof(GetUserForEditOutput), 200)]
        [ProducesResponseType(304)]
        [ProducesResponseType(404)]
        [Authorize(PermissionNames.Pages_Administration_Users_Query)]
        public async Task<IActionResult> Get(int id)
        {
            UserEditDto user = await _userService.GetUserForEditAsync(id);
            DateTime? lastModified = user.LastModificationTime;
            await _cache.SetStringAsync(String.Format(AuthorityManagementConsts.UserResponseCache, id), lastModified.ToString());
            return Ok(user);
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
        [Authorize(PermissionNames.Pages_Administration_Users_Create)]
        public async Task<IActionResult> Post([FromBody]CreateOrUpdateUser input)
        {
            UserListDto user = await _userService.CreateUserAsync(input.User, input.RoleIds);
            await CacheLastModificationTimeAsync(user.LastModificationTime);
            return CreatedAtRoute("GetUser", new { id = user.ID }, user);
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
        [Authorize(PermissionNames.Pages_Administration_Users_Edit)]
        public async Task<IActionResult> Put(int id, [FromBody]CreateOrUpdateUser input)
        {
            input.User.ID = id;
            UserListDto user = await _userService.EditUserAsync(input.User, input.RoleIds);
            _cache.Remove(String.Format(AuthorityManagementConsts.UserResponseCache, id));
            await CacheLastModificationTimeAsync(user.LastModificationTime);
            return NoContent();
        }

        /// <summary>
        /// 缓存最后修改时间
        /// </summary>
        /// <param name="lastModified"></param>
        private async Task CacheLastModificationTimeAsync(DateTime? lastModified)
        {
            string cacheTime = await _cache.GetStringAsync(AuthorityManagementConsts.UserLastResponseCache);
            if (cacheTime != null)
            {
                if (lastModified.HasValue && Convert.ToDateTime(cacheTime) < lastModified.Value)
                {
                    await _cache.SetStringAsync(AuthorityManagementConsts.UserLastResponseCache, lastModified.ToString());
                }
            }
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
        [Authorize(PermissionNames.Pages_Administration_Users_Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            _cache.Remove(String.Format(AuthorityManagementConsts.UserResponseCache, id));
            return NoContent();
        }
    }
}