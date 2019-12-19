using System;
using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    /// <summary>
    /// 用户查询参数
    /// </summary>
    public class GetUsersInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public GetUsersInput()
        {
            Sorting = "ID";
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 创建时间段
        /// </summary>
        public string Date { get; set; }
    }
}