using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        [Required]
        public UserEditDto User { get; set; }

        public IEnumerable<RoleSelectDto> Roles { get; set; }
    }
}