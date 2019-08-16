using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Application.Authorization.Users
{
    public partial interface IUserService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        Task<PagedResultDto<UserListDto>> GetPagedAsync(GetUserInput input);

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        Task<GetUserForEditOutput> GetForEditAsync(int? userId);

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserLoginOutput> LoginAsync(UserLoginInput input);

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
        Task<UserListDto> CreateAsync(UserEditDto input, List<int> roleIds);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        Task<UserListDto> EditAsync(UserEditDto input, List<int> roleIds);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
    }
}