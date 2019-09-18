using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anc.AspNetCore.Web.Mvc.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Menus;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Web.Controllers;
using Snow.AuthorityManagement.Web.Library;
using Snow.AuthorityManagement.Web.Mvc.Models.Menus;

namespace Snow.AuthorityManagement.Web.Mvc.Controllers.Authorization
{
    [AncAuthorize(PermissionNames.Pages_Menus)]
    public class MenuController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMenuService _menuService;

        public MenuController(IMapper mapper
            , IMenuService menuService)
        {
            _mapper = mapper;
            _menuService = menuService;
        }

        [AncAuthorize(PermissionNames.Pages_Menus)]
        [ResponseCache(CacheProfileName = "Header")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        [ResponseCache(CacheProfileName = "Header")]
        [AncAuthorize(PermissionNames.Pages_Menus_Create, PermissionNames.Pages_Menus_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            MenuEditDto output = new MenuEditDto();
            if (id.HasValue)
            {
                output = await _menuService.GetMenuForEditAsync(id.Value);
            }
            var viewModel = new CreateOrEditMenuModalViewModel();
            _mapper.Map(output, viewModel);
            return PartialView("_CreateOrEditModal", viewModel);
        }
    }
}