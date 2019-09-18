using Snow.AuthorityManagement.Application.Dto;
using System;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Dto
{
    public class MenuListDto : EntityDto
    {
        public string Name { get; set; }

        public string PermissionName { get; set; }

        public string Icon { get; set; }

        public string Route { get; set; }

        public int Sort { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string ParentName { get; set; }
    }
}