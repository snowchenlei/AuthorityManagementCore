using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Core.Dto.User
{
    public class GetUserForEditOutput
    {
        [Required]
        public UserEditDto User { get; set; }

        public Dictionary<int, string> Roles { get; set; }
    }
}