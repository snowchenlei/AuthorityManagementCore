using Anc.AspNetCore.Web.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Users;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Controllers.Authorization
{
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        //[AncAuthorize(PermissionNames.Pages_Users_Create, PermissionNames.Pages_Users_Edit)]
        public async Task<ActionResult> CreateOrEdit(CreateOrUpdateUser input)
        {
            UserListDto userDto = await _userService.CreateOrEditUserAsync(input);
            return Ok(new Result<UserListDto>()
            {
                Status = Status.Success,
                Message = "操作成功",
                Data = userDto
            });
        }
    }
}