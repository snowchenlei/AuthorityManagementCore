using Anc.Application.Services;
using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Application.Authorization.Menus
{
    /// <summary>
    /// 菜单应用服务
    /// </summary>
    public interface IMenuService : IApplicationService
    {
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        Task<List<MenuListDto>> GetAllMenuListAsync();

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        Task<PagedResultDto<MenuListDto>> GetPagedMenuAsync(GetMenuInput input);

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="menuId">用户编号</param>
        /// <returns></returns>
        Task<MenuEditDto> GetMenuForEditAsync(int menuId);

        /// <summary>
        /// 添加修改菜单
        /// </summary>
        /// <param name="input">信息</param>
        /// <returns>用户列表数据</returns>
        Task<MenuListDto> CreateOrEditMenuAsync(CreateOrUpdateMenu input);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input">数据信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        Task<MenuListDto> CreateMenuAsync(MenuEditDto input);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="input">数据信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        Task<MenuListDto> EditMenuAsync(MenuEditDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="menuId">编号</param>
        /// <returns></returns>
        Task<bool> DeleteMenuAsync(int menuId);
    }
}