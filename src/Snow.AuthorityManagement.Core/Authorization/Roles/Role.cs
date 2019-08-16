using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using Snow.AuthorityManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snow.AuthorityManagement.Core.Authorization.Roles
{
    /// <summary>
    /// 用户
    /// </summary>
    public class Role : Entity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}