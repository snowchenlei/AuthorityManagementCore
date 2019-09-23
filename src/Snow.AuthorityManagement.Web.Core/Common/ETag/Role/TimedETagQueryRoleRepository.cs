using Anc.Application.Services.Dto;
using CacheCow.Server;
using CacheManager.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Core.Authorization.Roles.DomainService;
using Snow.AuthorityManagement.Core.Authorization.Users.DomainService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Core.Common.ETag.Role
{
    public class TimedETagQueryRoleRepository : ITimedETagQueryProvider<PagedResultDto<RoleListDto>>, ITimedETagQueryProvider<GetRoleForEditOutput>
    {
        private readonly ICacheManager<DateTime?> _cache;
        private readonly IRoleManager _roleManager;

        public TimedETagQueryRoleRepository(ICacheManager<DateTime?> cache
            , IRoleManager userManager)
        {
            _cache = cache;
            _roleManager = userManager;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<TimedEntityTagHeaderValue> QueryAsync(HttpContext context)
        {
            int? id = null;
            var routeData = context.GetRouteData();
            if (routeData.Values.ContainsKey("id"))
                id = Convert.ToInt32(routeData.Values["id"]);

            if (id.HasValue)
            {
                DateTime? time = _cache.Get(string.Format(AuthorityManagementConsts.RoleResponseCache, id));
                if (time.HasValue)
                {
                    return Task.FromResult(new TimedEntityTagHeaderValue(new DateTimeOffset(time.Value).ToETagString()));
                }
                else
                {
                    return Task.FromResult((TimedEntityTagHeaderValue)null);
                }
            }
            else // all cars
            {
                DateTime? time = _cache.Get(AuthorityManagementConsts.RoleLastResponseCache);
                if (time == null)
                {
                    return Task.FromResult((TimedEntityTagHeaderValue)null);
                }
                return Task.FromResult(new TimedEntityTagHeaderValue(new DateTimeOffset(time.Value).ToETagString(_roleManager.GetCount())));
            }
        }
    }
}