using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anc.Authorization;
using AutoMapper;
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
    [AncAuthorize(PermissionNames.Pages_Administration_Menus)]
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

        [AncAuthorize(PermissionNames.Pages_Administration_Menus)]
        [ResponseCache(CacheProfileName = "Header")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        [ResponseCache(CacheProfileName = "Header")]
        [AncAuthorize(PermissionNames.Pages_Administration_Menus_Create, PermissionNames.Pages_Administration_Menus_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? menuId, int? parentId)
        {
            MenuEditDto output = new MenuEditDto()
            {
                ParentID = parentId
            };
            if (menuId.HasValue)
            {
                output = await _menuService.GetMenuForEditAsync(menuId.Value);
            }
            var viewModel = new CreateOrEditMenuModalViewModel();
            viewModel.Menu = output;
            List<MenuListDto> menus = await _menuService.GetAllMenuListAsync();
            List<SelectListItem> menuItems = _mapper.Map<List<SelectListItem>>(menus);
            viewModel.MenuList = menuItems;
            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}