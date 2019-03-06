﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Dto;
using Snow.AuthorityManagement.Core.Dto.User;
using Snow.AuthorityManagement.Core.Entities.Authorization.User;

namespace Snow.AuthorityManagement.IServices
{
    public partial interface IUserServices : IBaseServices<User>
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
        Task<GetUserForEditOutput> GetForEditAsync(int userId);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <param name="roleIds">角色Id</param>
        /// <returns>用户信息</returns>
        Task<UserListDto> AddAsync(UserEditDto input, List<int> roleIds);

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