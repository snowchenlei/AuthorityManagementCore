using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Models.Account
{
    public class LoginViewModel
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

        public UserLoginInput User { get; set; }

        [Required]
        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }
}