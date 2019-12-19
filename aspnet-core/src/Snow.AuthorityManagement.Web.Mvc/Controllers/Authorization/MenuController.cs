using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anc.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snow.AuthorityManagement.Application.Authorization.Menus;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Web.Controllers;
using Snow.AuthorityManagement.Web.Library;
using Snow.AuthorityManagement.Web.Mvc.Models.Menus;

namespace Snow.AuthorityManagement.Web.Mvc.Controllers.Authorization
{
    [Authorize(PermissionNames.Pages_Administration_Menus)]
    public class MenuController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMenuAppService _menuService;

        public MenuController(IMapper mapper
            , IMenuAppService menuService)
        {
            _mapper = mapper;
            _menuService = menuService;
        }

        [Authorize(PermissionNames.Pages_Administration_Menus)]
        [ResponseCache(CacheProfileName = "Header")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        [ResponseCache(CacheProfileName = "Header")]
        [Authorize(PermissionNames.Pages_Administration_Menus_Create)]
        public async Task<ActionResult> Create(int? parentId)
        {
            var viewModel = new CreateOrEditMenuModalViewModel();
            List<MenuListDto> menus = await _menuService.GetAllMenuListAsync();
            List<SelectListItem> menuItems = _mapper.Map<List<SelectListItem>>(menus);
            viewModel.MenuList = menuItems;
            if (parentId.HasValue)
            {
                viewModel.Menu = new MenuEditDto() { ParentID = parentId };
            }
            return PartialView("_CreateModal", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        [ResponseCache(CacheProfileName = "Header")]
        [Authorize(PermissionNames.Pages_Administration_Menus_Edit)]
        public async Task<ActionResult> Edit(int menuId)
        {
            MenuEditDto output = await _menuService.GetMenuForEditAsync(menuId);
            var viewModel = new CreateOrEditMenuModalViewModel();
            List<MenuListDto> menus = await _menuService.GetAllMenuListAsync();
            List<SelectListItem> menuItems = _mapper.Map<List<SelectListItem>>(menus);
            viewModel.Menu = output;
            viewModel.MenuList = menuItems;
            return PartialView("_EditModal", viewModel);
        }
    }
}