using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anc.Application.Services.Dto;
using Anc.Domain.Model;
using Anc.Domain.Repositories;
using Anc.Domain.Uow;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Application.Dto;
using Snow.AuthorityManagement.Common.Encryption;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Core.Authorization.Users;
using Snow.AuthorityManagement.Core.Authorization.Users.DomainService;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Data;

namespace Snow.AuthorityManagement.Application.Authorization.Users
{
    public partial class UserService : IUserService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthorityManagementContext CurrentContext;
        private readonly IConfiguration _configuration;
        private readonly IUserManager _userManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserService(
            IMapper mapper
            , IUnitOfWork unitOfWork
            , IUserManager userManager
            , AuthorityManagementContext context
            , IUserRepository userRepository
            , IConfiguration configuration
            , IRepository<Role> roleRepository
            , IUserRoleRepository userRoleRepository
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            CurrentContext = context;
            _userRepository = userRepository;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        public async Task<PagedResultDto<UserListDto>> GetUserPagedAsync(GetUserInput input)
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
            var result = await _userRepository
                .GetPagedAsync(input.PageIndex, input.PageSize,
                    string.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
            return new PagedResultDto<UserListDto>()
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                Items = _mapper.Map<List<UserListDto>>(result.Item1),
                TotalCount = result.Item2
            };
        }

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public async Task<UserEditDto> GetUserForEditAsync(int userId)
        {
            User user = await _userRepository.GetAsync(userId);
            UserEditDto userEditDto = _mapper.Map<UserEditDto>(user);
            return userEditDto;
        }

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public async Task<GetUserForEditOutput> GetUserForEditAsync(int? userId)
        {
            UserEditDto userEditDto = null;
            RoleSelectDto[] roles = (await _roleRepository
                .GetAllEnumerableAsync())
                .Select(r => new RoleSelectDto
                {
                    ID = r.ID,
                    Name = "rolw" + new Random().Next(),
                    DisplayName = r.Name
                }).ToArray();
            var output = new GetUserForEditOutput()
            {
                Roles = roles
            };
            if (userId.HasValue)
            {
                User user = await _userRepository.GetAsync(userId.Value);
                userEditDto = _mapper.Map<UserEditDto>(user);
                List<UserRole> userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(userId.Value);
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
        public async Task<UserLoginOutput> LoginAsync(string userName, string password)
        {
            User user = await _userRepository.GetUserByUserNameAsync(userName) ??
                        throw new UserFriendlyException("用户名和密码不匹配");

            if (user.Password != Md5Encryption.Encrypt(password))
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
                userDto = await EditUserAsync(input.User, input.RoleIds);
            }
            else
            {
                userDto = await CreateUserAsync(input.User, input.RoleIds);
            }
            return userDto;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input">信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>信息</returns>
        [UnitOfWork]
        public virtual async Task<UserListDto> CreateUserAsync(UserEditDto input, List<int> roleIds)
        {
            #region 用户

            if (await _userRepository.IsExistsByUserNameAsync(input.UserName))
            {
                throw new UserFriendlyException("用户名已存在");
            }

            User user = _mapper.Map<User>(input);
            user.CanUse = true;
            user.Password = _configuration["AppSetting:DefaultPassword"];
            user.LastModificationTime = user.CreationTime;
            user.ID = await _userRepository.InsertAndGetIdAsync(user);

            #endregion 用户

            await SetUserRoleAsync(user.ID, roleIds);
            return _mapper.Map<UserListDto>(user);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        [UnitOfWork]
        public virtual async Task<UserListDto> EditUserAsync(UserEditDto input, List<int> roleIds)
        {
            #region 用户

            int userId = input.ID.Value;
            User oldUser = await _userRepository.GetAsync(userId);
            if (oldUser == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            User user = _mapper.Map(input, oldUser);
            user.LastModificationTime = DateTime.Now;
            await _userRepository.UpdateAsync(user);

            #endregion 用户

            await SetUserRoleAsync(user.ID, roleIds);
            return _mapper.Map<UserListDto>(user);
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleIds">角色ID集合</param>
        /// <returns></returns>
        private async Task SetUserRoleAsync(int userID, IEnumerable<int> roleIds)
        {
            List<UserRole> userRoles = await _userRoleRepository
                    .GetUserRolesByUserIdAsync(userID);
            await _userRoleRepository.DeleteAsync(userRoles);
            if (roleIds != null && roleIds.Any())
            {
                await _userRoleRepository.InsertAsync(roleIds
                    .Select(r => new UserRole
                    {
                        UserID = userID,
                        RoleID = r
                    }));
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<bool> DeleteUserAsync(int id)
        {
            User user = await _userRepository.GetAsync(id)
                ?? throw new UserFriendlyException("用户不存在");
            List<UserRole> userRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(id);
            await _userRoleRepository.DeleteAsync(userRoles);
            await _userRepository.DeleteAsync(user);
            return true;
        }

        public Task<DateTime?> GetLastModificationTimeAsync()
        {
            return _userManager.GetLastModificationTimeAsync();
        }
    }
}