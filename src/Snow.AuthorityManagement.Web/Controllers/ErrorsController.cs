using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Snow.AuthorityManagement.Web.Controllers
{
    public class ErrorsController : BaseController
    {
        private IHostingEnvironment _env;

        public ErrorsController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Route("errors/{statusCode}")]
        public IActionResult CustomError(int statusCode)
        {
            var filePath = $"{_env.WebRootPath}/errors/{(statusCode == 404 ? 404 : 500)}.html";
            return new PhysicalFileResult(filePath, new MediaTypeHeaderValue("text/html"));
        }
    }
}