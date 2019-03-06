using AutoMapper;
using Snow.AuthorityManagement.Core.Dto;
using Snow.AuthorityManagement.Core.Dto.Role;
using Snow.AuthorityManagement.Core.Entities;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Enum;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Dto.User;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.IService.Authorization;

namespace Snow.AuthorityManagement.Service.Authorization
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly IMapper _mapper;

        public RoleService(
            IMapper mapper
            , AuthorityManagementContext context
            , IBaseRepository<Role> currentRepository) : base(context, currentRepository)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        public async Task<PagedResultDto<RoleListDto>> GetPagedAsync(GetRoleInput input)
        {
            List<string> wheres = new List<string>();
            List<object> parameters = new List<object>();
            int index = 0;
            if (!String.IsNullOrEmpty(input.Name))
            {
                wheres.Add($"Name.Contains(@{index++})");
                parameters.Add(input.Name);
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
            if (!String.IsNullOrEmpty(input.Sorting))
            {
                input.Sorting = input.Sorting + (input.Order == OrderType.ASC ? " ASC" : " DESC");
            }
            var result = await CurrentRepository
                .GetPagedAsync(input.PageIndex, input.PageSize,
                    String.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
            return new PagedResultDto<RoleListDto>()
            {
                Items = _mapper.Map<List<RoleListDto>>(result.Item1),
                TotalCount = result.Item2
            };
        }

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public async Task<GetRoleForEditOutput> GetForEditAsync(int roleId)
        {
            Role role = await CurrentRepository.FirstOrDefaultAsync(u => u.ID == roleId);
            return new GetRoleForEditOutput()
            {
                Role = _mapper.Map<RoleEditDto>(role)
            };
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input">信息</param>
        /// <returns>信息</returns>
        public async Task<RoleListDto> AddAsync(RoleEditDto input)
        {
            if (await CurrentRepository.IsExistsAsync(u => u.Name == input.Name))
            {
                throw new UserFriendlyException("角色名已存在");
            }
            Role role = Mapper.Map<Role>(input);
            role = await CurrentRepository.AddAsync(role);
            await CurrentContext.SaveChangesAsync();
            return _mapper.Map<RoleListDto>(role);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input">信息</param>
        /// <returns>信息</returns>
        public async Task<RoleListDto> EditAsync(RoleEditDto input)
        {
            Role oldRole = await CurrentRepository
                .FirstOrDefaultAsync(u => u.ID == input.ID.Value);
            if (oldRole == null)
            {
                throw new UserFriendlyException("角色不存在");
            }
            Role role = _mapper.Map(input, oldRole);
            CurrentRepository.Edit(role);
            if (await CurrentContext.SaveChangesAsync() <= 0)
            {
                throw new UserFriendlyException("修改失败");
            }
            return _mapper.Map<RoleListDto>(role);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            Role role = await CurrentRepository.FirstOrDefaultAsync(a => a.ID == id)
                        ?? throw new UserFriendlyException("角色不存在");

            CurrentRepository.Delete(role);
            return await CurrentContext.SaveChangesAsync() > 0;
        }
    }
}