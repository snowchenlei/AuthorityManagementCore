using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cl.AuthorityManagement.Entity;
using Cl.AuthorityManagement.IServices;
using Cl.AuthorityManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Cl.AuthorityManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationController : ControllerBase
    {
        private readonly IModuleServices ModuleServices = null;
        private readonly IUserInfoServices UserInfoServices = null;
        private readonly IDistributedCache Cache = null;
        public NavigationController(
            IModuleServices moduleServices,
            IUserInfoServices userInfoServices,
            IDistributedCache cache)
        {
            ModuleServices = moduleServices;
            UserInfoServices = userInfoServices;
            Cache = cache;
        }
        [HttpGet]
        public ActionResult<IEnumerable<object>> Get()
        {
            string username = Cache.GetString("user") ?? "admin";
            UserInfo user = UserInfoServices.LoadFirst(u => u.UserName == username);
            List<Module> modules = ModuleServices.LoadSelectModules(user);
            return Ok(new Result<object>
            {
                State = 1,
                Data = modules
                .Where(m => m.Parent == null)
                .Select(m => new
                {
                    m.ID,
                    m.Name,
                    m.IconName,
                    m.Url,
                    m.Sort,
                    Children = modules
                                .Where(c => c.Parent?.ID == m.ID)
                                .Select(c => new
                                {
                                    c.ID,
                                    c.Name,
                                    c.IconName,
                                    c.Url,
                                    c.Sort,
                                    Children = new object[0]
                                })
                })
            });
        }
    }
}