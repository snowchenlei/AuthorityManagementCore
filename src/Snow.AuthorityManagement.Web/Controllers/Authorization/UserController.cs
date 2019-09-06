using AutoMapper;
using Snow.AuthorityManagement.Common;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Common.Encryption;
using Snow.AuthorityManagement.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Web.Authorization;
using Snow.AuthorityManagement.Web.Library;
using Microsoft.Extensions.Logging;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Application.Authorization.Roles;
using Snow.AuthorityManagement.Application.Authorization.Users;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Web.Models.Users;
using Snow.AuthorityManagement.Web.Startup;
using FluentValidation;
using Anc.AspNetCore.Web.Mvc.Authorization;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    [AncAuthorize(PermissionNames.Pages_Users)]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(
            IMapper mapper, IUserService userService, IRoleService roleService)
        {
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
        }

        [AncAuthorize(PermissionNames.Pages_Users)]
        [ResponseCache(CacheProfileName = "Header")]
        public ActionResult Index()
        {
            ViewBag.AbsoluteUrl = "/User";
            return View();
        }

        [AncAuthorize(PermissionNames.Pages_Users_Query)]
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
        [AncAuthorize(PermissionNames.Pages_Users_Create, PermissionNames.Pages_Users_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            var output = await _userService.GetForEditAsync(id);
            var viewModel = new CreateOrEditUserModalViewModel(output)
            {
                //PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync()
            };
            _mapper.Map(output, viewModel);
            return PartialView(viewModel);
        }

        [AncAuthorize(PermissionNames.Pages_Users_Create)]
        private async Task<GetUserForEditOutput> Create()
        {
            return await _userService.GetForEditAsync(null);
        }

        [AncAuthorize(PermissionNames.Pages_Users_Edit)]
        private async Task<GetUserForEditOutput> Edit(int userId)
        {
            return await _userService.GetForEditAsync(userId);
        }

        [HttpPost]
        [AjaxOnly]
        [AncAuthorize(PermissionNames.Pages_Users_Create, PermissionNames.Pages_Users_Edit)]
        public async Task<ActionResult> CreateOrEdit(CreateOrUpdateUser input)
        {
            if (ModelState.IsValid)
            {
                UserListDto userDto = await _userService.CreateOrEditUserAsync(input);
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

        [AncAuthorize(PermissionNames.Pages_Users_Create)]
        public Task<UserListDto> Create(UserEditDto input, List<int> roleIds)
        {
            return _userService.CreateAsync(input, roleIds);
        }

        [AncAuthorize(PermissionNames.Pages_Users_Edit)]
        public Task<UserListDto> Edit(UserEditDto input, List<int> roleIds)
        {
            return _userService.EditAsync(input, roleIds);
        }

        [AncAuthorize(PermissionNames.Pages_Users_Delete)]
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