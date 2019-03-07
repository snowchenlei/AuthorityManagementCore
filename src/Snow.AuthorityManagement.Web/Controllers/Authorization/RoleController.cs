using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Dto.Role;
using Snow.AuthorityManagement.IService.Authorization;
using Snow.AuthorityManagement.Web.Authorization;
using Snow.AuthorityManagement.Web.Library;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    [RBACAuthorize(PermissionNames.Pages_Roles)]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService = null;

        public RoleController(
            IRoleService roleService)
        {
            _roleService = roleService;
        }

        [RBACAuthorize(PermissionNames.Pages_Roles)]
        [ResponseCache(CacheProfileName = "Header")]
        public ActionResult Index()
        {
            return View();
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Query)]
        public async Task<JsonResult> Load(GetRoleInput input)
        {
            var result = await _roleService.GetPagedAsync(input);
            return Json(new
            {
                total = result.TotalCount,
                rows = result.Items
            });
        }

        [HttpGet]
        [AjaxOnly]
        [RBACAuthorize(PermissionNames.Pages_Roles_Create, PermissionNames.Pages_Roles_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            if (id.HasValue)
            {
                return PartialView(await Edit(id.Value));
            }
            else
            {
                return PartialView(Create());
            }
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Create)]
        private GetRoleForEditOutput Create()
        {
            return new GetRoleForEditOutput()
            {
                Role = null
            };
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Edit)]
        private async Task<GetRoleForEditOutput> Edit(int roleId)
        {
            return await _roleService.GetForEditAsync(roleId);
        }

        [HttpPost]
        [AjaxOnly]
        [RBACAuthorize(PermissionNames.Pages_Roles_Create, PermissionNames.Pages_Roles_Edit)]
        public async Task<ActionResult> CreateOrEdit(CreateOrUpdateRole input)
        {
            if (ModelState.IsValid)
            {
                RoleListDto roleListDto;
                if (input.Role.ID.HasValue)
                {
                    roleListDto = await Edit(input.Role);
                }
                else
                {
                    roleListDto = await Create(input.Role);
                }

                return Json(new Result<RoleListDto>()
                {
                    Status = Status.Success,
                    Message = "操作成功",
                    Data = roleListDto
                });
            }
            else
            {
                IEnumerable<string> errors = ModelStateToArray();
                return Json(new Result
                {
                    Status = Status.Failure,
                    Message = "[" + string.Join(",", errors) + "]"
                });
            }
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Create)]
        public Task<RoleListDto> Create(RoleEditDto input)
        {
            return _roleService.AddAsync(input);
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Edit)]
        public Task<RoleListDto> Edit(RoleEditDto input)
        {
            return _roleService.EditAsync(input);
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Delete)]
        public async Task<JsonResult> Delete(int id)
        {
            if (await _roleService.DeleteAsync(id))
            {
                return Json(new Result()
                {
                    Status = Status.Success,
                    Message = "删除成功"
                });
            }
            return Json(new Result()
            {
                Status = Status.Failure,
                Message = "删除失败"
            });
        }
    }
}