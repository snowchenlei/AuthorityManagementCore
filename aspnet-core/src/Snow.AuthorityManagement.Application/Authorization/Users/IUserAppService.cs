using Anc.Application.Services;
using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Application.Authorization.Users
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public partial interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        Task<PagedResultDto<UserListDto>> GetUserPagedAsync(GetUsersInput input);

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        Task<UserEditDto> GetUserForEditAsync(int userId);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<RoleSelectDto>> GetUserRoleSelectAsync(int? userId);

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        Task<GetUserForEditOutput> GetUserInfoForEditAsync(int? userId);

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<UserLoginOutput> LoginAsync(string userName, string password);

        /// <summary>
        /// 添加修改用户
        /// </summary>
        /// <param name="input">信息</param>
        /// <returns>用户列表数据</returns>
        Task<UserListDto> CreateOrEditUserAsync(CreateOrUpdateUser input);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        Task<UserListDto> CreateUserAsync(UserEditDto input, List<int> roleIds);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        Task<UserListDto> EditUserAsync(UserEditDto input, List<int> roleIds);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        Task<bool> DeleteUserAsync(int id);
    }
}