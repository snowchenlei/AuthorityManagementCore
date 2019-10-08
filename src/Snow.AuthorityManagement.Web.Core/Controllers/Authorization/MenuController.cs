using Anc.Application.Services.Dto;
using Anc.AspNetCore.Web.Mvc.Authorization;
using AutoMapper;
using CacheCow.Server.Core.Mvc;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Menus;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Route("api/menus")]
    public class MenuController : PageController
    {
        private readonly IMenuAppService _menuService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="menuService"></param>
        public MenuController(IMapper mapper
            , IMenuAppService menuService) : base(mapper)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <remarks>GET: api/menus</remarks>
        /// <param name="input">分页参数</param>
        /// <response code="200">获取成功</response>
        /// <returns></returns>
        [HttpGet(Name = "GetMenusPage")]
        //[HttpCacheFactory(0, ViewModelType = typeof(PagedResultDto<MenuListDto>))]
        [AncAuthorize(PermissionNames.Pages_Administration_Menus_Query)]
        [ProducesResponseType(typeof(PagedResultDto<MenuListDto>), 200)]
        public async Task<IActionResult> GetPaged([FromQuery]GetMenuInput input)
        {
            var result = await _menuService.GetPagedMenuAsync(input);
            return Return<GetMenuInput, PagedResultDto<MenuListDto>, MenuListDto>(input, "GetUsersPage", result);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <remarks>GET: api/menus/{id}</remarks>
        /// <param name="id">主键</param>
        /// <response code="200">获取成功</response>
        /// <response code="304">没有修改</response>
        /// <response code="404">没有找到</response>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetMenu")]
        //[HttpCacheFactory(0, ViewModelType = typeof(MenuEditDto))]
        [ProducesResponseType(typeof(MenuEditDto), 200)]
        [ProducesResponseType(304)]
        [ProducesResponseType(404)]
        [AncAuthorize(PermissionNames.Pages_Administration_Menus_Query)]
        public async Task<IActionResult> Get(int id)
        {
            MenuEditDto result = await _menuService.GetMenuForEditAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <remarks>POST: api/menus</remarks>
        /// <param name="input">创建参数</param>
        /// <response code="201">创建成功</response>
        /// <response code="400">请求参数有误</response>
        /// <returns></returns>
        [HttpPost(Name = "PostMenu")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AncAuthorize(PermissionNames.Pages_Administration_Menus_Create)]
        public async Task<IActionResult> Post([FromBody]CreateOrUpdateMenu input)
        {
            var result = await _menuService.CreateMenuAsync(input.Menu);
            return CreatedAtRoute("GetMenu", new { id = result.ID }, result);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <remarks>PUT: api/menus/{id}</remarks>
        /// <param name="id">主键</param>
        /// <param name="input">修改参数</param>
        /// <response code="204">修改成功</response>
        /// <response code="400">请求参数有误</response>
        /// <returns></returns>
        [HttpPut("{id}", Name = "PutMenu")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [AncAuthorize(PermissionNames.Pages_Administration_Menus_Edit)]
        public async Task<IActionResult> Put(int id, [FromBody]CreateOrUpdateMenu input)
        {
            input.Menu.ID = id;
            await _menuService.EditMenuAsync(input.Menu);
            return NoContent();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <remarks>DELETE: api/menus/{id}</remarks>
        /// <param name="id">主键</param>
        /// <response code="204">删除成功</response>
        /// <response code="404">没有找到</response>
        [HttpDelete("{id}", Name = "DeleteMenu")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [AncAuthorize(PermissionNames.Pages_Administration_Menus_Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _menuService.DeleteMenuAsync(id);
            return NoContent();
        }
    }
}