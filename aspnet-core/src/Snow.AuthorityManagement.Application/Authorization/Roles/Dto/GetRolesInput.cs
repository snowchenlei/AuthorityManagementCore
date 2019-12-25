using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Dto
{
    public class GetRolesInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public GetRolesInput()
        {
            Sorting = "Sort";
            Order = OrderType.DESC;
        }

        public string DisplayName { get; set; }
        public string Date { get; set; }
    }
}