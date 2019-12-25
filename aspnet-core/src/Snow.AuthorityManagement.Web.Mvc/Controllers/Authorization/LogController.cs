using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Web.Controllers;

namespace Snow.AuthorityManagement.Web.Mvc.Controllers.Authorization
{
    [Authorize(PermissionNames.Pages_Administration_Logs)]
    public class LogController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}