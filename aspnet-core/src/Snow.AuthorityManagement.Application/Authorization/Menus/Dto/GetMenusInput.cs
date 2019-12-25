using System;
using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Dto
{
    public class GetMenusInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public GetMenusInput()
        {
            Order = OrderType.DESC;
            Sorting = "Sort";
        }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        public int? ParentID { get; set; }
    }
}