using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Web.Library;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Application.Authorization.Roles;
using Snow.AuthorityManagement.Application.Authorization.Users;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Web.Mvc.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using Anc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using System.Linq;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    [Authorize(PermissionNames.Pages_Administration_Users)]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserAppService _userService;
        private readonly IRoleAppService _roleService;

        public UserController(
            IMapper mapper, IUserAppService userService, IRoleAppService roleService)
        {
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
        }

        [Authorize(PermissionNames.Pages_Administration_Users)]
        [ResponseCache(CacheProfileName = "Header")]
        public async Task<ActionResult> Index()
        {
            var roles = await _roleService.GetAllRoleListAsync();
            List<SelectListItem> roleItems = _mapper.Map<List<SelectListItem>>(roles);
            ViewBag.RoleList = roleItems;
            ViewBag.AbsoluteUrl = "/User";
            return View();
        }

        [Authorize(PermissionNames.Pages_Administration_Users_Query)]
        public async Task<JsonResult> Load(GetUsersInput input)
        {
            var result = await _userService.GetUserPagedAsync(input);
            return Json(new
            {
                total = result.TotalCount,
                rows = result.Items
            });
        }

        [HttpGet]
        [AjaxOnly]
        [ResponseCache(CacheProfileName = "Header")]
        [Authorize(PermissionNames.Pages_Administration_Users_Create)]
        public async Task<ActionResult> Create()
        {
            var roles = await _userService.GetUserRoleSelectAsync(null);
            CreateOrEditUserModalViewModel viewModel = new CreateOrEditUserModalViewModel
            {
                Roles = roles
            };
            return PartialView("_CreateModal", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        [ResponseCache(CacheProfileName = "Header")]
        [Authorize(PermissionNames.Pages_Administration_Users_Create)]
        public async Task<ActionResult> Edit(int id)
        {
            UserEditDto output = await _userService.GetUserForEditAsync(id);
            var roles = await _userService.GetUserRoleSelectAsync(id);
            CreateOrEditUserModalViewModel viewModel = new CreateOrEditUserModalViewModel
            {
                User = output,
                Roles = roles,
                Id = id
            };
            return PartialView("_EditModal", viewModel);
        }
    }
}