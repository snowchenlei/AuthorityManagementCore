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
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService = null;

        public RoleController(
            IRoleService roleService)
        {
            _roleService = roleService;
        }

        //[ResponseCache(CacheProfileName = "Header")]
        public ActionResult Index()
        {
            return View();
        }

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

        private GetRoleForEditOutput Create()
        {
            return new GetRoleForEditOutput()
            {
                Role = null
            };
        }

        private async Task<GetRoleForEditOutput> Edit(int roleId)
        {
            return await _roleService.GetForEditAsync(roleId);
        }

        [HttpPost]
        [AjaxOnly]
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

        public Task<RoleListDto> Create(RoleEditDto input)
        {
            return _roleService.AddAsync(input);
        }

        public Task<RoleListDto> Edit(RoleEditDto input)
        {
            return _roleService.EditAsync(input);
        }

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