using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class UserEditDto
    {
        public int? ID { get; set; }

        //[Display(Name = "姓名")]
        //[StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        //[StringLength(50)]
        //[Display(Name = "用户名")]
        //[Required(ErrorMessage = "{0}不能为空")]
        public string UserName { get; set; }

        //[StringLength(50)]
        //[Display(Name = "密码")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        //[StringLength(11, MinimumLength = 11)]
        //[Display(Name = "手机号")]
        public string PhoneNumber { get; set; }
    }
}