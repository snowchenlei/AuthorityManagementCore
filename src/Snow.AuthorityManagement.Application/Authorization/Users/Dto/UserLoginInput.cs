using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class UserLoginInput
    {
        [Required(ErrorMessage = "{0}是必须的")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}个字符")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0}是必须的")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}个字符")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }
}