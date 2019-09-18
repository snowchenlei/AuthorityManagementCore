using Anc.Application.Services.Dto;
using Anc.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Dto
{
    public class MenuEditDto : NullableIdDto
    {
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "权限名称")]
        public string PermissionName { get; set; }

        [Display(Name = "图标")]
        public string Icon { get; set; }

        [Display(Name = "路由")]
        public string Route { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }

        [Display(Name = "父Id")]
        public int? ParentId { get; set; }
    }
}