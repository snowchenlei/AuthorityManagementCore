using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Core.Entities.Authorization
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : Entity
    {
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
    }
}