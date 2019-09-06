using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Web.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XsrfTokenController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public XsrfTokenController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

            return new ObjectResult(new
            {
                token = tokens.RequestToken,
                tokenName = tokens.HeaderName
            });
        }
    }
}