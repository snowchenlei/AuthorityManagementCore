using AutoMapper;
using Snow.AuthorityManagement.Common;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Common.Encryption;
using Snow.AuthorityManagement.Enum;
using Snow.AuthorityManagement.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Dto.User;
using Snow.AuthorityManagement.Web.Authorization;
using Snow.AuthorityManagement.Web.Library;
using Microsoft.Extensions.Logging;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.IService.Authorization;

namespace Snow.AuthorityManagement.Web.Controllers
{
    //[RBACAuthorize(PermissionNames.Pages_Users)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService = null;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        //[RBACAuthorize(PermissionNames.Pages_Users)]
        [ResponseCache(CacheProfileName = "Header")]
        public ActionResult Index()
        {
            return View();
        }

        //[RBACAuthorize(PermissionNames.Pages_Users_Query)]
        public async Task<JsonResult> Load(GetUserInput input)
        {
            var result = await _userService.GetPagedAsync(input);
            return Json(new
            {
                total = result.TotalCount,
                rows = result.Items
            });
        }

        [HttpGet]
        [AjaxOnly]
        //[RBACAuthorize(PermissionNames.Pages_Users_Create, PermissionNames.Pages_Users_Edit)]
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

        //[RBACAuthorize(PermissionNames.Pages_Users_Create)]
        private GetUserForEditOutput Create()
        {
            return new GetUserForEditOutput()
            {
                User = null,
                //Roles =
            };
        }

        //[RBACAuthorize(PermissionNames.Pages_Users_Edit)]
        private async Task<GetUserForEditOutput> Edit(int userId)
        {
            return await _userService.GetForEditAsync(userId);
        }

        [HttpPost]
        [AjaxOnly]
        //[RBACAuthorize(PermissionNames.Pages_Users_Create, PermissionNames.Pages_Users_Edit)]
        public async Task<ActionResult> CreateOrEdit(CreateOrUpdateUser input)
        {
            if (ModelState.IsValid)
            {
                UserListDto userDto;
                if (input.User.ID.HasValue)
                {
                    userDto = await Edit(input.User, input.RoleIds);
                }
                else
                {
                    userDto = await Create(input.User, input.RoleIds);
                }

                return Json(new Result<UserListDto>()
                {
                    Status = Status.Success,
                    Message = "操作成功",
                    Data = userDto
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

        //[RBACAuthorize(PermissionNames.Pages_Users_Create)]
        public Task<UserListDto> Create(UserEditDto input, List<int> roleIds)
        {
            return _userService.AddAsync(input, roleIds);
        }

        //[RBACAuthorize(PermissionNames.Pages_Users_Edit)]
        public Task<UserListDto> Edit(UserEditDto input, List<int> roleIds)
        {
            return _userService.EditAsync(input, roleIds);
        }

        //[RBACAuthorize(PermissionNames.Pages_Users_Delete)]
        public async Task<JsonResult> Delete(int id)
        {
            if (await _userService.DeleteAsync(id))
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