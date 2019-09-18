using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Dto
{
    public class CreateOrUpdateMenu
    {
        public CreateOrUpdateMenu()
        {
        }

        [Display(Name = "用户")]
        public MenuEditDto Menu { get; set; }
    }
}