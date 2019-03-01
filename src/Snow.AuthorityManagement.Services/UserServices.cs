using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.IRepository;
using Snow.AuthorityManagement.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Snow.AuthorityManagement.Core.Dto;
using Snow.AuthorityManagement.Core.Dto.User;
using Snow.AuthorityManagement.Core.Entities.Authorization.User;
using Snow.AuthorityManagement.Core.Enum;
using Snow.AuthorityManagement.Core.Exception;

namespace Snow.AuthorityManagement.Services
{
    public partial class UserServices : BaseServices<User>, IUserServices
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository = null;

        public UserServices(
            IMapper mapper
            , AuthorityManagementContext context
            , IBaseRepository<User> baseRepository
            , IUserRepository userRepository
            , IConfiguration configuration) : base(context, baseRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _configuration = configuration;
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
            if (!String.IsNullOrEmpty(input.UserName))
            {
                wheres.Add($"UserName.Contains(@{index++})");
                parameters.Add(input.UserName);
            }

            if (!String.IsNullOrEmpty(input.Date))
            {
                DateTime[] date = Array.ConvertAll(input.Date
                    .Split(new[] { '~' }, StringSplitOptions.RemoveEmptyEntries)
                    , DateTime.Parse);
                wheres.Add($"AddTime > (@{index++}) AND AddTime < (@{index++})");
                parameters.Add(date[0]);
                parameters.Add(date[1]);
            }
            if (!String.IsNullOrEmpty(input.Sort))
            {
                input.Sorting = input.Sort + (input.Order == OrderType.ASC ? " ASC" : " DESC");
            }
            var result = await _userRepository
                .GetPagedAsync(input.PageIndex, input.PageSize,
                    String.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
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
        public async Task<GetUserForEditOutput> GetForEditAsync(int userId)
        {
            User user = await _userRepository.FirstOrDefaultAsync(u => u.ID == userId);
            return new GetUserForEditOutput()
            {
                User = _mapper.Map<UserEditDto>(user)
            };
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        public async Task<UserListDto> AddAsync(UserEditDto input, List<int> roleIds)
        {
            if (await _userRepository.IsExistsAsync(u => u.UserName == input.UserName))
            {
                throw new UserFriendlyException("用户名已存在");
            }
            User user = Mapper.Map<User>(input);
            user.CanUse = true;
            user.Password = _configuration["AppSetting:DefaultPassword"];
            user = await _userRepository.AddAsync(user);
            await CurrentContext.SaveChangesAsync();
            //TODO:角色操作
            //if (roleIds != null && roleIds.Any())
            //{
            //    List<AncUserRoles> userRoles = new List<AncUserRoles>();
            //    foreach (int roleId in roleIds)
            //    {
            //        userRoles.Add(new AncUserRoles()
            //        {
            //            UserId = user.Id,
            //            RoleId = roleId,
            //            CreationTime = DateTime.Now
            //        });
            //    }
            //    _ancUserRolesController.BetchAddEntity(userRoles);
            //}
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
            User oldUser = await _userRepository
                .FirstOrDefaultAsync(u => u.ID == input.ID.Value);
            if (oldUser == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            User user = _mapper.Map(input, oldUser);
            _userRepository.Edit(user);
            if (await CurrentContext.SaveChangesAsync() <= 0)
            {
                throw new UserFriendlyException("修改失败");
            }
            //TODO:角色操作
            return _mapper.Map<UserListDto>(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            User user = await _userRepository.FirstOrDefaultAsync(a => a.ID == id)
                ?? throw new UserFriendlyException("用户不存在");

            _userRepository.Delete(user);
            return await CurrentContext.SaveChangesAsync() > 0;
        }
    }
}