using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Dto
{
    public class GetMenuForEditOutput
    {
        [Required]
        public MenuEditDto Menu { get; set; }
    }
}