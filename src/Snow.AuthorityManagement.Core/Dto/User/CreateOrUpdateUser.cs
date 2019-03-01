using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Core.Dto.User
{
    public class CreateOrUpdateUser
    {
        [Required]
        public UserEditDto User { get; set; }

        [Display(Name = "角色")]
        public List<int> RoleIds { get; set; }
    }
}