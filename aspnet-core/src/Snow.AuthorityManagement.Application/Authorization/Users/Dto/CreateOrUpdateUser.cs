using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class CreateOrUpdateUser
    {
        public CreateOrUpdateUser()
        {
            RoleIds = new List<int>();
        }

        [Display(Name = "用户")]
        public UserEditDto User { get; set; }

        [Display(Name = "角色")]
        public List<int> RoleIds { get; set; }
    }
}