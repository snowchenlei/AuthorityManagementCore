using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Anc.Authorization;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Permissions;
using Snow.AuthorityManagement.Application.Authorization.Permissions.Dto;
using Snow.AuthorityManagement.Application.Authorization.Roles;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Model;
using Snow.AuthorityManagement.Web.Library;
using Snow.AuthorityManagement.Web.Models.Roles;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    [AncAuthorize(PermissionNames.Pages_Administration_Roles)]
    public class RoleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IRoleAppService _roleService;
        private readonly IPermissionAppService _permissionService;
        //private readonly PermissionDefinitionContextBase _context;

        public RoleController(
            IRoleAppService roleService, IPermissionAppService permissionService
            //, PermissionDefinitionContextBase context
            , IMapper mapper)
        {
            _roleService = roleService;
            _permissionService = permissionService;
            //_context = PermissionDefinitionContextBase.Context;//context;
            _mapper = mapper;
        }

        [AncAuthorize(PermissionNames.Pages_Administration_Roles)]
        [ResponseCache(CacheProfileName = "Header")]
        public ActionResult Index()
        {
            ViewBag.AbsoluteUrl = "/Role";
            return View();
        }

        [AncAuthorize(PermissionNames.Pages_Administration_Roles_Query)]
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
        [AncAuthorize(PermissionNames.Pages_Administration_Roles_Create, PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            var output = await _roleService.GetForEditAsync(id);
            var viewModel = new CreateOrEditRoleModalViewModel(output)
            {
                //PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync()
            };
            _mapper.Map(output, viewModel);
            return PartialView(viewModel);
        }

        /*[AncAuthorize(PermissionNames.Pages_Roles_Create)]
        private GetRoleForEditOutput Create()
        {
            IReadOnlyList<AncPermission> permissions = _context
                .Permissions.Values.ToImmutableList();
            ICollection<FlatPermissionDto> result = new List<FlatPermissionDto>();
            foreach (AncPermission permission in permissions.Where(p => p.Parent == null))
            {
                result.Add(AddPermission(permission, null));
            }
            return new GetRoleForEditOutput()
            {
                Role = null,
                Permission = Serialization.SerializeObjectCamel(result)
            };
        }

        [AncAuthorize(PermissionNames.Pages_Roles_Edit)]
        private async Task<GetRoleForEditOutput> Edit(int roleId)
        {
            var roleDto = await _roleService.GetForEditAsync(roleId);
            IReadOnlyList<AncPermission> permissions = _context
                .Permissions.Values.ToImmutableList();
            ICollection<FlatPermissionDto> result = new List<FlatPermissionDto>();
            List<Permission> olPermissions = await _permissionService.GetPermissionsByRoleIdAsync(roleId);
            foreach (AncPermission permission in permissions.Where(p => p.Parent == null))
            {
                result.Add(AddPermission(permission, olPermissions));
            }

            return new GetRoleForEditOutput()
            {
                Role = roleDto,
                Permission = Serialization.SerializeObjectCamel(result)
            };
        }*/

        private FlatPermissionDto AddPermission(AncPermission permission
            , IList<Permission> oldPermissions)
        {
            var flatPermission = _mapper.Map<FlatPermissionDto>(permission);
            if (oldPermissions != null && oldPermissions.Any(op => op.Name == permission.Name))
            {
                flatPermission.IsGranted = true;
            }
            foreach (AncPermission child in permission.Children)
            {
                if (flatPermission.Children == null)
                {
                    flatPermission.Children = new List<FlatPermissionDto>();
                }

                flatPermission.Children.Add(AddPermission(child, oldPermissions));
            }

            return flatPermission;
        }

        [HttpPost]
        [AjaxOnly]
        [AncAuthorize(PermissionNames.Pages_Administration_Roles_Create, PermissionNames.Pages_Administration_Roles_Edit)]
        public async Task<ActionResult> CreateOrEdit(CreateOrUpdateRole input)
        {
            if (ModelState.IsValid)
            {
                RoleListDto roleListDto;
                if (input.Role.ID.HasValue)
                {
                    roleListDto = await Edit(input.Role, input.PermissionNames);
                }
                else
                {
                    roleListDto = await Create(input.Role, input.PermissionNames);
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

        [AncAuthorize(PermissionNames.Pages_Administration_Roles_Create)]
        public Task<RoleListDto> Create(RoleEditDto input, List<string> permission)
        {
            return _roleService.CreateAsync(input, permission);
        }

        [AncAuthorize(PermissionNames.Pages_Administration_Roles_Edit)]
        public Task<RoleListDto> Edit(RoleEditDto input, List<string> permission)
        {
            return _roleService.EditAsync(input, permission);
        }

        [AncAuthorize(PermissionNames.Pages_Administration_Roles_Delete)]
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