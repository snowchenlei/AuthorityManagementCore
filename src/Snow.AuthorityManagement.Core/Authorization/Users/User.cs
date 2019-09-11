using Anc.Domain.Entities;
using Anc.Domain.Entities.Auditing;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.UserRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Core.Entities.Authorization
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : Entity, IHasModificationTime, IHasCreationTime
    {
        public User()
        {
            CreationTime = DateTime.Now;
        }

        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(50)]
        [Description("用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(50)]
        [Description("密码")]
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [StringLength(11, MinimumLength = 11)]
        [Description("手机号")]
        public string PhoneNumber { get; set; }

        public bool CanUse { get; set; }

        public DateTime? LastModificationTime { get; set; }
        public DateTime CreationTime { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}