using Snow.AuthorityManagement.Application.Dto;
using System;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class UserListDto : EntityDto
    {
        public string Name { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}