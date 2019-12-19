using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Dto
{
    public class GetRoleInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public GetRoleInput()
        {
            Sorting = "Sort";
            Order = OrderType.DESC;
        }

        public string DisplayName { get; set; }
        public string Date { get; set; }
    }
}