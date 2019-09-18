﻿using System;
using Anc.Application.Services.Dto;
using Snow.AuthorityManagement.Application.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Dto
{
    public class GetMenuInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public GetMenuInput()
        {
            Sorting = "ID";
        }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
    }
}