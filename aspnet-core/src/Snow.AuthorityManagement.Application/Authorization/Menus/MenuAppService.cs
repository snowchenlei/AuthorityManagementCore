using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Anc;
using Anc.Application.Services.Dto;
using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Core.Authorization.Menus;
using Snow.AuthorityManagement.EntityFrameworkCore;

namespace Snow.AuthorityManagement.Application.Authorization.Menus
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuAppService : IMenuAppService
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
        public MenuAppService(
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
        public async Task<List<MenuListDto>> GetAllMenuListAsync()
        {
            List<Menu> menus = await _menuRepository.GetAllListAsync();
            return _mapper.Map<List<MenuListDto>>(menus);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MenuListDto>> GetPagedMenuAsync(GetMenusInput input)
        {
            List<string> wheres = new List<string>();
            List<object> parameters = new List<object>();
            int index = 0;
            if (!String.IsNullOrWhiteSpace(input.Name))
            {
                wheres.Add($"Name.Contains(@{index++})");
                parameters.Add(input.Name);
            }
            if (input.ParentID.HasValue)
            {
                wheres.Add($"ParentID=@{index++}");
                parameters.Add(input.ParentID);
            }
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                input.Sorting = input.Sorting + (input.Order == OrderType.ASC ? " ASC" : " DESC");
            }
            var result = await _menuRepository
                .GetPagedAsync(input.PageIndex, input.PageSize, string.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
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
            Menu menu = await _menuRepository.GetAsync(menuId)
                ?? throw new UserFriendlyException("菜单不存在");
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
            Menu parentMenu = await GetParentMenuAsync(input.ParentID);
            Menu menu = _mapper.Map<Menu>(input);
            menu.Parent = parentMenu;
            menu.LastModificationTime = menu.CreationTime;
            menu.Id = await _menuRepository.InsertAndGetIdAsync(menu);
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
            Menu parentMenu = await GetParentMenuAsync(input.ParentID);
            Menu menu = _mapper.Map(input, oldMenu);
            menu.Parent = parentMenu;
            menu.LastModificationTime = DateTime.Now;
            await _menuRepository.UpdateAsync(menu);
            await _currentContext.SaveChangesAsync();
            return _mapper.Map<MenuListDto>(menu);
        }

        private async Task<Menu> GetParentMenuAsync(int? parentId)
        {
            Menu parentMenu = null;
            if (parentId.HasValue)
            {
                parentMenu = await _menuRepository.GetAsync(parentId.Value);
            }
            return parentMenu;
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