using Microsoft.AspNetCore.Mvc.Rendering;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Mvc.Models.Menus
{
    public class CreateOrEditMenuModalViewModel
    {
        public MenuEditDto Menu { get; set; }

        public List<SelectListItem> MenuList { get; set; }
    }
}