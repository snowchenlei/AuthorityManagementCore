using Anc.Application.Services;
using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Application.Authorization.Roles
{
    /// <summary>
    /// 角色服务接口
    /// </summary>
    public interface IRoleService : IApplicationService
    {
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        Task<List<RoleListDto>> GetAllRoleListAsync();

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        Task<PagedResultDto<RoleListDto>> GetPagedAsync(GetRoleInput input);

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        Task<GetRoleForEditOutput> GetForEditAsync(int? roleId);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input">信息</param>
        /// <param name="permission">权限</param>
        /// <returns>信息</returns>
        Task<RoleListDto> CreateAsync(RoleEditDto input, List<string> permission);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input">信息</param>
        /// <param name="permission">权限</param>
        /// <returns>信息</returns>
        Task<RoleListDto> EditAsync(RoleEditDto input, List<string> permission);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
    }
}