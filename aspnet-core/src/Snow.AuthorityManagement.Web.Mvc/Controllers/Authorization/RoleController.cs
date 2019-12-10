using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Roles;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Web.Library;
using Snow.AuthorityManagement.Web.Models.Roles;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    [Authorize(PermissionNames.Pages_Administration_Roles)]
    public class RoleController : BaseController
    {
        private readonly IRoleAppService _roleService;

        public RoleController(
            IRoleAppService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(PermissionNames.Pages_Administration_Roles)]
        [ResponseCache(CacheProfileName = "Header")]
        public ActionResult Index()
        {
            ViewBag.AbsoluteUrl = "/Role";
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        [Authorize(PermissionNames.Pages_Administration_Roles_Create)]
        public async Task<ActionResult> CreateAsync()
        {
            string permissions = await _roleService.GetPermissionsAsync(null);
            var viewModel = new CreateOrEditRoleModalViewModel()
            {
                Role = new RoleEditDto(),
                Permission = permissions
            };
            return PartialView("_CreateModal", viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        [Authorize(PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<ActionResult> Edit(int id)
        {
            var output = await _roleService.GetForEditAsync(id);
            string permissions = await _roleService.GetPermissionsAsync(output.Name);
            var viewModel = new CreateOrEditRoleModalViewModel()
            {
                Role = output,
                Permission = permissions
            };
            return PartialView("_EditModal", viewModel);
        }
    }
}