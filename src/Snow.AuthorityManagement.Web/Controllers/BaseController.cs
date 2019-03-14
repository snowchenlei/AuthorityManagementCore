using Autofac;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snow.AuthorityManagement.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IEnumerable<object> ModelStateToJson()
        {
            var errors = ModelState.Where(m => m.Value.Errors.Any())
                .Select(m => new
                {
                    m.Key,
                    Errors = String.Join("", m.Value.Errors.Select(e => e.ErrorMessage))
                });
            return errors;
        }

        protected IEnumerable<string> ModelStateToArray()
        {
            var errors = ModelState.Where(m => m.Value.Errors.Any())
                .Select(m => String.Join("", m.Value.Errors.Select(e => e.ErrorMessage)));
            return errors;
        }
    }
}