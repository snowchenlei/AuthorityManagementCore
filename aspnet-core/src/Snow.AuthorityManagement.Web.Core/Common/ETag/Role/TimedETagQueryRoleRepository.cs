using Anc.Application.Services.Dto;
using CacheCow.Server;
using CacheManager.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IDistributedCache _cache;
        private readonly IRoleManager _roleManager;

        public TimedETagQueryRoleRepository(IDistributedCache cache
            , IRoleManager userManager)
        {
            _cache = cache;
            _roleManager = userManager;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<TimedEntityTagHeaderValue> QueryAsync(HttpContext context)
        {
            int? id = null;
            var routeData = context.GetRouteData();
            if (routeData.Values.ContainsKey("id"))
                id = Convert.ToInt32(routeData.Values["id"]);

            if (id.HasValue)
            {
                string time = await _cache.GetStringAsync(string.Format(AuthorityManagementConsts.RoleResponseCache, id));
                if (time != null)
                {
                    return new TimedEntityTagHeaderValue(new DateTimeOffset(Convert.ToDateTime(time)).ToETagString());
                }
                else
                {
                    return null;
                }
            }
            else // all cars
            {
                string time = await _cache.GetStringAsync(AuthorityManagementConsts.RoleLastResponseCache);
                if (time == null)
                {
                    return null;
                }
                return new TimedEntityTagHeaderValue(new DateTimeOffset(Convert.ToDateTime(time)).ToETagString(_roleManager.GetCount()));
            }
        }
    }
}