﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Application.Dto;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Enum;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Data;

namespace Snow.AuthorityManagement.Application.Authorization.Users
{
    public partial class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DbContext CurrentContext;
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<User> CurrentRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;

        public UserService(
            IMapper mapper
            , AuthorityManagementContext context
            , IBaseRepository<User> currentRepository
            , IConfiguration configuration
            , IBaseRepository<Role> roleRepository
            , IBaseRepository<UserRole> userRoleRepository)
        {
            _mapper = mapper;
            CurrentContext = context;
            _configuration = configuration;
            _roleRepository = roleRepository;
            CurrentRepository = currentRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        public async Task<PagedResultDto<UserListDto>> GetPagedAsync(GetUserInput input)
        {
            List<string> wheres = new List<string>();
            List<object> parameters = new List<object>();
            int index = 0;
            if (!string.IsNullOrEmpty(input.UserName))
            {
                wheres.Add($"UserName.Contains(@{index++})");
                parameters.Add(input.UserName);
            }

            if (!string.IsNullOrEmpty(input.Date))
            {
                DateTime[] date = Array.ConvertAll(input.Date
                    .Split(new[] { '~' }, StringSplitOptions.RemoveEmptyEntries)
                    , DateTime.Parse);
                wheres.Add($"AddTime > (@{index++}) AND AddTime < (@{index++})");
                parameters.Add(date[0]);
                parameters.Add(date[1]);
            }
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                input.Sorting = input.Sorting + (input.Order == OrderType.ASC ? " ASC" : " DESC");
            }
            var result = await CurrentRepository
                .GetPagedAsync(input.PageIndex, input.PageSize,
                    string.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
            return new PagedResultDto<UserListDto>()
            {
                Items = _mapper.Map<List<UserListDto>>(result.Item1),
                TotalCount = result.Item2
            };
        }

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public async Task<GetUserForEditOutput> GetForEditAsync(int? userId)
        {
            UserEditDto userEditDto = null;
            List<RoleSelectDto> roles = await _roleRepository
                .LoadEntities(r => true)
                .Select(r => new RoleSelectDto
                {
                    ID = r.ID,
                    Name = "rolw" + new Random().Next(),
                    DisplayName = r.Name
                }).ToListAsync();
            var output = new GetUserForEditOutput()
            {
                Roles = roles
            };
            if (userId.HasValue)
            {
                User user = await CurrentRepository
                    .FirstOrDefaultAsync(u => u.ID == userId.Value);
                userEditDto = _mapper.Map<UserEditDto>(user);
                List<UserRole> userRoles = await _userRoleRepository
                    .LoadListAsync(ur => ur.UserID == userId.Value);
                foreach (var userRoleDto in roles)
                {
                    userRoleDto.Selected = userRoles.Any(a => a.RoleID == userRoleDto.ID);
                }
            }
            output.User = userEditDto;
            return output;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UserLoginOutput> LoginAsync(UserLoginInput input)
        {
            User user = await CurrentRepository
                .FirstOrDefaultAsync(u => u.UserName == input.UserName) ??
                        throw new UserFriendlyException("用户名和密码不匹配");

            if (user.Password != input.Password)
            {
                throw new UserFriendlyException("用户名和密码不匹配");
            }

            if (!user.CanUse)
            {
                throw new UserFriendlyException("当前账号不可用，请联系管理员");
            }
            return _mapper.Map<UserLoginOutput>(user);
        }

        /// <summary>
        /// 添加修改用户
        /// </summary>
        /// <param name="input">信息</param>
        /// <returns>用户列表数据</returns>
        public async Task<UserListDto> CreateOrEditUserAsync(CreateOrUpdateUser input)
        {
            UserListDto userDto = null;
            if (input.User.ID.HasValue)
            {
                userDto = await EditAsync(input.User, input.RoleIds);
            }
            else
            {
                userDto = await CreateAsync(input.User, input.RoleIds);
            }
            return userDto;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input">信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>信息</returns>
        public async Task<UserListDto> CreateAsync(UserEditDto input, List<int> roleIds)
        {
            #region 用户

            if (await CurrentRepository.IsExistsAsync(u => u.UserName == input.UserName))
            {
                throw new UserFriendlyException("用户名已存在");
            }
            User user = _mapper.Map<User>(input);
            user.CanUse = true;
            user.Password = _configuration["AppSetting:DefaultPassword"];
            user = await CurrentRepository.AddAsync(user);

            #endregion 用户

            #region 角色

            if (roleIds != null && roleIds.Count > 0)
            {   // 删除用户的所有角色，添加用户现有角色
                List<UserRole> userRoles = await _userRoleRepository.LoadListAsync(a => a.UserID == user.ID);
                _userRoleRepository.DeleteRange(userRoles);
                await _userRoleRepository.AddRangeAsync(roleIds
                    .Select(r => new UserRole()
                    {
                        UserID = user.ID,
                        RoleID = r
                    }));
            }

            #endregion 角色

            await CurrentContext.SaveChangesAsync();
            return _mapper.Map<UserListDto>(user);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        public async Task<UserListDto> EditAsync(UserEditDto input, List<int> roleIds)
        {
            #region 用户

            User oldUser = await CurrentRepository
                    .FirstOrDefaultAsync(u => u.ID == input.ID.Value);
            if (oldUser == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            User user = _mapper.Map(input, oldUser);
            CurrentRepository.Edit(user);

            #endregion 用户

            #region 角色

            List<UserRole> userRoles = await _userRoleRepository
                    .LoadListAsync(ur => ur.UserID == input.ID);
            _userRoleRepository.DeleteRange(userRoles);
            if (roleIds.Count > 0)
            {
                await _userRoleRepository.AddRangeAsync(roleIds
                    .Select(r => new UserRole
                    {
                        UserID = user.ID,
                        RoleID = r
                    }));
            }

            #endregion 角色

            if (await CurrentContext.SaveChangesAsync() <= 0)
            {
                throw new UserFriendlyException("修改失败");
            }
            return _mapper.Map<UserListDto>(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            User user = await CurrentRepository.FirstOrDefaultAsync(a => a.ID == id)
                ?? throw new UserFriendlyException("用户不存在");

            CurrentRepository.Delete(user);
            return await CurrentContext.SaveChangesAsync() > 0;
        }
    }
}