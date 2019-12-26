using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Anc.Domain.Entities;
using Anc.Domain.Repositories;
using Anc.Users;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Application.Authorization.Permissions.Dto;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;

namespace Snow.AuthorityManagement.Application.Authorization.Permissions
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public class PermissionAppService : IPermissionAppService
    {
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILambdaRepository<UserRole> _userRoleRepository;

        public PermissionAppService(IMapper mapper
            , IPermissionRepository permissionRepository
            , ILambdaRepository<UserRole> userRoleRepository)
        {
            this._mapper = mapper;
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="roleNames">角色名称集合</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetUserPermissionsAsync(string[] roleNames)
        {
            List<AncPermission> permissions = await _permissionRepository.GetListAsync(roleNames);
            return permissions.Select(a => a.Name);
        }
    }
}