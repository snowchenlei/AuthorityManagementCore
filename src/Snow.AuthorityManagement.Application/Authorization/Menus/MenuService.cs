using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services.Dto;
using Anc.Domain.Model;
using Anc.Domain.Repositories;
using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Core.Authorization.Menus;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Data;

namespace Snow.AuthorityManagement.Application.Authorization.Menus
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IMapper _mapper;
        private readonly IMenuRepository _menuRepository;
        private readonly AuthorityManagementContext _currentContext;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="menuRepository"></param>
        /// <param name="context"></param>
        public MenuService(
            IMapper mapper
            , IMenuRepository menuRepository
            , AuthorityManagementContext context)
        {
            _mapper = mapper;
            _menuRepository = menuRepository;
            _currentContext = context;
        }

        /// <summary>
        /// 菜单应用服务
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuListDto>> GetAllListAsync()
        {
            List<Menu> menus = await _menuRepository.GetAllListAsync();
            return _mapper.Map<List<MenuListDto>>(menus);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MenuListDto>> GetPagedMenuAsync(GetMenuInput input)
        {
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                input.Sorting = input.Sorting + (input.Order == OrderType.ASC ? " ASC" : " DESC");
            }
            var result = await _menuRepository
                .GetPagedAsync(input.PageIndex, input.PageSize, String.Empty, null, input.Sorting);
            return new PagedResultDto<MenuListDto>()
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                Items = _mapper.Map<List<MenuListDto>>(result.Item1),
                TotalCount = result.Item2
            };
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<MenuEditDto> GetMenuForEditAsync(int menuId)
        {
            Menu menu = await _menuRepository.GetAsync(menuId);
            return _mapper.Map<MenuEditDto>(menu);
        }

        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MenuListDto> CreateOrEditMenuAsync(CreateOrUpdateMenu input)
        {
            MenuListDto menuDto = null;
            if (input.Menu.ID.HasValue)
            {
                menuDto = await EditMenuAsync(input.Menu);
            }
            else
            {
                menuDto = await CreateMenuAsync(input.Menu);
            }
            return menuDto;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MenuListDto> CreateMenuAsync(MenuEditDto input)
        {
            if (await _menuRepository.IsExistsByNameAsync(input.Name))
            {
                throw new UserFriendlyException("菜单名已存在");
            }

            Menu menu = _mapper.Map<Menu>(input);
            menu.LastModificationTime = menu.CreationTime;
            menu.ID = await _menuRepository.InsertAndGetIdAsync(menu);
            await _currentContext.SaveChangesAsync();
            return _mapper.Map<MenuListDto>(menu);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MenuListDto> EditMenuAsync(MenuEditDto input)
        {
            int menuId = input.ID.Value;
            Menu oldMenu = await _menuRepository.GetAsync(menuId);
            Menu menu = _mapper.Map(input, oldMenu);
            menu.LastModificationTime = DateTime.Now;
            await _menuRepository.UpdateAsync(menu);
            await _currentContext.SaveChangesAsync();
            return _mapper.Map<MenuListDto>(menu);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMenuAsync(int menuId)
        {
            await _menuRepository.DeleteAsync(menuId);
            return await (_currentContext.SaveChangesAsync()) > 0;
        }
    }
}