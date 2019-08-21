﻿using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class UserEditDto
    {
        public int? ID { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        //[StringLength(50)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        //[StringLength(50)]
        //[Display(Name = "密码")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }
    }
}