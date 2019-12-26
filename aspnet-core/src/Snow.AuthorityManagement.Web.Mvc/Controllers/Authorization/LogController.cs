using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Web.Controllers;

namespace Snow.AuthorityManagement.Web.Mvc.Controllers.Authorization
{
    [Authorize(PermissionNames.Pages_Administration_Logs)]
    public class LogController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.LogLevel = new List<SelectListItem>()
            {
                new SelectListItem("Verbose","Verbose"),
                new SelectListItem("Debug","Debug"),
                new SelectListItem("Information","Information"),
                new SelectListItem("Warning","Warning"),
                new SelectListItem("Error","Error"),
                new SelectListItem("Fatal","Fatal"),
            };
            return View();
        }
    }
}