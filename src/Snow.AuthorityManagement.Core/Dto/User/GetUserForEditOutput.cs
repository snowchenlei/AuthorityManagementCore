using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Snow.AuthorityManagement.Core.Dto.Role;

namespace Snow.AuthorityManagement.Core.Dto.User
{
    public class GetUserForEditOutput
    {
        [Required]
        public UserEditDto User { get; set; }

        public IEnumerable<RoleSelectDto> Roles { get; set; }
    }
}