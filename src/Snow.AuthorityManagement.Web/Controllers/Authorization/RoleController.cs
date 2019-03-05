using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Web.Authorization;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    //[RBACAuthorize(PermissionNames.Pages_Roles)]
    public class RoleController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}