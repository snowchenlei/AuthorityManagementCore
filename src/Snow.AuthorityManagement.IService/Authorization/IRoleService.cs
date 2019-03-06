using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Dto;
using Snow.AuthorityManagement.Core.Dto.Role;

namespace Snow.AuthorityManagement.IService.Authorization
{
    public interface IRoleService
    {
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
        Task<GetRoleForEditOutput> GetForEditAsync(int roleId);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input">信息</param>
        /// <returns>信息</returns>
        Task<RoleListDto> AddAsync(RoleEditDto input);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input">信息</param>
        /// <returns>信息</returns>
        Task<RoleListDto> EditAsync(RoleEditDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
    }
}